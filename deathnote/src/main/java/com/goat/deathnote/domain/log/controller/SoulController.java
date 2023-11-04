package com.goat.deathnote.domain.log.controller;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.SoulService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/souls")
public class SoulController {

    private final SoulService soulService;

    @Autowired
    public SoulController(SoulService soulService) {
        this.soulService = soulService;
    }

    @PostMapping
    public Log createSoul(@RequestBody Log log) {
        return soulService.saveSoul(log);
    }

    @GetMapping
    public List<Log> getAllSouls() {
        return soulService.getAllSouls();
    }

    @GetMapping("/{id}")
    public Optional<Log> getSoulById(@PathVariable Long id) {
        return soulService.getSoulById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteSoul(@PathVariable Long id) {
//        soulService.deleteSoul(id);
//    }
}
