package com.goat.deathnote.domain.log.service;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class logService {

    private final LogRepository logRepository;

    public Log saveLog(Log log) {
        return logRepository.save(log);
    }

    public List<Log> getAllLogs() {
        return logRepository.findAll();
    }

    public Optional<Log> getLogById(Long id) {
        return logRepository.findById(id);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
