package com.goat.deathnote.global.oauth.info;

import java.util.Map;

import com.goat.deathnote.domain.member.entity.SocialProvider;

public class OAuth2UserInfoFactory {

	public static OAuth2MemberInfo getOAuth2UserInfo(SocialProvider socialProvider, Map<String, Object> attributes) {
		return new GoogleOAuth2MemberInfo(attributes);
	}
}