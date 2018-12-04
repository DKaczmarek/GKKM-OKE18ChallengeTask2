from itertools import chain
from collections import Counter
import pycrfsuite
import ast

from sklearn.metrics import classification_report
from sklearn.preprocessing import LabelBinarizer

import CRFModule.crftools.databuilder as data_builder


def print_transitions(trans_features):
    for (label_from, label_to), weight in trans_features:
        print("%-6s -> %-7s %0.6f" % (label_from, label_to, weight))


def bio_classification_report(y_true, predictions):
    """
    Classification report for a list of BIO-encoded sequences.
    It computes token-level metrics and discards "O" labels.

    Note that it requires scikit-learn 0.15+ (or a version from github master)
    to calculate averages properly!
    """
    lb = LabelBinarizer()
    y_true_combined = lb.fit_transform(list(chain.from_iterable(y_true)))
    y_pred_combined = lb.transform(list(chain.from_iterable(predictions)))

    tagset = set(lb.classes_) - {'O'}
    tagset = sorted(tagset, key=lambda tag: tag.split('-', 1)[::-1])
    class_indices = {cls: idx for idx, cls in enumerate(lb.classes_)}

    return classification_report(
        y_true_combined,
        y_pred_combined,
        labels=[class_indices[cls] for cls in tagset],
        target_names=tagset,
    )


def run_train_process(samples_path, model_path):
    data_path = samples_path
    data = []
    with open(data_path, 'r') as file:
        for line in file:
            data.append(ast.literal_eval(line))

    train, test = data_builder.split_train_test(data, 0.7)

    x_train = [data_builder.sent2features(s) for s in train]
    y_train = [data_builder.sent2labels(s) for s in train]

    x_test = [data_builder.sent2features(s) for s in test]
    y_test = [data_builder.sent2labels(s) for s in test]

    trainer = pycrfsuite.Trainer(verbose=False)

    for xseq, yseq in zip(x_train, y_train):
        trainer.append(xseq, yseq)

    trainer.set_params({
        'c1': 1.0,  # coefficient for L1 penalty
        'c2': 1e-3,  # coefficient for L2 penalty
        'max_iterations': 50,  # stop earlier

        # include transitions that are possible, but not observed
        'feature.possible_transitions': True
    })
    trainer.train(model_path)
    print(trainer.logparser.last_iteration)

    # evaluate model
    tagger = pycrfsuite.Tagger()
    tagger.open(model_path)
    y_pred = [tagger.tag(xseq) for xseq in x_test]
    print(bio_classification_report(y_test, y_pred))

    info = tagger.info()
    print("Top likely transitions:")
    print_transitions(Counter(info.transitions).most_common(15))

    print("\nTop unlikely transitions:")
    print_transitions(Counter(info.transitions).most_common()[-15:])


if __name__ == '__main__':
    temp_path = './data/samples_v2.txt'
    run_train_process(temp_path)
