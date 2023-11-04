package com.goat.deathnote.domain.stage.service;

import com.goat.deathnote.domain.stage.entity.Stage;
import com.goat.deathnote.domain.stage.repository.StageRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class StageService {
    
    private final StageRepository stageRepository;

    public StageService(StageRepository stageRepository) {
        this.stageRepository = stageRepository;
    }

    public Stage saveMusic (Stage music){
            return stageRepository.save(music);
        }

        public List<Stage> getAllMusics () {
            return stageRepository.findAll();
        }

        public Optional<Stage> getMusicById (Long id){
            return stageRepository.findById(id);
        }

//        public void deleteMusic (Long id){
//            musicRepository.deleteById(id);
//        }
    }
