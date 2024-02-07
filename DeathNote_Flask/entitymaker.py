import joblib
import glob
import librosa as lb
import pandas as pd
import numpy as np
from alchemy import composeMusic
from keras.models import load_model
from dbutils import getDbConnection, get9oatSession
from freqstats import get_features_mean
from utils import generate_song_title

"""
요약: 미확인 노래 특성 값 전부 추출해서 DB에 저장/디렉토리에서 알아서 조회
1. 분석 하지 않은 노래가 있는지 검색 및 전처리
2. 모델 불러오기
3. 음악 파형 특성 값 추출
4. 모델 실행(Scaler, PCA, Regressor)
5. ORM을 활용한 Music 엔티티 삽입
"""

# Set to display all columns in the dataframe
pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind':'{:.8f}'.format})

scaler = joblib.load('scaler.pkl')
pca = joblib.load('pca.pkl')
regressor = joblib.load('regressor_model.pkl')

pop_scaler = joblib.load('pop_scaler.pkl')
pop_pca = joblib.load('pop_pca.pkl')

"""
아직 분석하지 않은 노래 검색 및 전처리
"""
conn = getDbConnection()
cursor = conn.cursor()

data = {}
name_list =[]
path = 'aiva/'

#song directory files
directory_files = [f for f in glob.glob(path + "*.wav", recursive=True)]
#song table files
cursor.execute("SELECT music_title FROM music")
table_files = [x[0] for x in cursor.fetchall()]
#Will process files
files = []

for file in directory_files:
    if file in table_files: continue
    
    files.append(file)

for file in files:
    name = file.split('\\')[-1].split('.')[0]

    y, sr = lb.load(file, sr=44100)
    data[name] = {'y' : y, 'sr': sr}

"""
모델 불러오기
"""

scaler = joblib.load('multiscaler.pkl')
pca = joblib.load('multipca.pkl')
regressor = joblib.load('multimodel.pkl')

"""
음악 파형 특성 값 추출
"""

df = pd.DataFrame()

for key in data.keys():
    res = get_features_mean(y=data[key]['y'], sr=data[key]['sr'], hop_length=512, n_fft=2048)
    #res['file_name'] = generate_song_title('0')
    res['file_name'] = key.split("/")[1]

    res_df = pd.DataFrame([res])

    df = pd.concat([df, res_df], ignore_index=True)

"""
모델 실행(Scaler, PCA, Regressor)
"""

cols = df.columns.tolist()

cols.remove("file_name")
#cols = ["file_name"] + cols
df_file_name = df["file_name"]
df = df[cols]

scaled_df = scaler.transform(df)
pca_df = pca.transform(scaled_df)

prediction = regressor.predict(pca_df)

prediction_df = pd.DataFrame(prediction, columns = [
    'acousticness','danceability','energy',
    'instrumentalness','liveness','loudness',
    'speechiness','valence','tempo'
])

df_final = pd.concat([df, prediction_df], axis = 1)
df_final = df_final[['contrast_std', 'onset_a', 'rmseh_skew', 'instrumentalness', 'rmsep_a', 'polyfeat_a', 'onset_std', 'rmseh_kurtosis', 'liveness', 'rmsep_std', 'polyfeat_std', 'bpm', 'loudness', 'rmseh_a', 'centroid_a', 'tonnetz_a', 'beats_a', 'speechiness', 'rmseh_std', 'centroid_std', 'tonnetz_std', 'beats_std', 'valence', 'bw_a', 'zcr_a', 'rmsep_skew', 'acousticness', 'tempo', 'bw_std', 'zcr_std', 'rmsep_kurtosis', 'danceability', 'contrast_a', 'energy']]

"""
Assuming 'df_final' is your final DataFrame after merging
Add a new column 'popularity' with random values between 0 and 1
This is where popularity will be computed and stored.
"""

#df_final['popularity'] = np.random.rand(df_final.shape[0])

pop_scaler = joblib.load('pop_scaler.pkl')
pop_pca = joblib.load('pop_pca.pkl')
pop_model = load_model('pop_model.keras')

scaled_df_final = pop_scaler.transform(df_final)
pca_df_final = pop_pca.transform(scaled_df_final)

prediction_df = pop_model.predict(pca_df_final)
df_final['popularity'] = prediction_df

df_mysql = pd.concat([df_final, df_file_name], axis = 1)

col_list = df_mysql.columns.tolist()

"""
ORM을 활용한 Music 엔티티 CRUD 연산
"""

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
        acousticness=row['acousticness'],  # Corrected typo here
        danceability=row['danceability'],
        energy=row['energy'],
        instrumentalness=row['instrumentalness'],  # Corrected typo here
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
    print(e)
    session.rollback()
finally:
    session.close()
