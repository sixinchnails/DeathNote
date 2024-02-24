from sqlalchemy import Column, Integer, Float, String
from sqlalchemy.orm import declarative_base
from DeathNote_Data.orm.DbUtils import getSession

Base = declarative_base()
session = getSession('9oat')


class ComposeMusic(Base):
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


def getComposeSongs():
    return [m.__dict__ for m in session.query(ComposeMusic).all()]
