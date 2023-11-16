package com.goat.deathnote.global.oauth.info;

import java.util.Map;

import com.goat.deathnote.domain.member.entity.SocialProvider;

public class OAuth2UserInfoFactory {

    public static OAuth2MemberInfo getOAuth2UserInfo(SocialProvider socialProvider, Map<String, Object> attributes) {
        switch (socialProvider) {
            case GOOGLE:
                return new GoogleOAuth2MemberInfo(attributes);
            case KAKAO:
                return new KakaoOAuth2MemberInfo(attributes);
            // 다른 소셜 프로바이더에 대한 처리 추가 가능
            default:
                throw new IllegalArgumentException("Unsupported provider: " + socialProvider);
        }
    }
}
