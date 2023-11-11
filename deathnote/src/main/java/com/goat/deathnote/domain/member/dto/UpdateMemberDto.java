package com.goat.deathnote.domain.member.dto;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class UpdateMemberDto {

    private Long level;
    private Long gold;
    private Long progress;
    private String nickname;

}
