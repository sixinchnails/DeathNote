package com.goat.deathnote.domain.collection.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@Builder
public class Collection {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "collection_id") // 기본 키
    private Long id;

    @Column(name = "user_id")
    private Long userId;

}