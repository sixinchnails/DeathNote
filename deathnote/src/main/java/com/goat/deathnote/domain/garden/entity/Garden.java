package com.goat.deathnote.domain.garden.entity;

import com.goat.deathnote.domain.member.entity.Member;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Builder
public class Garden {

    @Id @GeneratedValue
    @Column(name = "garden_id")
    private Long id;

    @Column(name = "garden_name")
    private String name;

    @ManyToOne
    @JoinColumn(name = "member_id")
    private Member member;


}
