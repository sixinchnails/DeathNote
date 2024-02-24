from sqlalchemy import Column, Integer, Float, String
from sqlalchemy.orm import declarative_base
from DeathNote_Data.orm.DbUtils import getSession

Base = declarative_base()
session = getSession('spotify')


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


def getSpotifySongs():
    return [m.__dict__ for m in session.query(SpotifyMusic).all()]
