import pandas as pd
from sklearn.decomposition import PCA
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error
from sklearn.ensemble import RandomForestRegressor
from math import sqrt
import joblib
import numpy as np

"""
Scaler, PCA, Regressor pkl generator
"""

# Set to display all columns in the dataframe
pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind':'{:.8f}'.format})

# Load your data
features_df = pd.read_csv('../data/feat_output.csv')
targets_df = pd.read_csv('../data/spotify_output.csv')

# Ensure that only the rows with matching 'file_name' and 'spotify_id' are used
df = features_df.merge(targets_df, left_on='file_name', right_on='spotify_id')

# Drop non-predictive columns
predictive_features = df.drop(['file_name', 'spotify_id', 'title', 'wav_path'], axis=1)

# Separate the predictors and targets
X = predictive_features.drop(['acousticness', 'danceability', 'energy', 'instrumentalness',
                              'liveness', 'loudness', 'speechiness', 'valence', 'tempo',
                              'song_key', 'time_signature', 'popularity'], axis=1)
y = predictive_features[['acousticness', 'danceability', 'energy', 'instrumentalness',
                         'liveness', 'loudness', 'speechiness', 'valence', 'tempo']]

# Optionally standardize the features
scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

joblib.dump(scaler, '../data/test/scaler.pkl')

# Define the PCA model
pca = PCA(n_components=0.95)  # Retain 95% of variance

# Fit PCA on the features
X_pca = pca.fit_transform(X_scaled)

joblib.dump(pca, '../data/test/pca.pkl')

# Create test DF with PCA applied, and analyze variance
pca_df = pd.DataFrame(X_pca, columns=[f'PC{i+1}' for i in range(X_pca.shape[1])])

print("\nExplained variance ratio per principal component:")
print(pca.explained_variance_ratio_)  # Shows the amount of variance captured by each component

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X_pca, y, test_size=0.2, random_state=1)

# Define the regression model
regressor = RandomForestRegressor(n_estimators=100, random_state=1)

# Fit the model
regressor.fit(X_train, y_train)

joblib.dump(regressor, '../data/test/regressor_model.pkl')