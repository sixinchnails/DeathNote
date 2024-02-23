import numpy as np
import librosa as lb
import glob
from collections import OrderedDict
from scipy.stats import skew
from scipy.stats import kurtosis

data = {}
name_list = []
path = 'aiva/'

files = [f for f in glob.glob(path + "*.wav", recursive=True)]


def get_features_mean(y, sr, hop_length, n_fft):
    # try:
    print('extracting features...')
    # Split song into harmonic and percussive parts
    y_harmonic, y_percussive = lb.effects.hpss(y)

    # Compute power spectrogram.
    stft_harmonic = lb.core.stft(y_harmonic, n_fft=n_fft, hop_length=hop_length)
    stft_percussive = lb.core.stft(y_percussive, n_fft=n_fft, hop_length=hop_length)

    rmsH = np.sqrt(np.mean(np.abs(lb.feature.rms(S=stft_harmonic)) ** 2, axis=0, keepdims=True))
    rmsH_a = np.mean(rmsH)
    rmsH_std = np.std(rmsH)
    rmsH_skew = skew(np.mean(rmsH, axis=0))
    rmsH_kurtosis = kurtosis(np.mean(rmsH, axis=0), fisher=True, bias=True)
    rmsP = np.sqrt(np.mean(np.abs(lb.feature.rms(S=stft_percussive)) ** 2, axis=0, keepdims=True))
    rmsP_a = np.mean(rmsP)
    rmsP_std = np.std(rmsP)
    rmsP_skew = skew(np.mean(rmsP, axis=0))
    rmsP_kurtosis = kurtosis(np.mean(rmsP, axis=0), fisher=True, bias=True)

    centroid = lb.feature.spectral_centroid(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)
    centroid_a = np.mean(centroid)
    centroid_std = np.std(centroid)

    # Compute p’th-order spectral bandwidth
    bw = lb.feature.spectral_bandwidth(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)
    bw_a = np.mean(bw)
    bw_std = np.std(bw)

    # Compute spectral contrast
    contrast = lb.feature.spectral_contrast(y=y, sr=sr, n_fft=n_fft, hop_length=hop_length)
    contrast_a = np.mean(contrast)
    contrast_std = np.std(contrast)

    # Get coefficients of fitting an nth-order polynomial to the columns of a spectrogram.
    polyfeat = lb.feature.poly_features(y=y_harmonic, sr=sr, n_fft=n_fft, hop_length=hop_length)
    polyfeat_a = np.mean(polyfeat[0])
    polyfeat_std = np.std(polyfeat[0])

    tonnetz = lb.feature.tonnetz(y=lb.effects.harmonic(y=y_harmonic), sr=sr)
    tonnetz_a = np.mean(tonnetz)
    tonnetz_std = np.std(tonnetz)

    zcr = lb.feature.zero_crossing_rate(y=y, hop_length=hop_length)  # zero crossing rate
    zcr_a = np.mean(zcr)
    zcr_std = np.std(zcr)

    onset_env = lb.onset.onset_strength(y=y_percussive, sr=sr)
    onset_a = np.mean(onset_env)
    onset_std = np.std(onset_env)

    D = lb.stft(y)

    bpm, beats = lb.beat.beat_track(y=y_percussive, sr=sr, onset_envelope=onset_env, units='time')
    beats_a = np.mean(beats)
    beats_std = np.std(beats)

    features_dict = OrderedDict({'rmsep_a': rmsP_a, 'rmsep_std': rmsP_std, 'rmseh_a': rmsH_a, 'rmseh_std': rmsH_std,
                                 'centroid_a': centroid_a, 'centroid_std': centroid_std, 'bw_a': bw_a, 'bw_std': bw_std,
                                 'contrast_a': contrast_a, 'contrast_std': contrast_std, 'polyfeat_a': polyfeat_a,
                                 'polyfeat_std': polyfeat_std, 'tonnetz_a': tonnetz_a, 'tonnetz_std': tonnetz_std,
                                 'zcr_a': zcr_a, 'zcr_std': zcr_std, 'onset_a': onset_a, 'onset_std': onset_std,
                                 'bpm': bpm, 'rmsep_skew': rmsP_skew, 'rmsep_kurtosis': rmsP_kurtosis,
                                 'rmseh_skew': rmsH_skew, 'rmseh_kurtosis': rmsH_kurtosis, 'beats_a': beats_a,
                                 'beats_std': beats_std})

    # combine_features = {**features_dict, **bands_dict}
    print('features extracted successfully')

    return features_dict
