package com.goat.deathnote.domain.stage.controller;

import com.goat.deathnote.domain.stage.dto.MemberRankingDTO;
import com.goat.deathnote.domain.stage.entity.Stage;
import com.goat.deathnote.domain.stage.service.StageService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequiredArgsConstructor
@RequestMapping("/stages")
public class StageController {

    private final StageService stageService;

    @PostMapping
    public Stage createStage(@RequestBody Stage stage) {
        return stageService.saveStage(stage);
    }

    @GetMapping
    public List<Stage> getAllStages() {
        return stageService.getAllStages();
    }

    @GetMapping("/{id}")
    public Optional<Stage> getStageById(@PathVariable Long id) {
        return stageService.getStageById(id);
    }

//    @GetMapping("/{stageId}/ranking")
//    public List<MemberRankingDTO> getRankingForStage(@PathVariable Long stageId) {
//        Optional<Stage> stage = stageService.getStageById(stageId);
//        return stageService.getRankingForStage(stage);
//    }
}
