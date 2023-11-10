package com.goat.deathnote.domain.garden.controller;

import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.garden.service.GardenService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequiredArgsConstructor
@RequestMapping("/gardens")
public class GardenController {

    private final GardenService gardenService;

    @PostMapping
    public ResponseEntity<Garden> createGarden(@RequestBody Garden garden) {
        return ResponseEntity.ok(gardenService.saveGarden(garden));
    }

    @GetMapping
    public ResponseEntity<List<Garden>> getAllGardens() {
        return ResponseEntity.ok(gardenService.getAllGardens());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Optional<Garden>> getGardenById(@PathVariable Long id) {
        return ResponseEntity.ok(gardenService.getGardenById(id));
    }

}
