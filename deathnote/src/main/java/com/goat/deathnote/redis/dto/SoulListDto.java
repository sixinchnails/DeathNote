package com.goat.deathnote.redis.dto;

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
public class SoulListDto {

    private String soulName;

    public SoulListDto(Soul soul) {
        this.soulName = soul.getSoulName();
    }
}
