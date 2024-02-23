import os, joblib, configparser
import mysql.connector
import spotipy, librosa as lb
import yt_dlp as youtube_dl
from spotipy.oauth2 import SpotifyClientCredentials

from DeathNote_Data.utils.Utils import get_features_mean

config = configparser.ConfigParser()
config.read('config.ini')
user = config['DEFAULT']['user']
password = config['DEFAULT']['password']
server_ip = config['DEFAULT']['serverIP']
client_id = config['DEFAULT']['clientID']
client_secret = config['DEFAULT']['clientSecret']

# Initialize Spotify API client
client_id = client_id
client_secret = client_secret
sp = spotipy.Spotify(client_credentials_manager=SpotifyClientCredentials(client_id=client_id, client_secret=client_secret))

scaler = joblib.load('../../data/test/scaler.pkl')
pca = joblib.load('../../data/test/pca.pkl')
regressor = joblib.load('../../data/test/regressor_model.pkl')
path = '.\\songs\\'

# Define the list of music keywords
# Define the list of music keywords
keywords = [
    "Rap",
    "Rock",
    "Indie",
    "Pop",
    "Hip-Hop",
    "Jazz",
    "Classical",
    "Electronic",
    "R&B",
    "Country",
    "Blues",
    "Reggae",
    "Metal",
    "Folk",
    "Punk",
    "Dance",
    "Alternative",
    "Soul",
    "Gospel",
    "Funk",
    "Latin",
    "Salsa",
    "Reggaeton",
    "K-Pop",
    "J-Pop",
    "EDM",
    "Techno",
    "House",
    "Dubstep",
    "Ambient",
    "Trap",
    "Synthwave",
    "Grunge",
    "Ska",
    "Bluegrass",
    "Afrobeat",
    "Samba",
    "Bossa Nova",
    "Disco",
    "Hard Rock",
    "Progressive Rock",
    "Motown",
    "New Wave",
    "World Music",
    "Acoustic",
    "Orchestral",
    "Choir",
    "Celtic",
    "Flamenco",
    "Bollywood"
]

def get_playlist_tracks(playlist_id):
    results = sp.playlist_tracks(playlist_id)
    return results['items']

def get_spotify_audio_features(track_id):
    audio_features = sp.audio_features(track_id)
    return audio_features[0] if audio_features else None

def get_track_popularity(track_id):
    track_info = sp.track(track_id)
    popularity = track_info['popularity'] if track_info else None
    return popularity

def download_youtube_video(query, output_path):
    ydl_opts = {
        'format': 'bestaudio/best',
        'postprocessors': [{
            'key': 'FFmpegExtractAudio',
            'preferredcodec': 'wav',
            'preferredquality': '192',
        }],
        'outtmpl': output_path,
        'default_search': 'ytsearch',
        'quiet': False,
        'no_warnings': True,
        'user_agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134',
    }

    #outtmpl에 설치가 진행
    with youtube_dl.YoutubeDL(ydl_opts) as ydl:
        ydl.download([query])

# Create a "songs" directory if it doesn't exist
if not os.path.exists("songs"):
    os.makedirs("songs")

# Create a MySQL connection
conn = mysql.connector.connect(
    host=server_ip,
    user=user,
    password=password,
    database='spotify'  # Replace with the name of your database
)

conn.autocommit = True

cursor = conn.cursor()

# Define the column names as an array
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
    'rmseP_a', 'rmseP_std', 'rmseH_a', 'rmseH_std', 'centroid_a', 'centroid_std', 'bw_a', 'bw_std', 'contrast_a', 'contrast_std',
    'polyfeat_a', 'polyfeat_std', 'tonnetz_a', 'tonnetz_std', 'zcr_a', 'zcr_std', 'onset_a', 'onset_std', 'bpm', 'rmseP_skew',
    'rmseP_kurtosis', 'rmseH_skew', 'rmseH_kurtosis', 'beats_a', 'beats_std'
]

# Assign VARCHAR(255) to 'spotify_id' and 'title', FLOAT to the rest
columns = ', '.join([f"{name} VARCHAR(255)" if name in ['spotify_id', 'title'] else f"{name} FLOAT" for name in column_names])

drop_table_query = f'DROP TABLE IF EXISTS spotify_songs'
cursor.execute(drop_table_query)
create_table_query = f'CREATE TABLE IF NOT EXISTS spotify_songs ({columns})'
cursor.execute(create_table_query)

