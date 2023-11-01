package com.goat.deathnote.domain.member.entity;

import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

@Entity
@Getter
@NoArgsConstructor
public class Member {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id; //기본키
    private String name; //유저 이름
    private String email; //유저 구글 이메일
    private MemberRole role; //유저 권한 (일반 유저, 관리지ㅏ)
    private SocialProvider provider; //공급자 (google, facebook ...)

    @Builder
    public Member(String name, String email, MemberRole role, SocialProvider provider) {
        this.name = name;
        this.email = email;
        this.role = role;
        this.provider = provider;
    }
}