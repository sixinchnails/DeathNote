package com.goat.deathnote.domain.log.dto;

import lombok.Getter;

@Getter
public class SoulDto {
    private Long state;

    public SoulDto(Long state) {
        this.state = state;
    }
}
