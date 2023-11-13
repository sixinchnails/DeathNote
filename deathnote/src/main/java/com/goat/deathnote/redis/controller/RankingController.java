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

//    @GetMapping("/{count}/{code}")
//    public ResponseEntity<List<RankingResponseDto>> getTopMembers(@PathVariable Long count, @PathVariable Long code) {
//        Set<ZSetOperations.TypedTuple<String>> topMembers = rankingService.getTopMembers(count);
//        List<RankingResponseDto> responseDtoList = new ArrayList<>();
//        List<Member> members = new ArrayList<>();
//        for (ZSetOperations.TypedTuple<String> memberTuple : topMembers) {
//            String memberId = memberTuple.getValue();
//            Double score = memberTuple.getScore();
//
//            // TODO: memberId를 사용하여 Member, Log, Soul 정보를 가져와서 RankingResponseDto를 생성
//            Member member = memberService.getMemberById(Long.valueOf(memberId));
//            List<Log> logs = logService.getLogByCode(code);
//            List<ScoreListDto> scoreListDtos = new ArrayList<>();
//            for (Log l : logs){
//                ScoreListDto scoreListDto = new ScoreListDto(l);
//                scoreListDtos.add(scoreListDto);
//            }
//            List<Soul> souls = soulRepository.findByMemberId(member.getId());
//            List<SoulListDto> soulListDtos = new ArrayList<>();
//            for (Soul s : souls) {
//                SoulListDto soulListDto = new SoulListDto(s);
//                soulListDtos.add(soulListDto);
//            }
//            members.add(memberRepository.findById(member.getId()).orElseThrow());
//            List<NicnknameListDto> nicnknameListDtos = new ArrayList<>();
//            for (Member m : members){
//                NicnknameListDto nicnknameListDto = new NicnknameListDto(m);
//                nicnknameListDtos.add(nicnknameListDto);
//            }
//
//            RankingResponseDto rankingResponseDto = new RankingResponseDto(scoreListDtos,nicnknameListDtos,soulListDtos);
//            responseDtoList.add(rankingResponseDto);
//        }
//
//        System.out.println(responseDtoList);
//        return ResponseEntity.ok(responseDtoList);
//    }

    @GetMapping
    public ResponseEntity<List<ReponseRankingDto>> getRankingByCode() {
        try {
            return ResponseEntity.ok(rankingService.createRankingResponse());
        } catch (Exception e){
            return ResponseEntity.badRequest().build();
        }
    }

    @GetMapping("/{code}")
    public List<ReponseRankingDto> getRankingResponse(@PathVariable String code) {
        return rankingService.getRankingResponse(code);
    }

}
