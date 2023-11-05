package com.goat.deathnote.domain.log.controller;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.logService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/logs")
public class LogController {

    private final logService logService;

    @Autowired
    public LogController(logService logService) {
        this.logService = logService;
    }

    @PostMapping
    public Log createLog(@RequestBody Log log) {
        return logService.saveSoul(log);
    }

    @GetMapping
    public List<Log> getAllLogs() {
        return logService.getAllSouls();
    }

    @GetMapping("/{id}")
    public Optional<Log> getLogById(@PathVariable Long id) {
        return logService.getSoulById(id);
    }

}
