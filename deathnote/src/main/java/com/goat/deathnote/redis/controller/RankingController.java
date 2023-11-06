package com.goat.deathnote.redis.controller;

import com.goat.deathnote.redis.service.RankingService;
import org.springframework.web.bind.annotation.*;

import java.util.Set;

@RestController
@RequestMapping("/ranking")
public class RankingController {

    private final RankingService rankingService;

    public RankingController(RankingService rankingService) {
        this.rankingService = rankingService;
    }

    @PostMapping("/add")
    public void addToRank(@RequestParam String leaderBoard, @RequestParam String player, @RequestParam double score) {
        rankingService.addToRank(leaderBoard, player, score);
    }

    @GetMapping("/top/{count}")
    public Set<String> getTopPlayers(@RequestParam String leaderBoard, @PathVariable long count) {
        return rankingService.getTopPlayers(leaderBoard, count);
    }
}
