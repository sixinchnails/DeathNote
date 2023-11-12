package com.goat.deathnote.redis.dto;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.redis.core.ZSetOperations;

import java.io.Serializable;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
public class RankingResponseDto implements Serializable {

    private List<ScoreListDto> scores;
    private List<NicnknameListDto> nicknames;
    private List<SoulListDto> soulNames;

    public RankingResponseDto(List<ScoreListDto> scores, List<NicnknameListDto> nicknames, List<SoulListDto> souls) {
        this.scores = scores;
        this.nicknames = nicknames;
        this.soulNames = souls;
    }

//    public static RankingResponseDto fromTuple(ZSetOperations.TypedTuple<String> memberTuple, Member member, Log log, Soul soul) {
//        return RankingResponseDto.builder()
//                .nickname(member.getNickname())
//                .score(memberTuple.getScore().longValue())
//                .soulName(soul.getSoulName())
//                .build();
//    }
}
