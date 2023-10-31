package com.goat.deathnote.domain.music.controller;

import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.service.MusicService;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import com.goat.deathnote.domain.world.entity.World;
import com.goat.deathnote.domain.world.service.WorldService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/souls")
public class MusicController {

    private final MusicService musicService;

    @Autowired
    public MusicController(MusicService musicService) {
        this.musicService = musicService;
    }

    @PostMapping
    public Music createMusic(@RequestBody Music music) {
        return musicService.saveMusic(music);
    }

    @GetMapping
    public List<Music> getAllMusics() {
        return musicService.getAllMusics();
    }

    @GetMapping("/{id}")
    public Optional<Music> getWorldById(@PathVariable Long id) {
        return musicService.getMusicById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteMusic(@PathVariable Long id) {
//        musicService.deleteMusic(id);
//    }
}
