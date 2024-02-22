import pandas as pd, joblib
from sklearn.decomposition import PCA
from sklearn.model_selection import train_test_split
from sklearn.multioutput import MultiOutputRegressor
from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_squared_error
import numpy as np
from sklearn.preprocessing import MinMaxScaler
from DeathNote_Data.orm.alchemy import spotifyMusic
from DeathNote_Data.orm.dbutils import getSpotifySession

session = getSpotifySession()

pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind':'{:.8f}'.format})

tracks = session.query(spotifyMusic).all()

data = []

for track in tracks:
    # Exclude SQLAlchemy internal attr.
    row_data = {key: value for key, value in track.__dict__.items() if not key.startswith('_')}
    data.append(row_data)

spotify_df = pd.DataFrame(data)

# Get all columns as a list
all_columns = spotify_df.columns.tolist()

feat_exclude_columns = ['music_id', 'music_title', 'populatrity']
target_columns_list = ['acousticness', 'danceability', 'energy', 'instrumentalness',
                       'liveness', 'loudness', 'speechiness', 'valence', 'tempo']

# Input feature column
feature_columns = [col for col in all_columns if col not in target_columns_list + feat_exclude_columns]
# Target column
target_columns = [col for col in target_columns_list if col in all_columns]

scaler = MinMaxScaler()
pca = PCA(n_components=0.95)

# Make input and output data for model
X = pca.fit_transform(scaler.fit_transform(spotify_df[feature_columns]))
y = spotify_df[target_columns]

# Save scaler and pca model as pkl for further use
joblib.dump(scaler, 'multiscaler.pkl')
joblib.dump(pca, 'multipca.pkl')

# Split the data into training and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

# Create model
model = MultiOutputRegressor(GradientBoostingRegressor(random_state=42))

# Train the model
model.fit(X_train, y_train)

# Save model
joblib.dump(model, 'multimodel.pkl')

# Predict on the test data
y_pred = model.predict(X_test)

# Calculate MSE
mse = mean_squared_error(y_test, y_pred, multioutput='raw_values')
print(f"Mean Squared Error for each output: {mse}")

# MSE across all outputs(AVG)
mean_mse = mse.mean()
print(f"Mean Squared Error (average across outputs): {mean_mse}")

