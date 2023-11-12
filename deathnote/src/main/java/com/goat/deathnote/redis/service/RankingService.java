package com.goat.deathnote.redis.service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.redis.dto.RankingResponseDto;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;

import java.util.Set;

@Service
@RequiredArgsConstructor
public class RankingService {

    private final String RANKING_KEY = "ranking";

    private final StringRedisTemplate redisTemplate;

    @Autowired
    private ObjectMapper objectMapper; // Jackson ObjectMapper
    public void updateRanking(String code, Object rankingResponseDto) throws JsonProcessingException {

        String dtoJson = objectMapper.writeValueAsString(rankingResponseDto);

        redisTemplate.opsForList().rightPush(code, dtoJson);
    }

    public Set<ZSetOperations.TypedTuple<String>> getTopMembers(Long count) {
        return redisTemplate.opsForZSet().reverseRangeWithScores(RANKING_KEY, 0, count - 1);
    }

}
