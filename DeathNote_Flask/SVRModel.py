from sklearn.svm import SVR
from sklearn.metrics import mean_squared_error
from sklearn.model_selection import train_test_split, RandomizedSearchCV
from sklearn.pipeline import make_pipeline
from sklearn.decomposition import PCA
from sklearn.preprocessing import MinMaxScaler
import pandas as pd
from alchemy import getSpotifySongs

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

svr = SVR()

param_grid_svr = {
    'C': [0.1, 1, 10],
    'gamma': ['scale', 'auto'],
    'epsilon': [0.01, 0.1, 0.2],
}

random_search_svr = RandomizedSearchCV(estimator=svr, param_distributions=param_grid_svr,
                                       n_iter=10, cv=3, random_state=42)

# Define a pipeline with MinMaxScaler, PCA, and SVR
pipeline_svr = make_pipeline(
    MinMaxScaler(),
    PCA(n_components=0.95),
    random_search_svr
)

# Train the model
pipeline_svr.fit(X_train, y_train)
y_pred_svr = pipeline_svr.predict(X_test)

# Calculate MSE
mse_svr = mean_squared_error(y_test, y_pred_svr)
print(f"Mean Squared Error with SVR: {mse_svr}")
