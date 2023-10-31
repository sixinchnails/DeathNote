package com.goat.deathnote.domain.user.dto;

import lombok.Getter;

@Getter
public class UserDto {
    private String name;
    private Long level;
    private Long experienceValue;
    private Long progress;

    public UserDto(String name, Long level, Long experienceValue, Long progress) {
        this.name = name;
        this.level = level;
        this.experienceValue = experienceValue;
        this.progress = progress;
    }
}
