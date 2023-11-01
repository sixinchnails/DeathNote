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

    @Column(name = "user_nickname")
    private String nickName;

    private String name;

    @Column(name = "user_level")
    private Long level;

    @Column(name = "user_experience_value")
    private Long experienceValue;

    @Column(name = "progress")
    private Long progress;

    @Column(name = "provider")
    private String provider;

    @Column(name = "provider_id")
    private String providerId;

    private UserRole role;

//    @Column(name = "open_id")
//    private String openId; // 구글로 받아온 사용자 고유id, 중복 방지로 비교, 사용할듯

}