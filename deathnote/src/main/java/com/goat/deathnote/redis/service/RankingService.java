package com.goat.deathnote.redis.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.LogService;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import com.goat.deathnote.global.config.RedisRepositoryConfig;
import com.goat.deathnote.redis.dto.ReponseRankingDto;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.RedisCallback;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

@Service
@RequiredArgsConstructor
public class RankingService {

    private final String RANKING_KEY = "ranking";
    private final LogService logService;
    private final SoulService soulService;
    private final RedisTemplate<String, Object> redisTemplate;

    @Autowired
    private ObjectMapper objectMapper; // Jackson ObjectMapper

//    @Scheduled(fixedRate = 3600000)
    public List<ReponseRankingDto> createRankingResponse(){
        Set<Long> codes = logService.getAllCodes();
        List<ReponseRankingDto> reponseRankingDtos = new ArrayList<>();

        flushDb(); // 레디스 초기화

        for(Long code : codes){
            List<Log> logs = logService.getTopLogsByCode(code);
            int cnt = 0;

            for (Log l : logs){
                ReponseRankingDto reponseRankingDto = new ReponseRankingDto();
                reponseRankingDto.setCode(l.getCode());
                reponseRankingDto.setNickname(l.getMember().getNickname());
                reponseRankingDto.setScore(l.getScore());
                //정령 조회 SQL 하나 더 해서
                List<Soul> souls = soulService.getSoulByMemberId(l.getMember().getId());
                List<String> soulsName = new ArrayList<>();
                for(Soul soul : souls) {
                    soulsName.add(soul.getSoulName());
                }
                reponseRankingDto.setSoulNames(soulsName);
                reponseRankingDtos.add(reponseRankingDto);

                if (++cnt == 20) break;
            }
            try {
                redisTemplate.opsForValue().set(String.valueOf(code), reponseRankingDtos);
            }catch(Exception e) {
                e.printStackTrace();
            }
            reponseRankingDtos.clear();
        }
        return reponseRankingDtos;
    }

    public List<ReponseRankingDto> getRankingResponse(String key) {
        return (List<ReponseRankingDto>) redisTemplate.opsForValue().get(key);
    }
    public void flushDb() {
        redisTemplate.execute((RedisCallback<Object>) connection -> {
            connection.flushDb();
            return null;
        });
    }
}
