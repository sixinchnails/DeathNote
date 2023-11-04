package com.goat.deathnote.domain.soul.service;

import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class SoulService {

    private final SoulRepository soulRepository;

    public SoulService(SoulRepository soulRepository) {
        this.soulRepository = soulRepository;
    }

    public Soul saveSoul(Soul soul) {
        return soulRepository.save(soul);
    }

    public List<Soul> getAllSouls() {
        return soulRepository.findAll();
    }

    public Optional<Soul> getSoulById(Long id) {
        return soulRepository.findById(id);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
