from dbutils import getDbConnection
from flask import Flask, request, jsonify, send_file
from flask_cors import CORS
import os
import joblib
from flask_expects_json import expects_json
import traceback
import logging
from simcalc import sim_calc

app = Flask(__name__)
CORS(app)

mysql_conn = getDbConnection()
cursor = mysql_conn.cursor()

scaler = joblib.load("scaler.pkl")
pca = joblib.load("pca.pkl")
regressor = joblib.load("regressor_model.pkl")

logging.basicConfig(level=logging.INFO, filename='nohup2.log',
        filemode='a', format='%(asctime)s - %(levelname)s - %(message)s')

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
    "required": ["energy", "acousticness", "danceability", "instrumentalness", "liveness", "loudness", "speechiness", "tempo", "valence"]
}

@app.route('/compose', methods=['POST'])
@expects_json(compose_schema)
def compose():
    print("Compose API")
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
        # Depending on the use case, you may want to exclude the exact match, which will have a cosine similarity of 1.
        #similar_rows = [row for row in similar_rows if row['cosine_similarity'] < 0.9999]
        print("[Logging]")
        logging.info('This is an info message that will go to nohup2.log.')

        similar_song = sim_calc([acousticness, danceability, energy, liveness, loudness, speechiness, valence, tempo])
        print(similar_song)
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
        print(f"Error: {e}")

        tb = traceback.format_exc()
        print(tb)

        return jsonify({"error": str(e)}), 500

@app.route('/', methods=['GET'])
def index():
    return ""

@app.route('/hello', methods=['GET'])
def hello():
    return "hello world!"

@app.route('/ssafy', methods=['GET'])
def ssafy():
    return "ssafy"

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)