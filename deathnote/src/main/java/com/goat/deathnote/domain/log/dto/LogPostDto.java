package com.goat.deathnote.domain.log.dto;

import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;

@Getter
@Setter
@Builder
public class LogPostDto {

    private Long memberId;

    private Long code;

    private Long score;

    private Float grade;

    private LocalDateTime logDate;
}
