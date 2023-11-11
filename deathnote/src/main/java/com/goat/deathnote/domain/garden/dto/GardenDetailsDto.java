package com.goat.deathnote.domain.garden.dto;

import com.goat.deathnote.domain.garden.entity.Garden;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class GardenDetailsDto {

    private Long gardenId;
    private Long type;

    public GardenDetailsDto(Garden garden) {
        this.gardenId = garden.getId();
        this.type = garden.getType();
    }
}
