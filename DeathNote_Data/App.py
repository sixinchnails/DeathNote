from DeathNote_Data.orm.DbUtils import getDbConnection
from flask import Flask, request, jsonify, send_file
from flask_cors import CORS
import os
import joblib
from flask_expects_json import expects_json
import traceback
import logging
from DeathNote_Data.utils.SimCalc import sim_calc

app = Flask(__name__)
CORS(app)

mysql_conn = getDbConnection()
cursor = mysql_conn.cursor()

scaler = joblib.load("deploy_models/multioutput/multiscaler.pkl")
pca = joblib.load("deploy_models/multioutput/multipca.pkl")
regressor = joblib.load("deploy_models/multioutput/multimodel.pkl")

logging.basicConfig(level=logging.INFO, filename='nohup2.log',
                    filemode='a', format='%(asctime)s - %(levelname)s - %(message)s')

# JSON request must follow this format or throws request error
compose_schema = {
    "type": "object",
    "properties": {
        "energy": {"type": "number"},
        "acousticness": {"type": "number"},
        "danceability": {"type": "number"},
        "instrumentalness": {"type": "number"},
        "liveness": {"type": "number"},
        "loudness": {"type": "number"},
        "speechiness": {"type": "number"},
        "tempo": {"type": "number"},
        "valence": {"type": "number"}
    },
    "required": ["energy", "acousticness", "danceability", "instrumentalness", "liveness", "loudness", "speechiness",
                 "tempo", "valence"]
}


@app.route('/compose', methods=['POST'])
@expects_json(compose_schema)
def compose():
    req = request.get_json()

    valence = float(req['valence'])
    energy = float(req['energy'])
    acousticness = float(req['acousticness'])
    danceability = float(req['danceability'])
    instrumentalness = float(req['instrumentalness'])
    liveness = float(req['liveness'])
    loudness = float(req['loudness'])
    speechiness = float(req['speechiness'])
    tempo = float(req['tempo'])

    try:
        print("[Logging]")
        logging.info('This is an info message that will go to nohup.log.')

        similar_song = sim_calc([acousticness, danceability, energy, liveness, loudness, speechiness, valence, tempo])

        file_name = similar_song + '.wav'
        directory_path = 'aiva/'
        file_path = os.path.join(directory_path, file_name)

        logging.info(file_path)

        if not os.path.isfile(file_path):
            return jsonify({"error": "File not found."}), 404

        response = send_file(file_path, mimetype='audio/wav')
        response.headers['Title'] = similar_song

        return response
    except Exception as e:
        traceback.print_exc()

        logging.error(traceback.format_exc())

        return jsonify({"error": str(e)}), 500


@app.route('/', methods=['GET'])
def index():
    return ""


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)
