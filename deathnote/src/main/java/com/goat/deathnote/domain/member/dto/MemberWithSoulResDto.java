package com.goat.deathnote.domain.member.dto;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.dto.SoulDetailsDto;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.util.List;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class MemberWithSoulResDto {

    private String nickname;

    private String token;

    private Long progress;

    private List<SoulDetailsDto> souls;

    private Long gold;

    public MemberWithSoulResDto(Member member, List<SoulDetailsDto> souls) {
        this.nickname = member.getNickname();
        this.token = member.getToken();
        this.progress = member.getProgress();
        this.gold = member.getGold();
        this.souls = souls;
    }
}
