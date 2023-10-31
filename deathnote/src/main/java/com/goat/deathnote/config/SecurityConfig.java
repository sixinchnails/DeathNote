package com.goat.deathnote.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;

@Configuration
@EnableWebSecurity
public class SecurityConfig extends WebSecurityConfigurerAdapter {

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http.csrf().disable()
                .authorizeRequests()
                .antMatchers("/security-login/info").authenticated()
                .antMatchers("/security-login/admin/**").hasAuthority(UserRole.ADMIN.name())
                .anyRequest().permitAll()
                .and()
                .formLogin()
                .usernameParameter("loginId")
                .passwordParameter("password")
                .loginPage("/security-login/login")
                .defaultSuccessUrl("/security-login")
                .failureUrl("/security-login/login")
                .and()
                .logout()
                .logoutUrl("/security-login/logout")
                .invalidateHttpSession(true).deleteCookies("JSESSIONID");
    }
//    authorizeRequest() : 인증, 인가가 필요한 URL 지정
//    antMatchers(URL)
//    authenticated() : 해당 URL에 진입하기 위해서 Authentication(인증, 로그인)이 필요함
//    hasAuthority() : 해당 URL에 진입하기 위해서 Authorization(인가, ex)권한이 ADMIN인 유저만 진입 가능)이 필요함
//    URL에 ** 사용 : ** 위치에 어떤 값이 들어와도 적용시킴
//    antMatchers(HttpMethod.POST, URL) : 이런 식으로 특정 HttpMethod만 검사 할 수도 있음
//    anyRequest() : 그 외의 모든 URL
//    permitAll() : Authentication, Authorization 필요 없이 통과
//    formLogin() : Form Login 방식 적용
//    usernameParameter() : 로그인할 때 사용되는 ID를 적어줌
//    이 예제에서는 loginId로 로그인하기 때문에 따로 적어줘야 함
//    만약 userName으로 로그인을 한다면 적어주지 않아도 됨
//    passwordParameter() : 로그인할 때 사용되는 password를 적어줌
//    이 예제에서는 password로 로그인하기 때문에 따로 적어주지 않아도 됨
//    loginPage() : 로그인 페이지 URL
//    defaultSuccessURL() : 로그인 성공 시 이동할 URL
//    failureURL() : 로그인 실패 시 이동할 URL
//    logout() : 로그아웃에 대한 정보
}