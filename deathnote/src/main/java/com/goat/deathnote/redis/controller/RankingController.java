package com.goat.deathnote.redis.controller;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.service.LogService;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.service.MemberService;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.service.SoulService;
import com.goat.deathnote.redis.dto.RankingResponseDto;
import com.goat.deathnote.redis.service.RankingService;
import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

@RestController
@RequiredArgsConstructor
@RequestMapping("/rankings")
public class RankingController {

    private final RankingService rankingService;
    private final MemberService memberService;
    private final LogService logService;
    private final SoulService soulService;


    @GetMapping("/{code}/{count}")
    public ResponseEntity<List<RankingResponseDto>> getTopMembers(@PathVariable Long code, @PathVariable Long count) {
        Set<ZSetOperations.TypedTuple<String>> topMembers = rankingService.getTopMembers(count);

        List<RankingResponseDto> responseDtoList = new ArrayList<>();
        for (ZSetOperations.TypedTuple<String> memberTuple : topMembers) {
            String memberId = memberTuple.getValue();
            Double score = memberTuple.getScore();

            Member member = memberService.getMemberById(Long.parseLong(memberId));
            List<Log> logs = logService.getLogByCode(code);
            Soul soul = soulService.getSoulById(member.getId()).orElseThrow();

            if (member != null && !logs.isEmpty()) {
                Log log = logs.get(0); // 단순성을 위해 첫 번째 로그를 가져오도록 가정
                RankingResponseDto rankingResponseDto = new RankingResponseDto(member, log, soul);
                responseDtoList.add(rankingResponseDto);
            }
        }

        return ResponseEntity.ok(responseDtoList);
    }
}
