package com.goat.deathnote.global.oauth.info;

import java.util.Map;

public abstract class OAuth2MemberInfo {

	protected Map<String, Object> attributes;

	public OAuth2MemberInfo(Map<String, Object> attributes) {
		this.attributes = attributes;
	}

	public Map<String, Object> getAttributes() {
		return attributes;
	}

	public abstract String getName();

	public abstract String getEmail();

	public abstract String getNickname();

}