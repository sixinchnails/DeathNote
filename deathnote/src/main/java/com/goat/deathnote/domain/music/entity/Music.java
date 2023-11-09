package com.goat.deathnote.domain.music.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@Builder
@Table(name = "music")
public class Music {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "music_id") // 기본 키
    private Long id;

    @Column(name = "music_title", nullable = false)
    private String title;

    @Column(name = "rmseP_a", nullable = false)
    private Float rmseP_a;

    @Column(name = "rmseP_std", nullable = false)
    private Float rmseP_std;

    @Column(name = "rmseH_a", nullable = false)
    private Float rmseH_a;

    @Column(name = "rmseH_std", nullable = false)
    private Float rmseH_std;

    @Column(name = "centroid_a", nullable = false)
    private Float centroid_a;

    @Column(name = "centroid_std", nullable = false)
    private Float centroid_std;

    @Column(name = "bw_a", nullable = false)
    private Float bw_a;

    @Column(name = "bw_std", nullable = false)
    private Float bw_std;

    @Column(name = "contrast_a", nullable = false)
    private Float contrast_a;

    @Column(name = "contrast_std", nullable = false)
    private Float contrast_std;

    @Column(name = "polyfeat_a", nullable = false)
    private Float polyfeat_a;

    @Column(name = "polyfeat_std", nullable = false)
    private Float polyfeat_std;

    @Column(name = "tonnetz_a", nullable = false)
    private Float tonnetz_a;

    @Column(name = "tonnetz_std", nullable = false)
    private Float tonnetz_std;

    @Column(name = "zcr_a", nullable = false)
    private Float zcr_a;

    @Column(name = "zcr_std", nullable = false)
    private Float zcr_std;

    @Column(name = "onset_a", nullable = false)
    private Float onset_a;

    @Column(name = "onset_std", nullable = false)
    private Float onset_std;

    @Column(name = "bpm", nullable = false)
    private Float bpm;

    @Column(name = "rmseP_skew", nullable = false)
    private Float rmseP_skew;

    @Column(name = "rmseP_kurtosis", nullable = false)
    private Float rmseP_kurtosis;

    @Column(name = "rmseH_skew", nullable = false)
    private Float rmseH_skew;

    @Column(name = "rmseH_kurtosis", nullable = false)
    private Float rmseH_kurtosis;

    @Column(name = "beats_a", nullable = false)
    private Float beats_a;

    @Column(name = "beats_std", nullable = false)
    private Float beats_std;

    @Column(name = "acousticness", nullable = false)
    private Float acousticness;

    @Column(name = "danceability", nullable = false)
    private Float danceability;

    @Column(name = "energy", nullable = false)
    private Float energy;

    @Column(name = "instrumentalness", nullable = false)
    private Float instrumentalness;

    @Column(name = "liveness", nullable = false)
    private Float liveness;

    @Column(name = "loudness", nullable = false)
    private Float loudness;

    @Column(name = "speechiness", nullable = false)
    private Float speechiness;

    @Column(name = "valence", nullable = false)
    private Float valence;

    @Column(name = "tempo", nullable = false)
    private Float tempo;

    @Column(name = "populatrity", nullable = false)
    private Float populatrity;

}