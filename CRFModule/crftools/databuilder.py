import random


def word2features(sent, i):
    word = sent[i][0]
    postag = sent[i][1]
    features = {
        'bias': 1.0,
        'word.lower': word.lower(),
        'word.isupper': word.isupper(),
        'word.istitle': word.istitle(),
        'word.isdigit': word.isdigit(),
        'postag=': postag
    }
    if i > 0:
        word1 = sent[i - 1][0]
        postag1 = sent[i - 1][1]
        features.update({
            '-1:word.lower': word1.lower(),
            '-1:word.istitle': word1.istitle(),
            '-1:word.isupper': word1.isupper(),
            '-1:word.isdigit': word1.isdigit(),
            '-1:postag': postag1
        })
    else:
        features['BOS'] = True

    if i < len(sent) - 1:
        word1 = sent[i + 1][0]
        postag1 = sent[i + 1][1]
        features.update({
            '+1:word.lower': word1.lower(),
            '+1:word.istitle': word1.istitle(),
            '+1:word.isupper': word1.isupper(),
            '+1:word.isdigit': word1.isdigit(),
            '+1:postag': postag1
        })
    else:
        features['EOS'] = True

    return features


def is_title(tag):
    return tag == 'BOS'


def word2features2predict(sent):
    word = sent['word']
    postag = sent['tag']
    features = {
        'bias': 1.0,
        'word.lower': word.lower(),
        'word.isupper': word.isupper(),
        'word.istitle': is_title(sent['placeInSentence']),
        'word.isdigit': word.isdigit(),
        'postag=': postag
    }
    if len(sent['wordBefore']) > 0:
        word1 = sent['wordBefore']['word']
        postag1 = sent['wordBefore']['tag']

        features.update({
            '-1:word.lower': word1.lower(),
            '-1:word.istitle': is_title(sent['wordBefore']['placeInSentence']),
            '-1:word.isupper': word1.isupper(),
            '-1:word.isdigit': word1.isdigit(),
            '-1:postag': postag1
        })
    else:
        features['BOS'] = True

    if len(sent['wordAfter']) > 0:
        word1 = sent['wordAfter']['word']
        postag1 = sent['wordAfter']['tag']
        features.update({
            '+1:word.lower': word1.lower(),
            '+1:word.istitle': is_title(sent['wordAfter']['placeInSentence']),
            '+1:word.isupper': word1.isupper(),
            '+1:word.isdigit': word1.isdigit(),
            '+1:postag': postag1
        })
    else:
        features['EOS'] = True

    return features


def sent2features(sent):
    return [word2features(sent, i) for i in range(len(sent))]


def sent2labels(sent):
    return [label for token, postag, label in sent]


def sent2tokens(sent):
    return [token for token, postag, label in sent]


def split_train_test(dataset, ratio=0.9):
    length_of_data = len(dataset)
    length_of_train = int(length_of_data * ratio)
    random.shuffle(dataset)

    train_set = dataset[:length_of_train]
    test_set = dataset[length_of_train:]

    return train_set, test_set
