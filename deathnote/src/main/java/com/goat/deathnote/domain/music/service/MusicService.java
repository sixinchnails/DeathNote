package com.goat.deathnote.domain.music.service;

import com.goat.deathnote.domain.music.dto.MusicDto;
import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.repository.MusicRepository;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;
import java.util.Optional;

@Service
public class MusicService {

    private final MusicRepository musicRepository;
    private final RestTemplate restTemplate;
    private final String pythonServerUrl = "http://52.79.253.69:5080/compose";

    public MusicService(MusicRepository musicRepository, RestTemplateBuilder restTemplateBuilder) {
        this.restTemplate = restTemplateBuilder.build();
        this.musicRepository = musicRepository;
    }

    public Music saveMusic(Music music) {
        return musicRepository.save(music);
    }

    public List<Music> getAllMusics() {
        return musicRepository.findAll();
    }

    public Optional<Music> getMusicById(Long id) {
        return musicRepository.findById(id);
    }

    public ResponseEntity<byte[]> getAudioFile(MusicDto musicDto) {
        ResponseEntity<byte[]> response = restTemplate.postForEntity(pythonServerUrl, musicDto, byte[].class);

        return response;
    }
}