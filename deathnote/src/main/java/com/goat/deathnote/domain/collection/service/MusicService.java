package com.goat.deathnote.domain.collection.service;

import com.goat.deathnote.domain.collection.entity.Collection;
import com.goat.deathnote.domain.collection.repository.MusicRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class MusicService {
    
    private final MusicRepository musicRepository;

    public MusicService(MusicRepository musicRepository) {
        this.musicRepository = musicRepository;
    }

    public Collection saveMusic (Collection music){
            return musicRepository.save(music);
        }

        public List<Collection> getAllMusics () {
            return musicRepository.findAll();
        }

        public Optional<Collection> getMusicById (Long id){
            return musicRepository.findById(id);
        }

//        public void deleteMusic (Long id){
//            musicRepository.deleteById(id);
//        }
    }
