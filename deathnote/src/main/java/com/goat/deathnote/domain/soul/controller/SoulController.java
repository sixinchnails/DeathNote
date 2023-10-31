package com.goat.deathnote.domain.soul.controller;

import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
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
    public Soul createSoul(@RequestBody Soul soul) {
        return soulService.saveSoul(soul);
    }

    @GetMapping
    public List<Soul> getAllSouls() {
        return soulService.getAllSouls();
    }

    @GetMapping("/{id}")
    public Optional<Soul> getSoulById(@PathVariable Long id) {
        return soulService.getSoulById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteSoul(@PathVariable Long id) {
//        soulService.deleteSoul(id);
//    }
}
