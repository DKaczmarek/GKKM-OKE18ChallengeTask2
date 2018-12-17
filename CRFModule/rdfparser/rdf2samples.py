import json

from rdflib.util import guess_format
from rdflib import Graph, Namespace

import CRFModule.rdfparser.sparqlutils as sparql
import CRFModule.rdfparser.sentences
import CRFModule.rdfparser.enrichment

# namespaces for rdf
NIF = Namespace("http://persistence.uni-leipzig.org/nlp2rdf/ontologies/nif-core#")


def parse_sentences_from_graph(rdf_graph, output_path):
    with open(output_path, 'a+') as file:
        for sentence in rdf_graph.objects(predicate=NIF.isString):
            new_sentence = sentence.replace('\"', '')
            file.write("{0}\n".format(new_sentence))


def load_graph_from_file(path):
    loaded_file_format = guess_format(path)
    loaded_graph = Graph()
    loaded_graph.parse(path, format=loaded_file_format)
    return loaded_graph


def print_triples(graph):
    print("graph has %s statements." % len(graph))
    for sub, pred, obj in graph:
        print("sub: {0} \npred: {1} \nobj: {2}\n\n".format(sub, pred, obj))


def run_process(graph_file_path, output_json_path):
    oke_graph = load_graph_from_file(graph_file_path)
    print("graph has %s statements." % len(oke_graph))

    g = CRFModule.rdfparser.sentences.graph_to_sentences(oke_graph)
    all_g = len(g)
    ontology_namespace = 'http://dbpedia.org/ontology/'

    updated_graph = []
    found_label = None
    for i in range(len(g)):
        updated_entities = {'sentence': str(g[i].sentence), 'words': []}
        for e in g[i].associated_entities:
            if 'notInWiki' in e[1]:
                updated_entities['words'].append({'value': e[0], 'uri': e[1],
                                                  'begin': e[2], 'end': e[3],
                                                  'label': 'UNE'})
            else:
                types = sparql.get_type_from_resource(e[1])['results']['bindings']
                print(types)
                if types is not None:
                    for value in types:
                        parsed_value = value['type']['value'].replace(ontology_namespace, '')
                        if parsed_value in sparql.list_of_superclass:
                            found_label = parsed_value
                            break
                        else:
                            found_label = 'UNE'
                else:
                    found_label = 'UNE'
                        
                updated_entities['words'].append({'value': e[0], 'uri': e[1],
                                                  'begin': e[2], 'end': e[3],
                                                  'label': found_label})

        print('{0} of {1} loaded sentences'.format(str(i), str(all_g)))
        updated_graph.append(updated_entities)

    json.dump(updated_graph, open(output_json_path, 'w'))

    CRFModule.rdfparser.enrichment.create_samples(output_json_path)


if __name__ == '__main__':
    JSON_GRAPH = './tmp/samples_graph.json'
    DATAPATH_TASKALL = './data/oke17taskAll.ttl'
    run_process(DATAPATH_TASKALL, JSON_GRAPH)
