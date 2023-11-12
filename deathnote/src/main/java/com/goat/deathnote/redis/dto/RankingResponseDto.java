package com.goat.deathnote.redis.dto;

import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.redis.core.ZSetOperations;

import java.io.Serializable;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RankingResponseDto implements Serializable {

    private Long scores;
    private String nicknames;
    private List<String> soulNames;

}
