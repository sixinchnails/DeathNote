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


def initializeSession(db):
    engine = create_engine(f"mysql+pymysql://{user}:{password}@{server_ip}/{db}")

    base = declarative_base()
    base.metadata.create_all(engine)

    session = sessionmaker(bind=engine)

    return session()


def getDbConnection():
    conn = pymysql.connect(
        host=server_ip,
        user=user,
        password=password,
        db=db
    )

    conn.autocommit = True

    return conn


def getSession(db):
    return initializeSession(db)


def get9oatSession():
    return initializeSession('9oat')


def getSpotifySession():
    return initializeSession('spotify')
