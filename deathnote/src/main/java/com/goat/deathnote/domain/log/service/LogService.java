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
import java.util.HashSet;
import java.util.List;
import java.util.Optional;
import java.util.Set;

@Service
@RequiredArgsConstructor
public class LogService {

    private final LogRepository logRepository;
    private final MemberRepository memberRepository;

    public Log saveLog(LogPostDto logPostDto) {
        Member member = memberRepository.findById(logPostDto.getMemberId()).orElseThrow();
        Log log = Log.builder()
                .member(member)
                .code(logPostDto.getCode())
                .score(logPostDto.getScore())
                .grade(logPostDto.getGrade())
                .logDate(LocalDateTime.now())
                .data(logPostDto.getData())
                .build();
        // 로그 생길때 유저 진행도보다 스테이지가 앞서면 유저 진행도 업데이트
        if (member.getProgress() < log.getCode()){
            member.setProgress(log.getCode());
        }
        logRepository.saveAndFlush(log);

        return log;
    }

    public Set<Long> getAllCodes() {
        List<Log> logs = logRepository.findAll();
        Set<Long> codes = new HashSet<>();
        for (Log l : logs){
            codes.add(l.getCode());
        }
        return codes;
    }

    public Set<Long> getAlldata() {
        List<Log> logs = logRepository.findAll();
        Set<Long> data = new HashSet<>();
        for (Log l : logs){
            data.add(l.getCode());
        }
        return data;
    }

    public List<Log> getAllLogs() {
        return logRepository.findAll();
    }

    public List<Log> getLogByNickname(String nickname) {
        System.out.println(nickname);
        return logRepository.findByMemberNickname(nickname);
    }

    // 멤버pk로 로그조회
    public List<Log> getLogByMemberId(Long id){
        return logRepository.findByMemberId(id);
    }

    public List<Log> getLogByCode(Long code) {
        return logRepository.findByCode(code);
    }

    // 특정 코드기반 로그조회
    public List<Log> getTopLogsByCode(Long code) {
        return logRepository.findByCodeOrderByScoreDesc(code);
    }

    public Optional<Log> getLogById(Long id) {
        return logRepository.findById(id);
    }
}
