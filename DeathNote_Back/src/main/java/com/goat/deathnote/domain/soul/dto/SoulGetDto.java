package com.goat.deathnote.domain.soul.dto;

import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.member.entity.Member;
import lombok.*;


@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
@Builder
public class SoulGetDto {

    private Long id;

    private Member member;

    private Garden garden;

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


