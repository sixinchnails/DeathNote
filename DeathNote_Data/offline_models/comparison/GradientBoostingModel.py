from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import train_test_split, RandomizedSearchCV
from sklearn.pipeline import make_pipeline
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
import pandas as pd
from DeathNote_Data.orm.entities.SpotifyMusic import getSpotifySongs

# Load and preprocess Spotify songs data
music_data = getSpotifySongs()

for music in music_data:
    music.pop('_sa_instance_state', None)

spotify_df = pd.DataFrame(music_data)

X = spotify_df.drop(['music_id', 'music_title', 'populatrity'], axis=1)
y = spotify_df['populatrity']

# Split the data into training and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

gbr = GradientBoostingRegressor()

param_grid_gbr = {
    'n_estimators': [100, 200, 300],
    'learning_rate': [0.01, 0.1, 0.2],
    'max_depth': [3, 4, 5]
}

# Create the RandomizedSearchCV object
random_search_gbr = RandomizedSearchCV(estimator=gbr, param_distributions=param_grid_gbr,
                                       n_iter=10, cv=3, random_state=42)

# Pipeline with MinMaxScaler, PCA, and Gradient Boosting Regressor
pipeline_gb = make_pipeline(
    MinMaxScaler(),
    PCA(n_components=0.95),
    random_search_gbr
)

# Train the model
pipeline_gb.fit(X_train, y_train)
y_pred_gb = pipeline_gb.predict(X_test)

# Calculate MSE
mse_gb = mean_squared_error(y_test, y_pred_gb)
print(f"Mean Squared Error with Gradient Boosting: {mse_gb}")