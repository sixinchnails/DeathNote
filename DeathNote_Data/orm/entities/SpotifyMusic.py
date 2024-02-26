from sqlalchemy import Column, Integer, Float, String
from sqlalchemy.orm import declarative_base
from sqlalchemy import insert
from DeathNote_Data.orm.DbUtils import *

Base = declarative_base()
session = getSession('spotify')
engine = session.get_bind()

# Create a MySQL connection
conn = getDbConnection()
cursor = conn.cursor()

# Define column names as an array
column_names = [
    'spotify_id',
    'title',
    'acousticness',
    'danceability',
    'energy',
    'instrumentalness',
    'liveness',
    'loudness',
    'speechiness',
    'valence',
    'tempo',
    'popularity',
    # Sound feature values
    'rmseP_a', 'rmseP_std', 'rmseH_a', 'rmseH_std', 'centroid_a', 'centroid_std', 'bw_a', 'bw_std', 'contrast_a',
    'contrast_std',
    'polyfeat_a', 'polyfeat_std', 'tonnetz_a', 'tonnetz_std', 'zcr_a', 'zcr_std', 'onset_a', 'onset_std', 'bpm',
    'rmseP_skew',
    'rmseP_kurtosis', 'rmseH_skew', 'rmseH_kurtosis', 'beats_a', 'beats_std'
]

# Assign VARCHAR(255) to 'spotify_id' and 'title', FLOAT to the rest
columns = ', '.join(
    [f"{name} VARCHAR(255)" if name in ['spotify_id', 'title'] else f"{name} FLOAT" for name in column_names])

class SpotifyMusic(Base):
    __tablename__ = 'spotify_songs'
    music_id = Column('spotify_id', Integer, primary_key=True, autoincrement=True)
    music_title = Column('title', String(200), nullable=False)
    rmsep_a = Column('rmseP_a', Float, nullable=False)
    rmsep_std = Column('rmseP_std', Float, nullable=False)
    rmseh_a = Column('rmseH_a', Float, nullable=False)
    rmseh_std = Column('rmseH_std', Float, nullable=False)
    centroid_a = Column(Float, nullable=False)
    centroid_std = Column(Float, nullable=False)
    bw_a = Column(Float, nullable=False)
    bw_std = Column(Float, nullable=False)
    contrast_a = Column(Float, nullable=False)
    contrast_std = Column(Float, nullable=False)
    polyfeat_a = Column(Float, nullable=False)
    polyfeat_std = Column(Float, nullable=False)
    tonnetz_a = Column(Float, nullable=False)
    tonnetz_std = Column(Float, nullable=False)
    zcr_a = Column(Float, nullable=False)
    zcr_std = Column(Float, nullable=False)
    onset_a = Column(Float, nullable=False)
    onset_std = Column(Float, nullable=False)
    bpm = Column(Integer, nullable=False)
    rmsep_skew = Column('rmseP_skew', Float, nullable=False)
    rmsep_kurtosis = Column('rmseP_kurtosis', Float, nullable=False)
    rmseh_skew = Column('rmseH_skew', Float, nullable=False)
    rmseh_kurtosis = Column('rmseH_kurtosis', Float, nullable=False)
    beats_a = Column(Float, nullable=False)
    beats_std = Column(Float, nullable=False)
    acousticness = Column(Float, nullable=False)
    danceability = Column(Float, nullable=False)
    energy = Column(Float, nullable=False)
    instrumentalness = Column(Float, nullable=False)
    liveness = Column(Float, nullable=False)
    loudness = Column(Float, nullable=False)
    speechiness = Column(Float, nullable=False)
    valence = Column(Float, nullable=False)
    tempo = Column(Float, nullable=False)
    populatrity = Column('popularity', Float, nullable=False)


SpotifyMusic.__table__.create(bind=engine, checkfirst=True)

def getSpotifySongs():
    return [m.__dict__ for m in session.query(SpotifyMusic).all()]


def dropSpotifySongs():
    query = f'DROP TABLE IF EXISTS spotify_songs'

    cursor.execute(query)


def createSpotifySongs():
    create_table_query = f'CREATE TABLE IF NOT EXISTS spotify_songs ({columns})'
    cursor.execute(create_table_query)


def findSpotifySong(spotify_id):
    query = f"SELECT * FROM spotify_songs WHERE spotify_id = %s"
    cursor.execute(query, (spotify_id,))

    return cursor.fetchone()


def insertSpotifySong(row):
    session.execute(insert(SpotifyMusic), row)
