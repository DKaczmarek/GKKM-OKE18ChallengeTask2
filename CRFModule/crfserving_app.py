import flask
import logging
import os
import json
import pycrfsuite

from crftools.databuilder import word2features2predict

# set up flask
main = flask.Blueprint('main', __name__)

# set up logger
logger = logging.getLogger(__name__)
handler = logging.StreamHandler()
formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')

logger.setLevel(logging.DEBUG)
handler.setFormatter(formatter)
logger.addHandler(handler)

# model related
MODEL_PATH = './tmp/oke-challenge-task.crfsuite'


@main.route("/crf/reload", methods=["POST"])
def reload_mode():
    logger.info('model reloading...')
    status = reload_crf_model(MODEL_PATH)
    json_response = {"status": status}
    return json.dumps(str(json_response)), 200


@main.route("/crf/predict", methods=['GET', 'POST'])
def get_tags():
    json_body = flask.request.get_json()
    print(json_body)
    # json_dict = json.loads(json_body)
    # print(json_dict)

    json_response = prepare_samples(json_body)
    print(json_response)
    return json.dumps(str(json_response)), 200


def prepare_samples(json_body):
    features = [word2features2predict(json_body[sample_key]) for sample_key in json_body]
    results = crftagger.tag(features)
    for sample_key, label in zip(json_body, results):
        json_body[sample_key]['label'] = label
    return json_body


def reload_crf_model(model_path):
    if model_exists(model_path):
        crftagger.open(model_path)
        return True
    return False


def model_exists(path):
    return os.path.isfile(path)


def run_serving_app(model_path):
    global crftagger
    crftagger = pycrfsuite.Tagger()
    reload_crf_model(model_path)

    serving_app = flask.Flask(__name__)
    serving_app.register_blueprint(main)
    return serving_app


if __name__ == '__main__':
    if model_exists(MODEL_PATH):
        app = run_serving_app(MODEL_PATH)
        app.run(host="0.0.0.0", port=17011)
    else:
        logger.info('crf model not exist')

    logger.info('server shutdown')

