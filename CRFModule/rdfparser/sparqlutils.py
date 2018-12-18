from SPARQLWrapper import SPARQLWrapper, JSON

SPARQL = SPARQLWrapper("http://dbpedia.org/sparql")
SPARQL.setReturnFormat(JSON)

list_of_superclass = ['Activity',
                      'Organisation',
                      'Person',
                      'Agent',
                      'Award',
                      'Disease',
                      'EthnicGroup',
                      'Event',
                      'Language',
                      'MeanOfTransportation',
                      'PersonFunction',
                      'Place',
                      'ArchitecturalStructure',
                      'Species',
                      'Work'
                      ]


def get_type_from_resource(resource_name):
    try:
        formated_name = replace_whitespaces(resource_name)
        type_query = return_type_sparql_query(formated_name)
        SPARQL.setQuery(type_query)
        SPARQL.setReturnFormat(JSON)
        results = SPARQL.query().convert()
        return results
    except TimeoutError:
        return None
    except:
        return None


def replace_whitespaces(name):
    return name.replace(" ", "_")


def return_type_sparql_query(name):
    return ("prefix itsrdf: <http://www.w3.org/2005/11/its/rdf#> "
            "select distinct ?type where { "
            "<{0}> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> ?type . "
            "filter (strstarts(str(?type), \"http://dbpedia.org/ontology/\")) }").replace("{0}", name)


def get_subclasses(super_class):
    subclasses_query = return_subclasses_sparql_query(super_class)
    SPARQL.setQuery(subclasses_query)
    results = SPARQL.query().convert()
    return results


def return_subclasses_sparql_query(super_class):
    return ("prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> "
            "select distinct ?subClass where { "
            "?subClass rdfs:subClassOf <http://dbpedia.org/ontology/{0}> . }").replace("{0}", super_class)