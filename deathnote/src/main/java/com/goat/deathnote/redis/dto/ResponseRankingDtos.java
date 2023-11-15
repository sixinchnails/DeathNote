package com.goat.deathnote.redis.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ResponseRankingDtos {

    private List<ResponseRankingDto> responseRankingDtoList;

}
