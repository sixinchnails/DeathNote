package com.goat.deathnote.domain.soul.entity;

import lombok.*;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;

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

    @Column(name = "soul_state")
    private Long state;
}
