package com.goat.deathnote.global.jwt;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
@AllArgsConstructor
public class Token {
	private String grantType;
	private String accessToken;
	private String refreshToken;
	private Long expireTime;
}