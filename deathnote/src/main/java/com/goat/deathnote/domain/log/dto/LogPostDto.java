package com.goat.deathnote.domain.log.dto;

import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class LogPostDto {

    private String nickname;

    private Long code;

    private Long score;

    private Float grade;

    private String data;

}
