from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import train_test_split, RandomizedSearchCV
from sklearn.pipeline import make_pipeline
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
import pandas as pd

from alchemy import getSpotifySongs, getComposeSongs

# Assuming getSpotifySongs and getComposeSongs are functions that retrieve your data

# Load and preprocess Spotify songs data
spotifySongs = getSpotifySongs()
music_data = [m.__dict__ for m in spotifySongs]
for music in music_data:
    music.pop('_sa_instance_state', None)
spotify_df = pd.DataFrame(music_data)

X = spotify_df.drop(['music_id', 'music_title', 'populatrity'], axis=1)
y = spotify_df['populatrity']

# Split the data into training and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

rf = RandomForestRegressor()

# Define the parameter grid for RandomizedSearchCV
param_grid_rf = {
    'n_estimators': [100, 200, 300],
    'max_features': ['sqrt', 'log2'],  # Corrected this line
    'max_depth': [10, 20, 30, None],
    'min_samples_split': [2, 5, 10],
    'min_samples_leaf': [1, 2, 4],
    # Add more parameters here if needed
}

# Create the RandomizedSearchCV object
random_search_rf = RandomizedSearchCV(estimator=rf, param_distributions=param_grid_rf,
                                      n_iter=10, cv=3, random_state=42)
# Define a pipeline with MinMaxScaler, PCA, and a Random Forest Regressor
pipeline = make_pipeline(
    MinMaxScaler(),
    PCA(n_components=0.95),
    random_search_rf
)

# Train the model
pipeline.fit(X_train, y_train)
y_pred = pipeline.predict(X_test)

mse = mean_squared_error(y_test, y_pred)
print(f"Mean Squared Error: {mse}")
#
# # Load and preprocess Compose songs data
# composeSongs = getComposeSongs()
# compose_data = [m.__dict__ for m in composeSongs]
# for compose in compose_data:
#     compose.pop('_sa_instance_state', None)
# compose_df = pd.DataFrame(compose_data)
# input_compose_df = compose_df[X.columns]  # Ensure columns are in the same order
#
# # Predict
# prediction = pipeline.predict(input_compose_df)
#
# # Add predictions to the DataFrame
# compose_df['predicted_popularity'] = prediction