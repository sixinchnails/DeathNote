package com.goat.deathnote.domain.log.dto;

import com.goat.deathnote.domain.member.entity.Member;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class LogGetDto {

    private Member member;

    private Long code;

    private Long score;

    private Float grade;

    private LocalDateTime logDate;
}
