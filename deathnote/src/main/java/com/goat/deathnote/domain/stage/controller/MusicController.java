package com.goat.deathnote.domain.stage.controller;

import com.goat.deathnote.domain.stage.entity.Stage;
import com.goat.deathnote.domain.stage.service.MusicService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequiredArgsConstructor
@RequestMapping("/musics")
public class MusicController {

    private final MusicService musicService;


    @PostMapping
    public Stage createMusic(@RequestBody Stage music) {
        return musicService.saveMusic(music);
    }

    @GetMapping
    public List<Stage> getAllMusics() {
        return musicService.getAllMusics();
    }

    @GetMapping("/{id}")
    public Optional<Stage> getMusicById(@PathVariable Long id) {
        return musicService.getMusicById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteMusic(@PathVariable Long id) {
//        musicService.deleteMusic(id);
//    }
}
