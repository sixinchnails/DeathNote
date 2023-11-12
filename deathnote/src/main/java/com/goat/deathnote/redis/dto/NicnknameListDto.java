package com.goat.deathnote.redis.dto;

import com.goat.deathnote.domain.member.entity.Member;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class NicnknameListDto {

    private String nickname;

    public NicnknameListDto(Member member) {
        this.nickname = member.getNickname();
    }
}
