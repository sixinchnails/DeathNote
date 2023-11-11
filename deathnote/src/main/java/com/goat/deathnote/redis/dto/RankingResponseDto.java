package com.goat.deathnote.redis.dto;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RankingResponseDto {

    private String nickname;
    private Long score;
    private String soulName;

    public RankingResponseDto(Member member, Log log, Soul soul) {
        this.nickname = member.getNickname();
        this.score = log.getScore();
        this.soulName = soul.getSoulName();
    }
}
