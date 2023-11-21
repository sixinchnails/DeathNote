package com.goat.deathnote.global.oauth.info;

import java.util.Map;

public class KakaoOAuth2MemberInfo extends OAuth2MemberInfo {

    public KakaoOAuth2MemberInfo(Map<String, Object> attributes) {
        super(attributes);
    }

    @Override
    public String getName() {
        return (String)attributes.get("name");
    }

    @Override
    public String getEmail() {
        Map<String, Object> account = (Map<String, Object>)attributes.get("kakao_account");
        return (String)account.get("email");
    }

    @Override
    public String getNickname() {
        Map<String, Object> properties = (Map<String, Object>)attributes.get("properties");

        if (properties == null) {
            return null;
        }

        return (String)properties.get("nickname");
    }

}
