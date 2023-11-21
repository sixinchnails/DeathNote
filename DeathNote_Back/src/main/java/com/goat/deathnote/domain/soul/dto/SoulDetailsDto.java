package com.goat.deathnote.domain.soul.dto;

import com.goat.deathnote.domain.soul.entity.Soul;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class SoulDetailsDto {

    private Long soulId;
    private String name;
    private Long equip;
    private List<Long> parameters;
    private List<Long> customizes;
    private List<Long> emotions;
    private Long revive;

    public SoulDetailsDto(Soul soul) {
        this.soulId = soul.getId();
        this.name = soul.getSoulName();
        this.equip = soul.getEquip();
        this.parameters = List.of(soul.getCritical(), soul.getBonus(), soul.getCombo(), soul.getWeight());
        this.customizes = List.of(soul.getBody(), soul.getEyes(), soul.getAcc(), soul.getSkill());
        this.emotions = List.of(soul.getBeat(), soul.getBlue(), soul.getMood(), soul.getPeace(), soul.getHeart(), soul.getMagna());
        this.revive = soul.getRevive();
    }

}
