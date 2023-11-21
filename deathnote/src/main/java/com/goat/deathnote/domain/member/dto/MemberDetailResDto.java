package com.goat.deathnote.domain.member.dto;

import com.goat.deathnote.domain.garden.dto.GardenDetailsDto;
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
public class MemberDetailResDto {

    private Long id;

    private String nickname;

    private String token;

    private Long progress;

    private Long gold;

    private List<SoulDetailsDto> souls;

    private List<GardenDetailsDto> gardens;

    public MemberDetailResDto(Member member, List<SoulDetailsDto> souls, List<GardenDetailsDto> gardens) {
        this.id = member.getId();
        this.nickname = member.getNickname();
        this.token = member.getToken();
        this.progress = member.getProgress();
        this.gold = member.getGold();
        this.souls = souls;
        this.gardens = gardens;
    }
}
