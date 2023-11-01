package com.goat.deathnote.domain.music.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@Builder
@Table(name = "music")
public class Music {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "music_id") // 기본 키
    private Long id;

}