package com.goat.deathnote.global.oauth.info;

import java.util.Map;

public class GoogleOAuth2MemberInfo extends OAuth2MemberInfo {

	public GoogleOAuth2MemberInfo(Map<String, Object> attributes) {
		super(attributes);
	}

	@Override
	public String getName() {
		return (String)attributes.get("name");
	}

	@Override
	public String getEmail() {
		return (String)attributes.get("email");
	}

	@Override
	public String getNickname() {
		return (String)attributes.get("nickname");
	}
}