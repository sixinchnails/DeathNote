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

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "garden_id", nullable = false)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "member_id", nullable = false)
    private Member member;

    @Column(name = "garden_type", nullable = false)
    private Long type;

}
