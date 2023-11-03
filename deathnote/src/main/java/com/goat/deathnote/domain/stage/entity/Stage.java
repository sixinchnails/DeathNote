package com.goat.deathnote.domain.stage.entity;

import com.goat.deathnote.domain.soul.entity.Soul;
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


    @Column(name = "music_id")
    private Long musicId;

    @Column(name = "world_id")
    private Long worldId;

    @OneToOne
    @JoinColumn(name = "soul_id", nullable = false)
    private Soul soul;

    @Column(name = "user_id")
    private Long userId;

}