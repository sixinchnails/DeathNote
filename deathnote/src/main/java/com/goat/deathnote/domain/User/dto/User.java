package com.goat.deathnote.domain.User.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.RequiredArgsConstructor;

import javax.persistence.*;

@Entity
@Getter
@AllArgsConstructor
@RequiredArgsConstructor
@Builder
public class User {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "user_id")
    private Long id;

    @Column(name = "user_name", unique = true, length = 8) // 유일 하고, 최대 길이 8
    private String name;

    @Column(name = "user_level")
    private Long level;

    @Column(name = "user_experience_value")
    private Long experienceValue;

    @Column(name = "user_progress")
    private Long progress;
}