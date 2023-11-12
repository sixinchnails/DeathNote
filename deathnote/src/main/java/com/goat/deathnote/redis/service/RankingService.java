package com.goat.deathnote.redis.service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.LogService;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import com.goat.deathnote.redis.dto.RankingResponseDto;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

@Service
@RequiredArgsConstructor
public class RankingService {

    private final String RANKING_KEY = "ranking";

    private final StringRedisTemplate redisTemplate;
    private final LogService logService;
    private final SoulService soulService;

    @Autowired
    private ObjectMapper objectMapper; // Jackson ObjectMapper

//    @Scheduled(fixedRate = 3600000)
    public List<RankingResponseDto> updateRanking() throws JsonProcessingException {
        Set<Long> codes = logService.getAllCodes();
        List<RankingResponseDto> rankingResponseDtos = new ArrayList<>();
        for(Long code : codes){
            List<Log> logs = logService.getTopLogsByCode(code);
            for (Log l : logs){
                RankingResponseDto rankingResponseDto = new RankingResponseDto();
                rankingResponseDto.setNicknames(l.getMember().getNickname());
                rankingResponseDto.setScores(l.getScore());
                //정령 조회 SQL 하나 더 해서
                List<Soul> souls = soulService.getSoulByMemberId(l.getMember().getId());
                List<String> soulsName = new ArrayList<>();
                for(Soul soul : souls) {
                    soulsName.add(soul.getSoulName());
                }
                rankingResponseDto.setSoulNames(soulsName);
                rankingResponseDtos.add(rankingResponseDto);
            }
        }
        return rankingResponseDtos;

    }

    public Set<ZSetOperations.TypedTuple<String>> getTopMembers(Long count) {
        return redisTemplate.opsForZSet().reverseRangeWithScores(RANKING_KEY, 0, count - 1);
    }

}
