//package com.goat.deathnote.redis.controller;
//
//import com.goat.deathnote.domain.log.entity.Log;
//import com.goat.deathnote.domain.log.service.LogService;
//import com.goat.deathnote.domain.member.entity.Member;
//import com.goat.deathnote.domain.member.service.MemberService;
//import com.goat.deathnote.redis.dto.RankingResponseDto;
//import com.goat.deathnote.redis.service.RankingService;
//import lombok.RequiredArgsConstructor;
//import org.springframework.data.redis.core.ZSetOperations;
//import org.springframework.http.ResponseEntity;
//import org.springframework.web.bind.annotation.GetMapping;
//import org.springframework.web.bind.annotation.PathVariable;
//import org.springframework.web.bind.annotation.RequestMapping;
//import org.springframework.web.bind.annotation.RestController;
//
//import java.util.ArrayList;
//import java.util.List;
//import java.util.Set;
//
//@RestController
//@RequiredArgsConstructor
//@RequestMapping("/rankings")
//public class RankingController {
//
//    private final RankingService rankingService;
//    private final MemberService memberService;
//    private final LogService logService;
//
//    @GetMapping("/{code}/{count}")
//    public ResponseEntity<List<RankingResponseDto>> getTopMembers(@PathVariable Long count, @PathVariable Long code) {
//        Set<ZSetOperations.TypedTuple<String>> topMembers = rankingService.getTopMembers(count);
//
//        List<RankingResponseDto> responseDtoList = new ArrayList<>();
//        for (ZSetOperations.TypedTuple<String> memberTuple : topMembers) {
//            String memberId = memberTuple.getValue();
//            Double score = memberTuple.getScore();
//
//            Member member = memberService.getMerberById(Long.parseLong(memberId));
//            List<Log> logs = logService.getLogByCode(code);
//
//            if (member != null) {
//                RankingResponseDto rankingResponseDto = new RankingResponseDto(member.getNickname(), score, log.getMember().get);
//                responseDtoList.add(rankingResponseDto);
//            }
//        }
//
//        return ResponseEntity.ok(responseDtoList);
//    }
//}
