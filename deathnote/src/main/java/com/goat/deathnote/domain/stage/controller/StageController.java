package com.goat.deathnote.domain.stage.controller;

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
    public Stage createMusic(@RequestBody Stage music) {
        return stageService.saveMusic(music);
    }

    @GetMapping
    public List<Stage> getAllMusics() {
        return stageService.getAllMusics();
    }

    @GetMapping("/{id}")
    public Optional<Stage> getMusicById(@PathVariable Long id) {
        return stageService.getMusicById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteMusic(@PathVariable Long id) {
//        musicService.deleteMusic(id);
//    }
}
