import json
import nltk


SAMPLES = './tmp/samples_v2.txt'


def pos_tags_to_string(list_of_pos_tags):
    line = ''
    for pos in list_of_pos_tags:
        line += pos[0] + ' '

    return line[-1:]


# aim to create triples (word, pos-tag, class)
def create_samples(json_graph_path, output=SAMPLES):
    json_body = json.load(open(json_graph_path, 'rb'))

    with open(output, 'w+') as samples_file:

        sentence_list = []
        for sentence in json_body:
            # print(len(sentence['words']), sentence['words'])

            tokens = nltk.word_tokenize(sentence['sentence'])
            pos_tags = nltk.pos_tag(tokens)

            prepared_tokens = []
            all_words = sorted(sentence['words'], key=lambda k: k['begin'])

            for word in all_words:
                # preprocess_word = word['value'].replace(',', '')
                # preprocess_word = preprocess_word.replace('.', '')
                preprocess_word = word['value']

                tokenized_entity = nltk.word_tokenize(preprocess_word)
                pretag_entity = nltk.pos_tag(tokenized_entity)

                for i, token in enumerate(pretag_entity):
                    value = token[0]
                    postag = token[1]
                    if i == 0:
                        pretag = 'I'
                    else:
                        pretag = 'O'
                    label = word['label']

                    if i == 0:
                        prepared_tokens.append((value, postag, pretag + '-' + label, 'beg'))
                    elif i == len(pretag_entity) - 1:
                        prepared_tokens.append((value, postag, pretag + '-' + label, 'end'))
                    else:
                        prepared_tokens.append((value, postag, pretag + '-' + label, 'cont'))

            length_of_prepared_tokens = len(prepared_tokens)
            prepared_tokens_pointer = 0
            for word in pos_tags:
                if prepared_tokens_pointer == length_of_prepared_tokens:
                    sentence_list.append((word[0], word[1], 'O'))
                    # samples_file.write(str((word[0], word[1], 'O')) + '\n')
                elif word[0] == prepared_tokens[prepared_tokens_pointer][0]:
                    label = prepared_tokens[prepared_tokens_pointer][2]
                    sentence_list.append((word[0], word[1], label))
                    # samples_file.write(str((word[0], word[1], label)) + '\n')
                    prepared_tokens_pointer += 1
                else:
                    sentence_list.append((word[0], word[1], 'O'))
                    # samples_file.write(str((word[0], word[1], 'O')) + '\n')

            samples_file.write(str(sentence_list) + '\n')
            sentence_list = []
