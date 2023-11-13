package com.goat.deathnote.redis.controller;

import com.goat.deathnote.redis.dto.ReponseRankingDto;
import com.goat.deathnote.redis.service.RankingService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
@RestController
@RequestMapping("/rankings")
@RequiredArgsConstructor
public class RankingController {

    private final RankingService rankingService;

    // 테스트용
//    @GetMapping
//    public ResponseEntity<List<ReponseRankingDto>> getRankingByCode() {
//        try {
//            return ResponseEntity.ok(rankingService.createRankingResponse());
//        } catch (Exception e){
//            return ResponseEntity.badRequest().build();
//        }
//    }

    @GetMapping("/{code}")
    public List<ReponseRankingDto> getRankingResponse(@PathVariable String code) {
        return rankingService.getRankingResponse(code);
    }

}
