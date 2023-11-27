# import pandas as pd
# import seaborn as sns
import numpy as np
import os
import librosa as lb
import glob
# from pprint import pprint as pp
# import scipy
# import matplotlib.pyplot as plt
import csv
# plt.rcParams["figure.figsize"] = (14,5)

# from more_itertools import unique_everseen
from collections import OrderedDict
# import soundfile as sf
# import difflib
# import statistics
# import itertools
from scipy.stats import skew
from scipy.stats import kurtosis

data ={}
name_list =[]
path = 'aiva/'

files = [f for f in glob.glob(path + "*.wav", recursive=True)]
print(files)

#for file in files:
#    name = file.split('\\')[-1].split('.')[0]
#    y, sr = lb.load(file, sr=44100)
#    data[name] = {'y' : y, 'sr': sr}

#print(data)

def get_features_mean(y, sr, hop_length, n_fft):
    # try:
    print('extracting features...')
    y_harmonic, y_percussive = lb.effects.hpss(y)  # split song into harmonic and percussive parts
    print("Phase 0")
    stft_harmonic = lb.core.stft(y_harmonic, n_fft=n_fft, hop_length=hop_length)  # Compute power spectrogram.
    stft_percussive = lb.core.stft(y_percussive, n_fft=n_fft, hop_length=hop_length)  # Compute power spectrogram.
    print("Phase 1")
    rmsH = np.sqrt(np.mean(np.abs(lb.feature.rms(S=stft_harmonic)) ** 2, axis=0, keepdims=True))
    rmsH_a = np.mean(rmsH)
    rmsH_std = np.std(rmsH)
    rmsH_skew = skew(np.mean(rmsH, axis=0))
    rmsH_kurtosis = kurtosis(np.mean(rmsH, axis=0), fisher=True, bias=True)
    print("Phase 2")
    rmsP = np.sqrt(np.mean(np.abs(lb.feature.rms(S=stft_percussive)) ** 2, axis=0, keepdims=True))
    rmsP_a = np.mean(rmsP)
    rmsP_std = np.std(rmsP)
    rmsP_skew = skew(np.mean(rmsP, axis=0))
    rmsP_kurtosis = kurtosis(np.mean(rmsP, axis=0), fisher=True, bias=True)
    print("Phase 3")
    centroid = lb.feature.spectral_centroid(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)
    centroid_a = np.mean(centroid)
    centroid_std = np.std(centroid)
    print("Phase 4")
    bw = lb.feature.spectral_bandwidth(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)  # Compute p’th-order spectral bandwidth:
    bw_a = np.mean(bw)
    bw_std = np.std(bw)
    print("Phase 5")
    contrast = lb.feature.spectral_contrast(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)  # Compute spectral contrast [R16]
    contrast_a = np.mean(contrast)
    contrast_std = np.std(contrast)
    print("Phase 6")
    polyfeat = lb.feature.poly_features(y=y_harmonic, sr=sr, n_fft=n_fft, hop_length=hop_length)  # Get coefficients of fitting an nth-order polynomial to the columns of a spectrogram.
    polyfeat_a = np.mean(polyfeat[0])
    polyfeat_std = np.std(polyfeat[0])
    print("Phase 7")
    tonnetz = lb.feature.tonnetz(y=lb.effects.harmonic(y=y_harmonic), sr=sr)  # Computes the tonal centroid features (tonnetz), following the method of [R17].
    tonnetz_a = np.mean(tonnetz)
    tonnetz_std = np.std(tonnetz)
    print("Phase 8")
    zcr = lb.feature.zero_crossing_rate(y=y, hop_length=hop_length)  # zero crossing rate
    zcr_a = np.mean(zcr)
    zcr_std = np.std(zcr)
    print("Phase 9")
    onset_env = lb.onset.onset_strength(y=y_percussive, sr=sr)
    onset_a = np.mean(onset_env)
    onset_std = np.std(onset_env)
    print("Phase 10")
    D = lb.stft(y)
    times = lb.frames_to_time(np.arange(D.shape[1]))  # not returned, but could be if you want to plot things as a time series
    print("Phase 11")
    bpm, beats = lb.beat.beat_track(y=y_percussive, sr=sr, onset_envelope=onset_env, units='time')
    beats_a = np.mean(beats)
    beats_std = np.std(beats)
    print("Phase 12")
    features_dict = OrderedDict({'rmsep_a': rmsP_a, 'rmsep_std': rmsP_std, 'rmseh_a': rmsH_a, 'rmseh_std': rmsH_std,
                                 'centroid_a': centroid_a, 'centroid_std': centroid_std, 'bw_a': bw_a, 'bw_std': bw_std,
                                 'contrast_a': contrast_a, 'contrast_std': contrast_std, 'polyfeat_a': polyfeat_a,
                                 'polyfeat_std': polyfeat_std, 'tonnetz_a': tonnetz_a, 'tonnetz_std': tonnetz_std,
                                 'zcr_a': zcr_a, 'zcr_std': zcr_std, 'onset_a': onset_a, 'onset_std': onset_std,
                                 'bpm': bpm, 'rmsep_skew': rmsP_skew, 'rmsep_kurtosis': rmsP_kurtosis,
                                 'rmseh_skew': rmsH_skew, 'rmseh_kurtosis': rmsH_kurtosis, 'beats_a':beats_a,
                                 'beats_std':beats_std})

    # combine_features = {**features_dict, **bands_dict}
    print('features extracted successfully')

    return features_dict

#for key in data.keys():
#    res = get_features_mean(y=data[key]['y'], sr=data[key]['sr'], hop_length=512, n_fft=2048)
#    print(res)

# os.remove('feat_output.csv')
# csv_file = 'feat_output.csv'
#
# with open(csv_file, 'a', newline='') as csvfile:
#     for key in data.keys():
#         res = get_features_mean(y=data[key]['y'], sr=data[key]['sr'], hop_length=512, n_fft=2048)
#
#         # Add the file's name to the OrderedDict
#         res['file_name'] = key
#
#         # Open the CSV file in append mode and create a CSV writer
#         fieldnames = ['file_name'] + list(res.keys())
#         writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
#
#         # Write headers in the first iteration
#         if key == next(iter(data.keys())):
#             writer.writeheader()
#
#         # Write the data to a new row
#         writer.writerow({**{'file_name': key}, **res})
#
# print(f'Data has been written to {csv_file}')

csv_file = 'feat_output.csv'

#Check if the file already exists and delete it
# if os.path.exists(csv_file):
#     os.remove(csv_file)
#
# for key in data.keys():
#     res = get_features_mean(y=data[key]['y'], sr=data[key]['sr'], hop_length=512, n_fft=2048)
#
#     # Open the CSV file in append mode and create a CSV writer
#     with open(csv_file, 'a', newline='') as csvfile:
#         fieldnames = ['file_name'] + list(res.keys())  # Add 'file_name' as the first column
#         writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
#
#         # Write headers in the first iteration
#         if key == next(iter(data.keys())):
#             writer.writeheader()
#
#         # Add the file name to the res dictionary
#         res['file_name'] = key
#
#         # Write the data to a new row
#         writer.writerow(res)
