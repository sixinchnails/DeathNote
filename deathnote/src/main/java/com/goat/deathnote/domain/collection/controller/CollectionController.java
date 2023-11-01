package com.goat.deathnote.domain.collection.controller;

import com.goat.deathnote.domain.collection.entity.Collection;
import com.goat.deathnote.domain.collection.service.CollectionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/collections")
public class CollectionController {

    private final CollectionService musicService;

    @Autowired
    public CollectionController(CollectionService musicService) {
        this.musicService = musicService;
    }

    @PostMapping
    public Collection createMusic(@RequestBody Collection music) {
        return musicService.saveMusic(music);
    }

    @GetMapping
    public List<Collection> getAllMusics() {
        return musicService.getAllMusics();
    }

    @GetMapping("/{id}")
    public Optional<Collection> getWorldById(@PathVariable Long id) {
        return musicService.getMusicById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteMusic(@PathVariable Long id) {
//        musicService.deleteMusic(id);
//    }
}
