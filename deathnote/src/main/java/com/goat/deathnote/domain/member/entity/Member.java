package com.goat.deathnote.domain.member.entity;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Entity
@Getter
@NoArgsConstructor // 기본생성자
public class Member {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "member_id")
    private Long id; //기본키

    @Column(name = "member_name")
    private String name; //유저 이름

    @Column(name = "member_email")
    private String email; //유저 구글 이메일

    @Enumerated(EnumType.STRING)
    @Column(name = "member_role")
    private MemberRole role; //유저 권한 (일반 유저, 관리자)

    @Enumerated(EnumType.STRING)
    @Column(name = "member_provider")
    private SocialProvider provider; //공급자 (google, facebook ...)

    @Column(name = "member_nickname")
    private String nickName;

    @Column(name = "member_level")
    private Long level;

    @Column(name = "member_experience_value")
    private Long experienceValue; // 경험치

    @Column(name = "member_progress")
    private Long progress; // 진행도

    @Builder
    public Member(Long id, String name, String email, MemberRole role, SocialProvider provider, String nickName, Long level, Long experienceValue, Long progress) {
        this.id = id;
        this.name = name;
        this.email = email;
        this.role = role;
        this.provider = provider;
        this.nickName = nickName;
        this.level = level;
        this.experienceValue = experienceValue;
        this.progress = progress;
    }
}