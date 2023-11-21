package com.goat.deathnote.domain.world.service;

import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import com.goat.deathnote.domain.world.entity.World;
import com.goat.deathnote.domain.world.repository.WorldRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class WorldService {
    
    private final WorldRepository worldRepository;

    public WorldService(WorldRepository worldRepository) {
        this.worldRepository = worldRepository;
    }

    public World saveWorld (World world){
            return worldRepository.save(world);
        }

        public List<World> getAllWorlds () {
            return worldRepository.findAll();
        }

        public Optional<World> getWorldById (Long id){
            return worldRepository.findById(id);
        }

//        public void deleteWorld (Long id){
//            soulRepository.deleteById(id);
//        }
    }
