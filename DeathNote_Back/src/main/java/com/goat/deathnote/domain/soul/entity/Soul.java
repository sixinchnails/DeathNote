package com.goat.deathnote.domain.soul.entity;

import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.member.entity.Member;
import lombok.*;

import javax.persistence.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Builder
public class Soul {

    @Id @GeneratedValue
    @Column(name = "soul_id")
    private Long id;

    @ManyToOne
    @JoinColumn(name = "member_id")
    private Member member;

    @ManyToOne
    @JoinColumn(name = "garden_id")
    private Garden garden;

    @Column(name = "soul_name", nullable = false)
    private String soulName;

    @Column(name = "soul_equip", nullable = false)
    private Long equip;

    @Column(name = "soul_critical", nullable = false)
    private Long critical;

    @Column(name = "soul_bonus", nullable = false)
    private Long bonus;

    @Column(name = "soul_combo", nullable = false)
    private Long combo;

    @Column(name = "soul_weight", nullable = false)
    private Long weight;

    @Column(name = "soul_beat", nullable = false)
    private Long beat;

    @Column(name = "soul_blue", nullable = false)
    private Long blue;

    @Column(name = "soul_mood", nullable = false)
    private Long mood;

    @Column(name = "soul_peace", nullable = false)
    private Long peace;

    @Column(name = "soul_heart", nullable = false)
    private Long heart;

    @Column(name = "soul_magna", nullable = false)
    private Long magna;

    @Column(name = "soul_body", nullable = false)
    private Long body;

    @Column(name = "soul_eyes", nullable = false)
    private Long eyes;

    @Column(name = "soul_acc", nullable = false)
    private Long acc;

    @Column(name = "soul_skill", nullable = false)
    private Long skill;

    @Column(name = "soul_revive", nullable = false)
    private Long revive;

}
