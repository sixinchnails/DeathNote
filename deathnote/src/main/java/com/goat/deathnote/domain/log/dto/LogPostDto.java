package com.goat.deathnote.domain.log.dto;

import lombok.*;

import java.time.LocalDateTime;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class LogPostDto {

    private Long memberId;

    private Long code;

    private Long score;

    private Float grade;

//    private LocalDateTime logDate;
}
