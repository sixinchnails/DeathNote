package com.goat.deathnote.domain.user.entity;

import com.goat.deathnote.domain.member.entity.MemberRole;
import com.goat.deathnote.domain.member.entity.SocialProvider;
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

    @Column(name = "user_level")
    private Long level;

    @Column(name = "user_experience_value")
    private Long experienceValue; // 경험치

    @Column(name = "user_progress")
    private Long progress; // 진행도

    @Column(name = "user_name")
    private String name; //유저 이름

    @Column(name = "user_email")
    private String email; //유저 구글 이메일

    @Column(name = "user_role")
    private UserRole role; //유저 권한 (일반 유저 or 관리자)

    @Column(name = "user_provider")
    private SocialProvider provider; //공급자 (google, facebook ...)

}