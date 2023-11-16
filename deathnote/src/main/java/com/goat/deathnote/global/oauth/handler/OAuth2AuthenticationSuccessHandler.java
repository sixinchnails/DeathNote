package com.goat.deathnote.global.oauth.handler;

import java.io.IOException;
import java.util.Collection;
import java.util.Optional;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.entity.MemberRole;
import com.goat.deathnote.domain.member.entity.SocialProvider;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.global.jwt.JwtTokenProvider;
import com.goat.deathnote.global.jwt.Token;
import com.goat.deathnote.global.oauth.info.OAuth2MemberInfo;
import com.goat.deathnote.global.oauth.info.OAuth2UserInfoFactory;
import com.goat.deathnote.global.oauth.util.CookieUtil;
import com.goat.deathnote.global.oauth.repository.OAuth2AuthorizationRequestBasedOnCookieRepository;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.oauth2.client.authentication.OAuth2AuthenticationToken;
import org.springframework.security.oauth2.core.oidc.user.OidcUser;
import org.springframework.security.web.authentication.SimpleUrlAuthenticationSuccessHandler;
import org.springframework.stereotype.Component;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@Slf4j
@Component
@RequiredArgsConstructor
public class OAuth2AuthenticationSuccessHandler extends SimpleUrlAuthenticationSuccessHandler {

	private final OAuth2AuthorizationRequestBasedOnCookieRepository authorizationRequestRepository;
	private final JwtTokenProvider jwtTokenProvider;
	private final MemberRepository memberRepository;
	private String redirectUrl = "http://localhost:8080/login-success"; // 여기로 보내짐

	@Override
	public void onAuthenticationSuccess(HttpServletRequest request, HttpServletResponse response,
										Authentication authentication) throws IOException {
		String nickname = determineTargetUrl(request, response, authentication);
		if (response.isCommitted()) {
			log.error("Response has already been committed. Unable to redirect");
			return;
		}

		request.setAttribute("nickname", nickname);
		clearAuthenticationAttributes(request, response);
		getRedirectStrategy().sendRedirect(request, response, redirectUrl);
	}

	protected String determineTargetUrl(HttpServletRequest request, HttpServletResponse response,
		Authentication authentication) {
		// 쿠키로 redirectUri 받아옴
		Optional<String> redirectUri = CookieUtil.getCookie(request,
				OAuth2AuthorizationRequestBasedOnCookieRepository.REDIRECT_URI_PARAM_COOKIE_NAME)
			.map(Cookie::getValue);

		String targetUrl = redirectUri.orElse(getDefaultTargetUrl());

		// 아래는 전부 유저 정보로 토큰을 만드는 과정
		OAuth2AuthenticationToken authToken = (OAuth2AuthenticationToken)authentication;
		SocialProvider socialProvider = SocialProvider.valueOf(
			authToken.getAuthorizedClientRegistrationId().toUpperCase());

		OidcUser user = ((OidcUser)authentication.getPrincipal());
		OAuth2MemberInfo memberInfo = OAuth2UserInfoFactory.getOAuth2UserInfo(socialProvider, user.getAttributes());
		Collection<? extends GrantedAuthority> authorities = ((OidcUser)authentication.getPrincipal()).getAuthorities();

		String nickname = memberInfo.getNickname(); // 멤버의 닉네임을 받아옴
		MemberRole memberRole;
		if (hasAuthority(authorities, MemberRole.USER.name())) {
			memberRole = MemberRole.USER;
		} else return null;

		Member member = memberRepository.findByNickname(nickname).orElseThrow(); // 받아온 이메일로 멤버를 찾음
		Token tokenInfo = jwtTokenProvider.createToken(member.getId(),
			memberRole.name());

		log.info("successHanlder : " + member.getEmail() + member.getNickname());

		CookieUtil.deleteCookie(request, response, OAuth2AuthorizationRequestBasedOnCookieRepository.REFRESH_TOKEN);
		CookieUtil.addCookie(response, OAuth2AuthorizationRequestBasedOnCookieRepository.REFRESH_TOKEN,
			tokenInfo.getRefreshToken(),
			JwtTokenProvider.getRefreshTokenExpireTimeCookie());
//		 아까 받아온 redirectUri로 토큰을 보내줌
		return nickname;
	}

	protected void clearAuthenticationAttributes(HttpServletRequest request, HttpServletResponse response) {
		super.clearAuthenticationAttributes(request);
		authorizationRequestRepository.removeAuthorizationRequestCookies(request, response);
	}

	private boolean hasAuthority(Collection<? extends GrantedAuthority> authorities, String authority) {
		if (authorities == null) {
			return false;
		}
		for (GrantedAuthority grantedAuthority : authorities) {
			if (authority.equals(grantedAuthority.getAuthority())) {
				return true;
			}
		}
		return false;
	}
}