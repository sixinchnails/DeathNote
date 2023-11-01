package com.goat.deathnote.domain.world.controller;

import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import com.goat.deathnote.domain.world.entity.World;
import com.goat.deathnote.domain.world.service.WorldService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/worlds")
public class WorldController {

    private final WorldService worldService;

    @Autowired
    public WorldController(WorldService worldService) {
        this.worldService = worldService;
    }

    @PostMapping
    public World createWorld(@RequestBody World world) {
        return worldService.saveWorld(world);
    }

    @GetMapping
    public List<World> getAllWorlds() {
        return worldService.getAllWorlds();
    }

    @GetMapping("/{id}")
    public Optional<World> getWorldById(@PathVariable Long id) {
        return worldService.getWorldById(id);
    }

//    @DeleteMapping("/{id}")
//    public void deleteWorld(@PathVariable Long id) {
//        worldService.deleteWorld(id);
//    }
}
