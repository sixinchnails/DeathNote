package com.goat.deathnote.domain.stage.dto;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.stage.entity.Stage;
import lombok.*;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class MemberRankingDTO {
    private Member member;
    private int ranking;

}
