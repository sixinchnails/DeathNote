import pandas as pd, joblib
from sklearn.decomposition import PCA
from sklearn.model_selection import train_test_split
from sklearn.multioutput import MultiOutputRegressor
from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_squared_error
import numpy as np
from sklearn.preprocessing import MinMaxScaler

from alchemy import spotifyMusic
from dbutils import getSpotifySession

session = getSpotifySession()

pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind':'{:.8f}'.format})

tracks = session.query(spotifyMusic).all()

data = []

for track in tracks:
    # Use the __dict__ attribute but exclude SQLAlchemy internal attributes
    row_data = {key: value for key, value in track.__dict__.items() if not key.startswith('_')}
    data.append(row_data)

spotify_df = pd.DataFrame(data)

# Merge the features and targets DataFrames on the corresponding ID columns
# In this case, 'file_name' from features_df and 'spotify_id' from targets_df

# After merging, you can drop the 'file_name' and 'spotify_id' columns if they are not needed for prediction
#
# Get all columns as a list
all_columns = spotify_df.columns.tolist()

# Specify the columns to exclude
columns_to_exclude = ['file_name', 'spotify_id', 'title', 'wav_path', 'time_signature', 'song_key', 'popularity']  # Add any other columns you want to exclude
exclude_columns = ['music_id', 'music_title', 'populatrity']

# Get feature columns by excluding the unwanted columns
# Assume that target columns are known and listed as 'target_columns_list'
target_columns_list = ['acousticness', 'danceability', 'energy', 'instrumentalness',
                       'liveness', 'loudness', 'speechiness', 'valence', 'tempo']

# Exclude the target columns and any other specific columns you don't want from the features
feature_columns = [col for col in all_columns if col not in target_columns_list + exclude_columns]
# Your target columns are already defined, but if you need to dynamically exclude columns, do similarly:
target_columns = [col for col in target_columns_list if col in all_columns]

# Now, select the features and targets from the merged dataframe
X = spotify_df[feature_columns]
y = spotify_df[target_columns]
scaler = MinMaxScaler()
pca = PCA(n_components=0.95)

X = scaler.fit_transform(X)
X = pca.fit_transform(X)

joblib.dump(scaler, 'multiscaler.pkl')
joblib.dump(pca, 'multipca.pkl')

# Split the data into training and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

# Create the multioutput regressor
model = MultiOutputRegressor(GradientBoostingRegressor(random_state=42))

# Train the model
model.fit(X_train, y_train)

joblib.dump(model, 'multimodel.pkl')

# Predict on the test data
y_pred = model.predict(X_test)

# Evaluate the model
mse = mean_squared_error(y_test, y_pred, multioutput='raw_values')
print(f"Mean Squared Error for each output: {mse}")

# If you want to calculate the average MSE across all outputs
mean_mse = mse.mean()
print(f"Mean Squared Error (average across outputs): {mean_mse}")

