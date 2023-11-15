package com.goat.deathnote.domain.member.entity;

import lombok.*;

import javax.persistence.*;

@Entity
@Getter
@Setter
@NoArgsConstructor // 기본생성자
@AllArgsConstructor
@Builder
public class Member {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "member_id", nullable = false)
    private Long id; //기본키

    @Column(name = "member_name", nullable = false)
    private String name; //유저 이름

    @Column(name = "member_email", nullable = false)
    private String email; //유저 구글 이메일

    @Enumerated(EnumType.STRING)
    @Column(name = "member_role", nullable = false)
    private MemberRole role; //유저 권한 (일반 유저, 관리자)

    @Enumerated(EnumType.STRING)
    @Column(name = "member_provider", nullable = false)
    private SocialProvider provider; //공급자 (google, facebook ...)

    @Column(name = "member_nickname", nullable = false, length = 5)
    private String nickname;

    @Column(name = "member_level", nullable = false)
    private Long level;

    @Column(name = "member_gold", nullable = false)
    private Long gold;

    @Column(name = "member_progress", nullable = false)
    private Long progress; // 진행도

    @Column(name = "member_token", columnDefinition = "TEXT")
    private String token;

}