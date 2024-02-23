from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import train_test_split, RandomizedSearchCV
from sklearn.pipeline import make_pipeline
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
import pandas as pd
from DeathNote_Data.orm.Alchemy import getSpotifySongs

# Load and preprocess Spotify songs data
music_data = getSpotifySongs()

for music in music_data:
    music.pop('_sa_instance_state', None)

spotify_df = pd.DataFrame(music_data)

X = spotify_df.drop(['music_id', 'music_title', 'populatrity'], axis=1)
y = spotify_df['populatrity']

# Split the data into training and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

rf = RandomForestRegressor()

# Parameter grid for RandomizedSearchCV
param_grid_rf = {
    'n_estimators': [100, 200, 300],
    'max_features': ['sqrt', 'log2'],
    'max_depth': [10, 20, 30, None],
    'min_samples_split': [2, 5, 10],
    'min_samples_leaf': [1, 2, 4]
}

# Create the RandomizedSearchCV object
random_search_rf = RandomizedSearchCV(estimator=rf, param_distributions=param_grid_rf,
                                      n_iter=10, cv=3, random_state=42)

# Pipeline with MinMaxScaler, PCA, and a Random Forest Regressor
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