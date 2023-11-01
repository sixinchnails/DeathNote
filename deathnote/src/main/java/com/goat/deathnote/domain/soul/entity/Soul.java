package com.goat.deathnote.domain.soul.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@AllArgsConstructor
@RequiredArgsConstructor
@Getter
@Setter
@Builder
@Table(name = "soul")
public class Soul {

    @Id @GeneratedValue
    @Column(name = "soul_id")
    private Long id;

    @Column(name = "soul_state")
    private Long state;
}
