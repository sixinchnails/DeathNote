import logging
import joblib
import glob
import librosa as lb
import pandas as pd
import numpy as np
from DeathNote_Data.orm.Alchemy import composeMusic
from keras.models import load_model
from DbUtils import getDbConnection, get9oatSession
from DeathNote_Data.utils.FreqStats import get_features_mean

pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind': '{:.8f}'.format})

"""
Summary: Extract features from unseen songs and store in DB
1. Find unseen songs and extract features
2. Load model and extract waveform features from song
3. Run model pipeline(Scaler/PCA/Regressor)
4. Insert Music Entity into MySQL using ORM
"""

scaler = joblib.load('scaler.pkl')
pca = joblib.load('pca.pkl')
regressor = joblib.load('regressor_model.pkl')

pop_scaler = joblib.load('pop_scaler.pkl')
pop_pca = joblib.load('pop_pca.pkl')

#Find unseen songs
conn = getDbConnection()
cursor = conn.cursor()

data = {}
name_list = []
path = 'aiva/'

# Song directory files
directory_files = [f for f in glob.glob(path + "*.wav", recursive=True)]
# Song table files
cursor.execute("SELECT music_title FROM music")
table_files = [x[0] for x in cursor.fetchall()]
files = []

for file in directory_files:
    if file in table_files: continue

    files.append(file)

for file in files:
    name = file.split('\\')[-1].split('.')[0]

    y, sr = lb.load(file, sr=44100)
    data[name] = {'y': y, 'sr': sr}

# Load multioutput model
scaler = joblib.load('multiscaler.pkl')
pca = joblib.load('multipca.pkl')
regressor = joblib.load('multimodel.pkl')

# Extract Waveform features
df = pd.DataFrame()

for key in data.keys():
    res = get_features_mean(y=data[key]['y'], sr=data[key]['sr'], hop_length=512, n_fft=2048)

    res['file_name'] = key.split("/")[1]

    res_df = pd.DataFrame([res])

    df = pd.concat([df, res_df], ignore_index=True)

# Run Scaler, Pca, Regression Pipeline
cols = df.columns.tolist()

cols.remove("file_name")
df_file_name = df["file_name"]
df = df[cols]

scaled_df = scaler.transform(df)
pca_df = pca.transform(scaled_df)

prediction = regressor.predict(pca_df)

prediction_df = pd.DataFrame(prediction, columns=[
    'acousticness', 'danceability', 'energy',
    'instrumentalness', 'liveness', 'loudness',
    'speechiness', 'valence', 'tempo'
])

df_final = pd.concat([df, prediction_df], axis=1)
df_final = df_final[['contrast_std', 'onset_a', 'rmseh_skew', 'instrumentalness', 'rmsep_a', 'polyfeat_a', 'onset_std',
                     'rmseh_kurtosis', 'liveness', 'rmsep_std', 'polyfeat_std', 'bpm', 'loudness', 'rmseh_a',
                     'centroid_a', 'tonnetz_a', 'beats_a', 'speechiness', 'rmseh_std', 'centroid_std', 'tonnetz_std',
                     'beats_std', 'valence', 'bw_a', 'zcr_a', 'rmsep_skew', 'acousticness', 'tempo', 'bw_std',
                     'zcr_std', 'rmsep_kurtosis', 'danceability', 'contrast_a', 'energy']]

# df_final['popularity'] = np.random.rand(df_final.shape[0])

pop_scaler = joblib.load('pop_scaler.pkl')
pop_pca = joblib.load('pop_pca.pkl')
pop_model = load_model('pop_model.keras')

prediction_df = pop_model.predict(
    pop_pca.transform(
        pop_scaler.transform(df_final)
    )
)

df_final['popularity'] = prediction_df

df_mysql = pd.concat([df_final, df_file_name], axis=1)

col_list = df_mysql.columns.tolist()

# Music Entity CRUD operation using SQLAlchemy
session = get9oatSession()

# Loop through each row of the DataFrame and create an instance of the Music class
for index, row in df_mysql.iterrows():
    music = composeMusic(
        music_title=row['file_name'],
        rmsep_a=row['rmsep_a'],
        rmsep_std=row['rmsep_std'],
        rmseh_a=row['rmseh_a'],
        rmseh_std=row['rmseh_std'],
        centroid_a=row['centroid_a'],
        centroid_std=row['centroid_std'],
        bw_a=row['bw_a'],
        bw_std=row['bw_std'],
        contrast_a=row['contrast_a'],
        contrast_std=row['contrast_std'],
        polyfeat_a=row['polyfeat_a'],
        polyfeat_std=row['polyfeat_std'],
        tonnetz_a=row['tonnetz_a'],
        tonnetz_std=row['tonnetz_std'],
        zcr_a=row['zcr_a'],
        zcr_std=row['zcr_std'],
        onset_a=row['onset_a'],
        onset_std=row['onset_std'],
        bpm=row['bpm'],
        rmsep_skew=row['rmsep_skew'],
        rmsep_kurtosis=row['rmsep_kurtosis'],
        rmseh_skew=row['rmseh_skew'],
        rmseh_kurtosis=row['rmseh_kurtosis'],
        beats_a=row['beats_a'],
        beats_std=row['beats_std'],
        acousticness=row['acousticness'],
        danceability=row['danceability'],
        energy=row['energy'],
        instrumentalness=row['instrumentalness'],
        liveness=row['liveness'],
        loudness=row['loudness'],
        speechiness=row['speechiness'],
        valence=row['valence'],
        tempo=row['tempo'],
        populatrity=row['popularity']
    )

    # Add the new instance to the session
    session.add(music)

# Commit the session to insert the records into the database
try:
    session.commit()
except Exception as e:
    logging.exception(e)
    session.rollback()
finally:
    session.close()
