package com.goat.deathnote.domain.garden.entity;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Builder
public class Garden {

    @Id @GeneratedValue
    @Column(name = "garden_id")
    private Long id;

}
