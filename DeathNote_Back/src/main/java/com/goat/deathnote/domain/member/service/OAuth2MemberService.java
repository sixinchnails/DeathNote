package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.entity.MemberRole;
import com.goat.deathnote.domain.member.entity.SocialProvider;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.global.oauth.dto.MemberPrincipal;
import com.goat.deathnote.global.oauth.info.OAuth2MemberInfo;
import com.goat.deathnote.global.oauth.info.OAuth2UserInfoFactory;
import org.apache.commons.lang3.RandomStringUtils;
import org.springframework.security.oauth2.client.userinfo.DefaultOAuth2UserService;
import org.springframework.security.oauth2.client.userinfo.OAuth2UserRequest;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.stereotype.Service;

import org.springframework.security.authentication.InternalAuthenticationServiceException;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

import java.util.Optional;

@Slf4j
@Service
@RequiredArgsConstructor
public class OAuth2MemberService extends DefaultOAuth2UserService {

    private final MemberRepository memberRepository;

    @Override
    public OAuth2User loadUser(OAuth2UserRequest userRequest) throws OAuth2AuthenticationException {
        OAuth2User user = super.loadUser(userRequest); // 유저를 불러옴
        System.out.println(user);
        try {
            return this.process(userRequest, user);
        } catch (Exception ex) {
            log.error("OAuth2UserService loadUser Error {} ", ex.getMessage());
            throw new InternalAuthenticationServiceException(ex.getMessage(), ex.getCause());
        }
    }

    private OAuth2User process(OAuth2UserRequest userRequest, OAuth2User user) {
        SocialProvider socialProvider = SocialProvider.valueOf( // 소셜 프로바이더 얻어옴
                userRequest.getClientRegistration().getRegistrationId().toUpperCase());

        // 얻어온 프로바이더로 해당 소셜에 맞는 유저 정보 가져옴
        OAuth2MemberInfo memberInfo = OAuth2UserInfoFactory.getOAuth2UserInfo(socialProvider, user.getAttributes());
        // 해당 유저가 존재하는지 확인후 존재하지 않으면 회원가입
        Optional<Member> foundMember = memberRepository.findOneByEmail(memberInfo.getEmail());
        Member savedMember;
        if (foundMember.isEmpty()) {
            savedMember = createMember(memberInfo, socialProvider);
        } else {
            savedMember = foundMember.get();
        }
//        String jwtToken = JwtTokenProvider.generateToken(savedMember.getId());

        MemberPrincipal principal = MemberPrincipal.create(savedMember, user.getAttributes(), savedMember.getRole());

        return principal;
    }

    private Member createMember(OAuth2MemberInfo memberInfo, SocialProvider socialProvider) {
        Member member = Member.builder()
                .name(memberInfo.getName())
                .nickname(memberInfo.getNickname())
                .email(memberInfo.getEmail())
                .role(MemberRole.USER)
                .provider(socialProvider)
                .level(1L)
                .gold(0L)
                .progress(0L)
                .build();

        String randomNickname = generateRandomNickname();

        // 받아온 닉네임이 없으면
        if (member.getNickname() == null) {
            // 랜덤 생성 닉네임 배치
            member.setNickname(randomNickname);
        }

        memberRepository.saveAndFlush(member);

        return member;
    }

    private String generateRandomNickname() {
        return RandomStringUtils.randomAlphabetic(5);
    }

}