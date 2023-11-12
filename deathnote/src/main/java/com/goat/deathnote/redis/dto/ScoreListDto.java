package com.goat.deathnote.redis.dto;

import com.goat.deathnote.domain.log.entity.Log;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;


@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ScoreListDto {
    private Long score;

    public ScoreListDto(Log log) {
        this.score = log.getScore();
    }
}
