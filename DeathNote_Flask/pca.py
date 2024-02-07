import pandas as pd
from sklearn.decomposition import PCA
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error
from sklearn.ensemble import RandomForestRegressor
from math import sqrt
import joblib
#import pickle
import numpy as np

"""
Scaler, PCA, Regressor 생성 코드
"""

# Set to display all columns in the dataframe
pd.set_option('display.max_columns', None)
np.set_printoptions(formatter={'float_kind':'{:.8f}'.format})

# Load your data
features_df = pd.read_csv('feat_output.csv')
targets_df = pd.read_csv('spotify_output.csv')

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

# with open('scaler.pkl', 'wb') as file:
#     pickle.dump(scaler, file)

joblib.dump(scaler, 'scaler.pkl')

# Define the PCA model
pca = PCA(n_components=0.95)  # Retain 95% of variance

# Fit PCA on the features
X_pca = pca.fit_transform(X_scaled)

# with open('pca_model.pkl', 'wb') as file:
#     pickle.dump(pca, file)

joblib.dump(pca, 'pca.pkl')

# Create a DataFrame of the PCA-transformed features for inspection
pca_df = pd.DataFrame(X_pca, columns=[f'PC{i+1}' for i in range(X_pca.shape[1])])

print("\nExplained variance ratio per principal component:")
print(pca.explained_variance_ratio_)  # Shows the amount of variance captured by each component

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X_pca, y, test_size=0.2, random_state=1)

# Define the regression model
regressor = RandomForestRegressor(n_estimators=100, random_state=1)

# Fit the model
regressor.fit(X_train, y_train)

joblib.dump(regressor, 'regressor_model.pkl')

# Predict on the test data
y_pred = regressor.predict(X_test)

# Calculate RMSE for each target variable
rmse_scores = [sqrt(mean_squared_error(y_test.iloc[:,i], y_pred[:,i])) for i in range(y_test.shape[1])]

for i, col in enumerate(y.columns):
    print(f'RMSE for {col}: {rmse_scores[i]:.4f}')

sample_data = {
    'rmseP_a': [0.1],           # Replace with actual rmseP_a value
    'rmseP_std': [0.2],         # Replace with actual rmseP_std value
    'rmseH_a': [0.05],          # Replace with actual rmseH_a value
    'rmseH_std': [0.07],        # Replace with actual rmseH_std value
    'centroid_a': [0.3],        # Replace with actual centroid_a value
    'centroid_std': [0.4],      # Replace with actual centroid_std value
    'bw_a': [0.5],              # Replace with actual bw_a value
    'bw_std': [0.6],            # Replace with actual bw_std value
    'contrast_a': [0.1],        # Replace with actual contrast_a value
    'contrast_std': [0.1],      # Replace with actual contrast_std value
    'polyfeat_a': [0.2],        # Replace with actual polyfeat_a value
    'polyfeat_std': [0.2],      # Replace with actual polyfeat_std value
    'tonnetz_a': [0.3],         # Replace with actual tonnetz_a value
    'tonnetz_std': [0.3],       # Replace with actual tonnetz_std value
    'zcr_a': [0.4],             # Replace with actual zcr_a value
    'zcr_std': [0.4],           # Replace with actual zcr_std value
    'onset_a': [0.5],           # Replace with actual onset_a value
    'onset_std': [0.5],         # Replace with actual onset_std value
    'bpm': [120],               # Replace with actual bpm value
    'rmseP_skew': [0.1],        # Replace with actual rmseP_skew value
    'rmseP_kurtosis': [0.1],    # Replace with actual rmseP_kurtosis value
    'rmseH_skew': [0.2],        # Replace with actual rmseH_skew value
    'rmseH_kurtosis': [0.2],    # Replace with actual rmseH_kurtosis value
    'beats_a': [0.3],           # Replace with actual beats_a value
    'beats_std': [0.3],         # Replace with actual beats_std value
}

another_sample_data = {
    'rmseP_a': [0.15],            # Replace with actual rmseP_a value
    'rmseP_std': [0.25],          # Replace with actual rmseP_std value
    'rmseH_a': [0.1],             # Replace with actual rmseH_a value
    'rmseH_std': [0.15],          # Replace with actual rmseH_std value
    'centroid_a': [0.35],         # Replace with actual centroid_a value
    'centroid_std': [0.45],       # Replace with actual centroid_std value
    'bw_a': [0.55],               # Replace with actual bw_a value
    'bw_std': [0.65],             # Replace with actual bw_std value
    'contrast_a': [0.2],          # Replace with actual contrast_a value
    'contrast_std': [0.2],        # Replace with actual contrast_std value
    'polyfeat_a': [0.25],         # Replace with actual polyfeat_a value
    'polyfeat_std': [0.25],       # Replace with actual polyfeat_std value
    'tonnetz_a': [0.35],          # Replace with actual tonnetz_a value
    'tonnetz_std': [0.35],        # Replace with actual tonnetz_std value
    'zcr_a': [0.45],              # Replace with actual zcr_a value
    'zcr_std': [0.5],             # Replace with actual zcr_std value
    'onset_a': [0.55],            # Replace with actual onset_a value
    'onset_std': [0.6],           # Replace with actual onset_std value
    'bpm': [130],                 # Replace with actual bpm value
    'rmseP_skew': [0.15],         # Replace with actual rmseP_skew value
    'rmseP_kurtosis': [0.2],      # Replace with actual rmseP_kurtosis value
    'rmseH_skew': [0.25],         # Replace with actual rmseH_skew value
    'rmseH_kurtosis': [0.3],      # Replace with actual rmseH_kurtosis value
    'beats_a': [0.35],            # Replace with actual beats_a value
    'beats_std': [0.4],           # Replace with actual beats_std value
}

loaded_scaler = joblib.load('scaler.pkl')
loaded_pca = joblib.load('pca.pkl')
loaded_regressor = joblib.load('regressor_model.pkl')

sample_df = pd.DataFrame(sample_data)
another_sample_df = pd.DataFrame(another_sample_data)

scaled_data = loaded_scaler.transform(sample_df)

pca_data = loaded_pca.transform(scaled_data)

prediction = regressor.predict(pca_data)

another_scaled_data = loaded_scaler.transform(another_sample_df)
another_pca_data = loaded_pca.transform(scaled_data)
another_prediction = regressor.predict(another_pca_data)

pd.set_option('display.float_format', '{:.8f'.format)
