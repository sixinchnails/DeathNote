package com.goat.deathnote.domain.user.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@AllArgsConstructor
@RequiredArgsConstructor
@Builder
public class User {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "user_id") // 기본 키
    private Long id;

    @Column(name = "user_nickname", unique = true, length = 8) // 유일 하고, 최대 길이 8
    private String nickName;

    @Column(name = "user_level")
    private Long level;

    @Column(name = "user_experience_value")
    private Long experienceValue;

    @Column(name = "progress")
    private Long progress;

<<<<<<< HEAD
    @Column(name = "provider")
    private String provider;

    @Column(name = "provider_id")
    private String providerId;
=======
    @Column(name = "open_id")
    private String openId; // 구글로 받아온 사용자 고유id, 중복 방지로 비교, 사용할듯

>>>>>>> 530eb286187bb8f59dd4c288f340883cbbf4ce04
}