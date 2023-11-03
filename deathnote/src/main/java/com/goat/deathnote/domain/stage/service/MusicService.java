package com.goat.deathnote.domain.stage.service;

import com.goat.deathnote.domain.stage.entity.Stage;
import com.goat.deathnote.domain.stage.repository.MusicRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class MusicService {
    
    private final MusicRepository musicRepository;

    public MusicService(MusicRepository musicRepository) {
        this.musicRepository = musicRepository;
    }

    public Stage saveMusic (Stage music){
            return musicRepository.save(music);
        }

        public List<Stage> getAllMusics () {
            return musicRepository.findAll();
        }

        public Optional<Stage> getMusicById (Long id){
            return musicRepository.findById(id);
        }

//        public void deleteMusic (Long id){
//            musicRepository.deleteById(id);
//        }
    }
