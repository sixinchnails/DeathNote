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

    private final CollectionService collectionService;

    @Autowired
    public CollectionController(CollectionService collectionService) {
        this.collectionService = collectionService;
    }

    @PostMapping
    public Collection createCollection(@RequestBody Collection music) {
        return collectionService.saveCollection(music);
    }

    @GetMapping
    public List<Collection> getAllCollections() {
        return collectionService.getAllCollections();
    }

    @GetMapping("/{id}")
    public Optional<Collection> getCollectionById(@PathVariable Long id) {
        return collectionService.getCollectionById(id);
    }

//        musicService.deleteMusic(id);
//    }
}
