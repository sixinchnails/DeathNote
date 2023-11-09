package com.goat.deathnote.domain.music.service;

import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.repository.MusicRepository;
import com.goat.deathnote.domain.world.entity.World;
import com.goat.deathnote.domain.world.repository.WorldRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class MusicService {

    private final MusicRepository musicRepository;

    public MusicService(MusicRepository musicRepository) {
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

}
