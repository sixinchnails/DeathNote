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

    public Stage saveStage(Stage stage) {
        return stageRepository.save(stage);
    }

    public List<Stage> getAllStages() {
        return stageRepository.findAll();
    }

    public Optional<Stage> getStageById(Long id) {
        return stageRepository.findById(id);
    }

//        public void deleteMusic (Long id){
//            musicRepository.deleteById(id);
//        }
}
