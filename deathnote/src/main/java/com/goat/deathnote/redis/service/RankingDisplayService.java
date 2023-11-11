package com.goat.deathnote.redis.service;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;

import java.util.Set;

@Service
@RequiredArgsConstructor
public class RankingDisplayService {

    private final RankingService rankingService;
    private final MemberRepository memberRepository;

    public void displayTopMembers(Long count) {
        Set<ZSetOperations.TypedTuple<String>> topMembers = rankingService.getTopMembers(count);

        for (ZSetOperations.TypedTuple<String> memberTuple : topMembers) {
            String memberId = memberTuple.getValue();
            Double score = memberTuple.getScore();

            Member member = memberRepository.findById(Long.parseLong(memberId)).orElse(null);

            if (member != null) {
                System.out.println("Member: " + member.getNickname() + ", Score: " + score);
            }
        }
    }
}
