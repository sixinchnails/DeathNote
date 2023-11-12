package com.goat.deathnote.redis.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ReponseRankingDto implements Serializable {

    private Long code;
    private Long score;
    private String nickname;
    private List<String> soulNames;

}
