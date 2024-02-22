import pandas as pd, numpy as np
from sklearn.model_selection import RandomizedSearchCV
from sklearn.preprocessing import MinMaxScaler
from DeathNote_Data.orm.alchemy import spotifyMusic
from DeathNote_Data.orm.dbutils import getSpotifySession
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor
from sklearn.model_selection import GridSearchCV

spotify_df = pd.read_csv('../data/spotify_output.csv')
feature_df = pd.read_csv('../data/feat_output.csv')

session = getSpotifySession()

tracks = session.query(spotifyMusic).all()

data = []

for track in tracks:
    # Use the __dict__ attribute but exclude SQLAlchemy internal attributes
    row_data = {key: value for key, value in track.__dict__.items() if not key.startswith('_')}
    data.append(row_data)

spotify_df = pd.DataFrame(data)

merged_df = pd.merge(spotify_df, feature_df, left_on='spotify_id', right_on='file_name')

columns = [
       'rmsep_a', 'rmsep_std', 'rmseh_a', 'rmseh_std',
       'centroid_a', 'centroid_std', 'bw_a', 'bw_std', 'contrast_a',
       'contrast_std', 'polyfeat_a', 'polyfeat_std', 'tonnetz_a',
       'tonnetz_std', 'zcr_a', 'zcr_std', 'onset_a', 'onset_std', 'bpm',
       'rmsep_skew', 'rmsep_kurtosis', 'rmseh_skew', 'rmseh_kurtosis',
       'beats_a', 'beats_std', 'popularity'
]

string_df = spotify_df[['spotify_id', 'title']]

filter_df = merged_df[columns]

X = filter_df.drop('popularity', axis=1)
Y = filter_df['popularity']

X_convert = X.iloc[:, 2:]

scaler = MinMaxScaler()
X_scaled = pd.DataFrame(scaler.fit_transform(X_convert), columns=X.columns[2:])
X_scaled.index = filter_df.index

train_x, test_x, train_y, test_y = train_test_split(X_scaled, Y)

n_estimators = [int(x) for x in np.linspace(start=200, stop=2000, num=10)]

max_features = ['auto', 'sqrt']
max_depth = [int(x) for x in np.linspace(10, 110, num=11)]
max_depth.append(None)
min_samples_split = [2, 5, 10]
min_samples_leaf = [1, 2, 4]
bootstrap = [True, False]

random_grid = {'n_estimators': n_estimators,
               'max_features': max_features,
               'max_depth': max_depth,
               'min_samples_split': min_samples_split,
               'min_samples_leaf': min_samples_leaf,
               'bootstrap': bootstrap}

rf = RandomForestRegressor(random_state = 42)
rf_random = RandomizedSearchCV(estimator=rf, param_distributions=random_grid, n_iter=100, cv=3, verbose=2, random_state=42, n_jobs=-1)
rf_random.fit(train_x, train_y)

print(rf_random.best_params_)

def evaluate(model, test_features, test_labels):
    predictions = model.predict(test_features)
    errors = abs(predictions - test_labels)
    mape = 100 * np.mean(errors / test_labels)
    accuracy = 100 - mape

    print('Model Performance')
    print('Average Error: {:0.4f} degrees.'.format(np.mean(errors)))
    print('Accuracy = {:0.2f}%'.format(accuracy))

    return accuracy

base_model = RandomForestRegressor(n_estimators=10, random_state=42)
base_model.fit(train_x, train_y)
base_accuracy = evaluate(base_model, train_x, train_y)

best_random = rf_random.best_estimator_
random_accuracy = evaluate(best_random, train_x, train_y)

print('Improvement of {:0.2f}%'.format(100 * (random_accuracy - base_accuracy) / base_accuracy))

param_grid = {
    'bootstrap': [True],
    'max_depth': [80, 90, 100, 110],
    'max_features':[2, 3],
    'min_samples_leaf': [3, 4, 5],
    'min_samples_split': [8, 10, 12],
    'n_estimators': [100, 200, 300, 1000]
}

grid_search = GridSearchCV(estimator = rf, param_grid = param_grid, cv = 3, n_jobs = -1, verbose = 2)

grid_search.fit(train_x, train_y)

print(grid_search.best_params_)

best_grid = grid_search.best_estimator_
grid_accuracy = evaluate(best_grid, train_x, train_y)

print('Improvement f {:0.2f}%.'.format(100 * (grid_accuracy - base_accuracy) / base_accuracy))

best_params = grid_search.best_params_
grid_rf_model = RandomForestRegressor(**best_params, random_state = 42)
grid_rf_model.fit(train_x, train_y)

predictions = grid_rf_model.predict(test_x)

print(predictions)
