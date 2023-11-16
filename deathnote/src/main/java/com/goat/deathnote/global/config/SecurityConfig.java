package com.goat.deathnote.global.config;

import com.goat.deathnote.domain.member.entity.MemberRole;
import com.goat.deathnote.domain.member.service.OAuth2MemberService;
import com.goat.deathnote.global.oauth.handler.OAuth2AuthenticationFailureHandler;
import com.goat.deathnote.global.oauth.handler.OAuth2AuthenticationSuccessHandler;
import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.SecurityFilterChain;

@RequiredArgsConstructor
@EnableWebSecurity
public class SecurityConfig {

    private final OAuth2MemberService oAuth2MemberService;
    private final OAuth2AuthenticationSuccessHandler successHandler;
    private final OAuth2AuthenticationFailureHandler failureHandler;

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        http.csrf().disable();
        http.authorizeRequests()
                .anyRequest().permitAll() // 모두 접근 가능하도록 설정
                .and()
//                .logout()
//                .logoutSuccessUrl("/")
//                .and()
                .oauth2Login()
                .userInfoEndpoint()
                .userService(oAuth2MemberService)
                .and()
                .successHandler(successHandler)
                .failureHandler(failureHandler);
        return http.build();
    }

}
