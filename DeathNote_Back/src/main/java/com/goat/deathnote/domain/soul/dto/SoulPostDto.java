package com.goat.deathnote.domain.soul.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;


@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class SoulPostDto {

    private Long memberId;

    private Long gardenId;

    private String soulName;

    private Long equip;

    private Long critical;

    private Long bonus;

    private Long combo;

    private Long weight;

    private Long beat;

    private Long blue;

    private Long mood;

    private Long peace;

    private Long heart;

    private Long magna;

    private Long body;

    private Long eyes;

    private Long acc;

    private Long skill;

    private Long revive;

}
