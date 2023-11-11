package com.goat.deathnote.domain.log.service;

import com.goat.deathnote.domain.log.dto.LogPostDto;
import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class logService {

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
                .build();
        return logRepository.save(log);
    }

    public List<Log> getAllLogs() {
        return logRepository.findAll();
    }

    public Optional<Log> getLogById(Long id) {
        return logRepository.findById(id);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
