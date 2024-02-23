import joblib
from keras.models import load_model
from keras.models import Sequential
from keras.layers import Dense
import pandas as pd
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
from keras.backend import sigmoid
from DeathNote_Data.orm.Alchemy import getSpotifySongs, getComposeSongs


# Swish Function
def swish(x, beta=1):
    return x * sigmoid(beta * x)


scaler = MinMaxScaler()
pca = PCA(n_components=0.95)

# Fetch spotify data from MySQL
music_data = getSpotifySongs()

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

# Define 'Sequential' model for 'popularity' prediction
model = Sequential()

model.add(Dense(64, activation='relu', input_shape=(X.shape[1],)))

for i in range(3):
    model.add(Dense(64, activation='relu'))

model.add(Dense(1))  # Output layer: Predicting a single value

# Compile the model
model.compile(optimizer='adam', loss='mean_squared_error')

# Train the model
model.fit(X, y, epochs=1000, batch_size=32)

# Get songs composed in AIVA
compose_data = getComposeSongs()

for compose in compose_data:
    compose.pop('_sa_instance_state', None)

raw_compose_df = pd.DataFrame(compose_data)[column_order]
scaled_compose_df = scaler.transform(raw_compose_df)
input_compose_df = pca.transform(scaled_compose_df)

prediction = model.predict(input_compose_df)

model.save('pop_model.keras')

saved_model = load_model('pop_model.keras')
