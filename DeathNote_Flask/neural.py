import joblib
import keras
from keras.models import load_model
from keras.models import Sequential
from keras.layers import Dense, Lambda
import pandas as pd, numpy as np
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
from keras.backend import sigmoid
from alchemy import getSpotifySongs, getComposeSongs

# Define Swish Function
def swish(x, beta=1):
    return x * sigmoid(beta * x)

# Assuming you have a dataset loaded into X (features) and y (popularity)
# X = ... # Your input features (acousticness, energy, etc.)
# y = ... # Your target output (popularity)
scaler = MinMaxScaler()
pca = PCA(n_components=0.95)

spotifySongs = getSpotifySongs()

music_data = [m.__dict__ for m in spotifySongs]

for music in music_data:
    music.pop('_sa_instance_state', None)

spotify_df = pd.DataFrame(music_data)

X = spotify_df.drop(['music_id', 'music_title', 'populatrity'], axis=1)
column_order = X.columns.tolist()

X = scaler.fit_transform(X)

X = pca.fit_transform(X)

joblib.dump(scaler, 'pop_scaler.pkl')
joblib.dump(pca, 'pop_pca.pkl')

y = spotify_df['populatrity']

# Define the model
model = Sequential()

model.add(Dense(64, activation='relu', input_shape=(X.shape[1],)))
model.add(Dense(64, activation='relu'))
model.add(Dense(64, activation='relu'))
model.add(Dense(64, activation='relu'))

model.add(Dense(1))  # Output layer: Predicting a single value

# Compile the model
model.compile(optimizer='adam', loss='mean_squared_error')

# Train the model
model.fit(X, y, epochs=1000, batch_size=32)

# Now you can use model.predict(new_data) to predict popularity of new songs

composeSongs = getComposeSongs()

compose_data = [m.__dict__ for m in composeSongs]

for compose in compose_data:
    compose.pop('_sa_instance_state', None)

compose_df = pd.DataFrame(compose_data)
input_compose_df = compose_df[column_order]
scaled_input_compose_df = scaler.transform(input_compose_df)
pca_input_compose_df = pca.transform(scaled_input_compose_df)

prediction = model.predict(pca_input_compose_df)

compose_df['predicted_popularity'] = prediction.flatten()

model.save('pop_model.keras')

saved_model = load_model('pop_model.keras')