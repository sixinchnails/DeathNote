package com.goat.deathnote.domain.log.service;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class logService {

    private final LogRepository soulRepository;

    public logService(LogRepository soulRepository) {
        this.soulRepository = soulRepository;
    }

    public Log saveSoul(Log log) {
        return soulRepository.save(log);
    }

    public List<Log> getAllSouls() {
        return soulRepository.findAll();
    }

    public Optional<Log> getSoulById(Long id) {
        return soulRepository.findById(id);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
