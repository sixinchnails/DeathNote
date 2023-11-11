package com.goat.deathnote.domain.garden.dto;

import com.goat.deathnote.domain.member.entity.Member;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;


@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class GardenGetDto {

    private Member member;

    private String name;

}
