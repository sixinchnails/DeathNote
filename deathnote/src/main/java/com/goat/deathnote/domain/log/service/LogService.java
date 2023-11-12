package com.goat.deathnote.domain.log.service;

import com.goat.deathnote.domain.log.dto.LogPostDto;
import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.redis.service.RankingService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
@RequiredArgsConstructor
public class LogService {

    private final LogRepository logRepository;
    private final MemberRepository memberRepository;
    private final RankingService rankingService;

    public Log saveLog(LogPostDto logPostDto) {
        Member member = memberRepository.findById(logPostDto.getMemberId()).orElseThrow();
        Log log = Log.builder()
                .member(member)
                .code(logPostDto.getCode())
                .score(logPostDto.getScore())
                .grade(logPostDto.getGrade())
                .logDate(LocalDateTime.now())
                .build();
        if (member.getProgress() < log.getCode()){
            member.setProgress(log.getCode());
        }
        logRepository.save(log);

        try{
            rankingService.updateRanking(String.valueOf(log.getCode()), logPostDto.getScore());
        } catch (Exception e){
            e.printStackTrace();
        }

        return log;
    }

    public List<Log> getAllLogs() {
        return logRepository.findAll();
    }

    public List<Log> getLogByNickname(String nickname) {
        System.out.println(nickname);
        return logRepository.findByMemberNickname(nickname);
    }

    public List<Log> getLogByMemberId(Long id){
        return logRepository.findByMemberId(id);
    }
    public List<Log> getLogByCode(Long code) {
        return logRepository.findByCode(code);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
