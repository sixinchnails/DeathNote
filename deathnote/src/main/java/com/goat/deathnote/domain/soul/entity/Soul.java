package com.goat.deathnote.domain.soul.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@AllArgsConstructor
@RequiredArgsConstructor
@Getter
@Setter
@Builder
public class Soul {

    @Id @GeneratedValue
    @Column(name = "soul_id")
    private Long id;

    @Column(name = "soul_name")
    private String soulName;

    @Column(name = "soul_state")
    private boolean state;

    @Column(name = "soul_passive")
    private String passive;

    @Column(name = "soul_active1")
    private String active1;

    @Column(name = "soul_active2")
    private String active2;

    @Column(name = "soul_active3")
    private String active3;

}
