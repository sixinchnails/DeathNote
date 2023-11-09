package com.goat.deathnote.domain.soul.controller;

import com.goat.deathnote.domain.soul.dto.SoulPostDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/souls")
public class SoulController {

    private final SoulService soulService;

    @Autowired
    public SoulController(SoulService soulService) {
        this.soulService = soulService;
    }

    @PostMapping
    public Long createSoul(@RequestBody SoulPostDto soulDto) { // 등록된 정령 id만 달래서
        Soul soul = soulService.saveSoul(soulDto);
        return soul.getId();
    }

    @GetMapping
    public ResponseEntity<List<Soul>> getAllSouls() {
        return ResponseEntity.ok(soulService.getAllSouls());
    }

    @GetMapping("/{soulName}")
    public List<Soul> getSoulByName(@PathVariable String soulName) {
        return soulService.getSoulByName(soulName);
    }

}
