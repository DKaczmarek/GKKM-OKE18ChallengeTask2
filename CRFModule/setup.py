import logging
import time

import CRFModule.rdfparser.rdf2samples
import CRFModule.crftools.trainer

# set up logger
logger = logging.getLogger(__name__)
handler = logging.StreamHandler()
formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')

logger.setLevel(logging.DEBUG)
handler.setFormatter(formatter)
logger.addHandler(handler)

# file output paths
JSON_GRAPH = './tmp/samples_graph.json'
SAMPLES_PATH = './tmp/samples_v2.txt'
MODEL_PATH = './tmp/oke-challenge-task.crfsuite'

# useful paths
DATAPATH_TASK1 = './data/oke17task1Training.xml.ttl'
DATAPATH_TASK2 = './data/oke17task2Training.xml.ttl'
DATAPATH_TASKALL = './data/oke17taskAll.ttl'

if __name__ == '__main__':

    start_time = time.time()
    logger.info('converting rdf graph to samples...')
    CRFModule.rdfparser.rdf2samples.run_process(DATAPATH_TASKALL, JSON_GRAPH)
    logger.info('conversion complete, it takes {0} s'.format(time.time() - start_time))

    start_time = time.time()
    logger.info('training crf model...')
    CRFModule.crftools.trainer.run_train_process(SAMPLES_PATH, MODEL_PATH)
    logger.info('training complete, it takes {0} s'.format(time.time() - start_time))
