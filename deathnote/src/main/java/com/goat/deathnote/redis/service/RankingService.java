package com.goat.deathnote.redis.service;

import com.goat.deathnote.domain.member.entity.Member;
import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;

import java.util.Set;

@Service
@RequiredArgsConstructor
public class RankingService {

    private final String RANKING_KEY = "ranking";

    private final StringRedisTemplate redisTemplate;


    public void updateRanking(Member member, Long score) {
        redisTemplate.opsForZSet().add(RANKING_KEY, member.getId().toString(), score);
    }

    public Set<ZSetOperations.TypedTuple<String>> getTopMembers(Long count) {
        return redisTemplate.opsForZSet().reverseRangeWithScores(RANKING_KEY, 0, count - 1);
    }
}
