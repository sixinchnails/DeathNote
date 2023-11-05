package com.goat.deathnote.domain.stage.entity;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.world.entity.World;
import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Stage {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "stage_id") // 기본 키
    private Long id;

    @OneToOne
    @JoinColumn(name = "music_id")
    private Music music;

    @OneToOne
    @JoinColumn(name = "world_id")
    private World world;

    @OneToOne
    @JoinColumn(name = "soul_id", nullable = false)
    private Soul soul;

    @ManyToOne
    @JoinColumn(name = "member_id")
    private Member member;

}