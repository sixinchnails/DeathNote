from DeathNote_Data.orm.DbUtils import getDbConnection
from sklearn.metrics.pairwise import cosine_similarity
import numpy as np
import random

conn = getDbConnection()
cursor = conn.cursor()


def select_element(popularities):
    inverted_pop = [1 / pop for pop in popularities]

    total = sum(inverted_pop)
    normalized_probs = [inv_pop / total for inv_pop in inverted_pop]

    return random.choices([x for x in range(len(popularities))], weights=normalized_probs, k=1)[0]


def sim_calc(my_song):
    cursor.execute(
        "SELECT music_title, acousticness, danceability, energy, liveness, loudness, speechiness, valence, tempo, populatrity FROM music")
    songs = list(cursor.fetchall())

    for i in range(len(songs)):
        songs[i] = list(songs[i])

    cursor.execute("SELECT MIN(tempo), MAX(tempo) FROM music")
    tempo_info = cursor.fetchall()
    min_tempo, max_tempo = min(my_song[7], tempo_info[0][0]), max(my_song[7], tempo_info[0][1])

    my_song[4] = (my_song[4] + 60) / 120
    my_song[7] = (my_song[7] - min_tempo) / (max_tempo - min_tempo)

    my_song = np.array(my_song).reshape(1, -1)

    sim_res = []

    # #6 => loudness 9 => tempo
    for i in range(len(songs)):
        songs[i][5] = (songs[i][8] + 60) / 120
        songs[i][5] = (songs[i][8] - min_tempo) / (max_tempo - min_tempo)

        # Reshape the song features into a 2D array
        song_features = np.array(songs[i][1:-1]).reshape(1, -1)

        # Calculate the cosine similarity and append it to the results
        # [0][0] to get the scalar value
        similarity = cosine_similarity(my_song, song_features)[0][0]
        sim_res.append([songs[i][0], similarity, songs[i][-1]])

    sim_res.sort(key=lambda x: x[1], reverse=True)

    popularities = [sim_res[i][-1] for i in range(min(9, len(sim_res)))]

    idx = select_element(popularities)

    return sim_res[idx][0]