# Set the maximum number of songs to collect
max_songs = 10000
collected_songs = 0  # Initialize the counter

# Define the number of columns you are inserting data into
num_columns = 37

# Create a string with placeholders for the values
# This will create a string like: "%s, %s, %s, ..., %s" with 37 "%s" placeholders
placeholders = ", ".join(["%s"] * num_columns)

# Iterate through each music keyword
for keyword in keywords:
    if collected_songs >= max_songs:
        print(f"Collected {max_songs} songs. Stopping the collection process.")
        break

    print(f"Searching for playlists related to '{keyword}'...")
    playlists = sp.search(q=keyword, type='playlist')['playlists']['items']

    for playlist in playlists:
        if collected_songs >= max_songs:
            print(f"Collected {max_songs} songs. Stopping the collection process.")
            break

        playlist_id = playlist['id']
        playlist_name = playlist['name']
        print(f"Collecting songs from playlist '{playlist_name}'...")

        # Get tracks from the playlist
        tracks = get_playlist_tracks(playlist_id)

        for track in tracks:
            if collected_songs >= max_songs:
                print(f"Collected {max_songs} songs. Stopping the collection process.")
                break

            track_name = track['track']['name']
            artist = track['track']['artists'][0]['name']
            spotify_id = track['track']['id']

            # Check if the song already exists in the database to avoid duplicates
            check_query = "SELECT * FROM spotify_songs WHERE spotify_id = %s"
            cursor.execute(check_query, (spotify_id,))
            existing_song = cursor.fetchone()

            if existing_song:
                print(f"Song '{track_name}' by {artist} already exists in the database.")
            else:
                # Get Spotify audio features for the track
                audio_features = get_spotify_audio_features(spotify_id)
                popularity = get_track_popularity(spotify_id)  # Retrieve popularity
                print(f"Song '{track_name}' by {artist} is not in the database.")

                if audio_features:
                    print("Audio features: ", audio_features)
                    # Adjust the query as needed
                    query = f"{track_name} {artist} official audio"
                    output_path = f"songs/{spotify_id}"

                    try:
                        # Download the YouTube audio content
                        download_youtube_video(query, output_path)

                        #다운 받은 파일을 읽어와서 파형 특성 값 분석해서 insert query에 같이 삽입
                        data = {}
                        name_list = []
                        file = 'songs/' + spotify_id + ".wav"

                        name = file.split('\\')[-1].split('.')[0]

                        y, sr = lb.load(file, sr=44100)
                        res = get_features_mean(y=y, sr=sr, hop_length=512, n_fft=2048)
                        print(res)
                        # Insert the data into the table
                        #insert_query = "INSERT INTO songs VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)"
                        # Create the INSERT INTO statement
                        insert_query = f"INSERT INTO spotify_songs VALUES ({placeholders})"
                        print(insert_query)
                        data = (spotify_id, track_name, audio_features['acousticness'],
                                audio_features['danceability'], audio_features['energy'],
                                audio_features['instrumentalness'], audio_features['liveness'],
                                audio_features['loudness'], audio_features['speechiness'],
                                audio_features['valence'], audio_features['tempo'],
                                popularity,
                                float(res['rmseP_a']), float(res['rmseP_std']), float(res['rmseH_a']), float(res['rmseH_std']), float(res['centroid_a']),
                                float(res['centroid_std']), float(res['bw_a']), float(res['bw_std']), float(res['contrast_a']), float(res['contrast_std']),
                                float(res['polyfeat_a']), float(res['polyfeat_std']), float(res['tonnetz_a']), float(res['tonnetz_std']), float(res['zcr_a']),
                                float(res['zcr_std']), float(res['onset_a']), float(res['onset_std']), float(res['bpm']), float(res['rmseP_skew']), float(res['rmseP_kurtosis']),
                                float(res['rmseH_skew']), float(res['rmseH_kurtosis']), float(res['beats_a']), float(res['beats_std'])
                        )

                        cursor.execute(insert_query, data)

                        collected_songs += 1
                    except Exception as e:
                        print(e)
                else:
                    print(f"Audio features not available for {track_name} by {artist}")

        conn.commit()

# Commit the changes and close the database connection
conn.commit()
conn.close()
