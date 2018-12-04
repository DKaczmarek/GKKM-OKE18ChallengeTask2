from rdflib import Namespace

NIF = Namespace("http://persistence.uni-leipzig.org/nlp2rdf/ontologies/nif-core#")
ITSRDF = Namespace("http://www.w3.org/2005/11/its/rdf#")


class Sentence:

    def __init__(self, sentence):
        self.sentence = sentence
        self.associated_entities = []

    def __str__(self):
        return "[{0}]: {1}".format(self.sentence, self.associated_entities)

    def add_entity(self, entity):
        self.associated_entities.append(entity)


def graph_to_sentences(graph):
    sentences = []
    for reference in graph.subjects(object=NIF.Context):
        for sentence_object in graph.objects(subject=reference, predicate=NIF.isString):
            new_sentence = Sentence(sentence_object)
            sentences.append(new_sentence)

        for entity_subject in graph.subjects(predicate=NIF.referenceContext, object=reference):
            url, name, begin_index, end_index = None, None, None, None
            for phrase in graph.predicate_objects(subject=entity_subject):
                if 'taIdentRef' in phrase[0]:
                    url = str(phrase[1])
                if 'anchorOf' in phrase[0]:
                    name = str(phrase[1])
                if 'beginIndex' in phrase[0]:
                    begin_index = int(phrase[1])
                if 'endIndex' in phrase[0]:
                    end_index = int(phrase[1])

            sentences[-1].add_entity([name, url, begin_index, end_index])

    return [s for s in sentences if len(s.associated_entities) != 0]