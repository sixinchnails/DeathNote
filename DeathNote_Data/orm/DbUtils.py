import pymysql, configparser
from sqlalchemy import create_engine
from sqlalchemy.orm import declarative_base
from sqlalchemy.orm import sessionmaker

config = configparser.ConfigParser()
config.read('config.ini')
user = config['DB']['user']
password = config['DB']['password']
server_ip = config['DB']['serverIP']
db = config['DB']['db']

engine = create_engine(f"mysql+pymysql://{user}:{password}@{server_ip}/9oat")
spotify_engine = create_engine(f'mysql+pymysql://{user}:{password}@{server_ip}/spotify')

Base = declarative_base()
spotifyBase = declarative_base()

# Create all tables in the engine
Base.metadata.create_all(engine)
spotifyBase.metadata.create_all(spotify_engine)

# Bind the engine to a session
Session = sessionmaker(bind=engine)
SpotifySession = sessionmaker(bind=spotify_engine)

# Create a session instance
session = Session()
spotifySession = SpotifySession()


def getDbConnection():
    conn = pymysql.connect(
        host=server_ip,
        user=user,
        password=password,
        db=db
    )

    return conn


def get9oatSession():
    return session


def getSpotifySession():
    return spotifySession
