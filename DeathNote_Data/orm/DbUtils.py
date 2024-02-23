import pymysql, configparser
from sqlalchemy import create_engine, Column, Integer, Float, String
from sqlalchemy.orm import declarative_base
from sqlalchemy.orm import sessionmaker

config = configparser.ConfigParser()
config.read('config.ini')
user = config['DEFAULT']['user']
password = config['DEFAULT']['password']
server_ip = config['DEFAULT']['serverIP']
db = config['DEFAULT']['db']

conn = pymysql.connect(
    host=server_ip,
    user=user,
    password=password,
    db=db
)

spotify_conn = pymysql.connect(
    host=server_ip,
    user=user,
    password=password,
    db=db
)

engine = create_engine(f"mysql+pymysql://{user}:{password}@{server_ip}/9oat")
spotify_engine = create_engine(f'mysql+pymysql://{user}:{password}@{server_ip}/spotify')

Base = declarative_base()
spotifyBase = declarative_base()

# Create all tables in the engine. This is equivalent to "Create Table" statements in raw SQL.
Base.metadata.create_all(engine)
spotifyBase.metadata.create_all(spotify_engine)

# Bind the engine to a session
Session = sessionmaker(bind=engine)
SpotifySession = sessionmaker(bind=spotify_engine)

# Create a session instance
session = Session()
spotifySession = SpotifySession()


def getDbConnection():
    return conn


def get9oatSession():
    return session


def getSpotifySession():
    return spotifySession
