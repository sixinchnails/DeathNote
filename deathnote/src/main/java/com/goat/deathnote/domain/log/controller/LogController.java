package com.goat.deathnote.domain.log.controller;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.logService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequiredArgsConstructor
@RequestMapping("/logs")
public class LogController {

    private final logService logService;

    @PostMapping
    public ResponseEntity<Log> createLog(@RequestBody Log log) {
        return ResponseEntity.ok(logService.saveLog(log));
    }

    @GetMapping
    public ResponseEntity<List<Log>> getAllLogs() {
        return ResponseEntity.ok(logService.getAllLogs());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Optional<Log>> getLogById(@PathVariable Long id) {
        return ResponseEntity.ok(logService.getLogById(id));
    }

}
