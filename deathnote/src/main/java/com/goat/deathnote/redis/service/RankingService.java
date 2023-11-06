package com.goat.deathnote.redis.service;

import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Service;

import java.util.Set;

@Service
public class RankingService {

    private final RedisTemplate<String, String> redisTemplate;
    private final ZSetOperations<String, String> zSetOperations;

    public RankingService(RedisTemplate<String, String> redisTemplate) {
        this.redisTemplate = redisTemplate;
        this.zSetOperations = redisTemplate.opsForZSet();
    }

    public void addToRank(String leaderboard, String player, double score) {
//        POST /ranking/add?leaderboard=stage1&player=player1&score=100.0
        zSetOperations.add(leaderboard, player, score);
    }

    public Set<String> getTopPlayers(String leaderBoard, long count) {
        return zSetOperations.reverseRange(leaderBoard, 0, count - 1);
    }
}
