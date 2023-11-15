package com.goat.deathnote.domain.music.dto;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class MusicDto {
    private double valence;
    private double energy;
    private double acousticness;
    private double danceability;
    private double instrumentalness;
    private double liveness;
    private double loudness;
    private double speechiness;
    private double tempo;
}