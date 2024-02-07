from sqlalchemy import create_engine, Column, Integer, Float, String
from sqlalchemy.orm import declarative_base
from sqlalchemy.orm import sessionmaker
import configparser

Base = declarative_base()
spotifyBase = declarative_base()

config = configparser.ConfigParser()
config.read('config.ini')
user = config['DEFAULT']['user']
password = config['DEFAULT']['password']
server_ip = config['DEFAULT']['serverIP']

# Define the MySQL connection string
# mysql://<user>:<password>@<host>/<dbname>
engine = create_engine(f"mysql+pymysql://{user}:{password}@{server_ip}/9oat")
spotifyEngine = create_engine(f'mysql+pymysql://{user}:{password}@{server_ip}/spotify')

class composeMusic(Base):
    __tablename__ = 'music'
    music_id = Column(Integer, primary_key=True, autoincrement=True)
    music_title = Column(String(200), nullable=False)
    rmsep_a = Column(Float, nullable=False)
    rmsep_std = Column(Float, nullable=False)
    rmseh_a = Column(Float, nullable=False)
    rmseh_std = Column(Float, nullable=False)
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
    rmsep_skew = Column(Float, nullable=False)
    rmsep_kurtosis = Column(Float, nullable=False)
    rmseh_skew = Column(Float, nullable=False)
    rmseh_kurtosis = Column(Float, nullable=False)
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
    populatrity = Column(Float, nullable=False)

class spotifyMusic(Base):
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

# After this, you would still create an engine, bind it to the sessionmaker,
# and create a session as shown in the previous example.

# Create all tables in the engine. This is equivalent to "Create Table" statements in raw SQL.
Base.metadata.create_all(engine)
spotifyBase.metadata.create_all(spotifyEngine)

# Bind the engine to a session
Session = sessionmaker(bind=engine)
SpotifySession = sessionmaker(bind=spotifyEngine)

# Create a session instance
session = Session()
spotifySession = SpotifySession()

#all_music = session.query(composeMusic).all()

def getComposeSongs():
    return session.query(composeMusic).all()

def getSpotifySongs():
    #spotify_music = spotifySession.query(spotifyMusic).all()
    return spotifySession.query(spotifyMusic).all()

# Create an instance of the Music class
# Create an instance of the Music class with sample data

"""
new_music = Music(
    music_title="Sample Song",
    rmsep_a=0.6,
    rmsep_std=0.1,
    rmseh_a=0.4,
    rmseh_std=0.05,
    centroid_a=1500.0,
    centroid_std=250.0,
    bw_a=2000.0,
    bw_std=300.0,
    contrast_a=0.3,
    contrast_std=0.06,
    polyfeat_a=1.0,
    polyfeat_std=0.2,
    tonnetz_a=0.5,
    tonnetz_std=0.1,
    zcr_a=0.2,
    zcr_std=0.04,
    onset_a=0.3,
    onset_std=0.05,
    bpm=120,
    rmsep_skew=0.5,
    rmsep_kurtosis=3.0,
    rmseh_skew=0.4,
    rmseh_kurtosis=3.2,
    beats_a=0.7,
    beats_std=0.15,
    acousticness=0.2,
    danceability=0.8,
    energy=0.9,
    instrumentalness=0.0,
    liveness=0.1,
    loudness=-5.0,
    speechiness=0.05,
    valence=0.7,
    tempo=100.0,
    populatrity=0.85
)
"""

# Add the record to the session
#session.add(new_music)

# Commit the session to write the data to the database
#session.commit()
