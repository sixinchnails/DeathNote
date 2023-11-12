package com.goat.deathnote.redis.controller;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import com.goat.deathnote.domain.log.service.LogService;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.domain.member.service.MemberService;
import com.goat.deathnote.domain.soul.dto.SoulDetailsDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import com.goat.deathnote.redis.dto.NicnknameListDto;
import com.goat.deathnote.redis.dto.RankingResponseDto;
import com.goat.deathnote.redis.dto.ScoreListDto;
import com.goat.deathnote.redis.dto.SoulListDto;
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
@RequestMapping("/rankings")
@RequiredArgsConstructor
public class RankingController {

    private final RankingService rankingService;
    private final MemberService memberService;
    private final LogService logService;
    private final LogRepository logRepository;
    private final SoulRepository soulRepository;
    private final MemberRepository memberRepository;

    @GetMapping("/{count}/{code}")
    public ResponseEntity<List<RankingResponseDto>> getTopMembers(@PathVariable Long count, @PathVariable Long code) {
        Set<ZSetOperations.TypedTuple<String>> topMembers = rankingService.getTopMembers(count);
        List<RankingResponseDto> responseDtoList = new ArrayList<>();
        List<Member> members = new ArrayList<>();
        for (ZSetOperations.TypedTuple<String> memberTuple : topMembers) {
            String memberId = memberTuple.getValue();
            Double score = memberTuple.getScore();

            // TODO: memberId를 사용하여 Member, Log, Soul 정보를 가져와서 RankingResponseDto를 생성
            Member member = memberService.getMemberById(Long.valueOf(memberId));
            List<Log> logs = logService.getLogByCode(code);
            List<ScoreListDto> scoreListDtos = new ArrayList<>();
            for (Log l : logs){
                ScoreListDto scoreListDto = new ScoreListDto(l);
                scoreListDtos.add(scoreListDto);
            }
            List<Soul> souls = soulRepository.findByMemberId(member.getId());
            List<SoulListDto> soulListDtos = new ArrayList<>();
            for (Soul s : souls) {
                SoulListDto soulListDto = new SoulListDto(s);
                soulListDtos.add(soulListDto);
            }
            members.add(memberRepository.findById(member.getId()).orElseThrow());
            List<NicnknameListDto> nicnknameListDtos = new ArrayList<>();
            for (Member m : members){
                NicnknameListDto nicnknameListDto = new NicnknameListDto(m);
                nicnknameListDtos.add(nicnknameListDto);
            }

            RankingResponseDto rankingResponseDto = new RankingResponseDto(scoreListDtos,nicnknameListDtos,soulListDtos);
            responseDtoList.add(rankingResponseDto);
        }

        System.out.println(responseDtoList);
        return ResponseEntity.ok(responseDtoList);
    }
}
