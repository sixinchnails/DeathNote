package com.goat.deathnote.domain.webview.controller;

import com.goat.deathnote.global.oauth.dto.MemberPrincipal;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class WebViewController {

    @GetMapping("/login-success")
    public String loginSuccess(Model model, @AuthenticationPrincipal MemberPrincipal principal) {
        if (principal != null) {
            // 로그인 성공 시 처리할 내용 추가 (예: 모델에 필요한 데이터 추가)
            model.addAttribute("message", "Login successful!");
            model.addAttribute("nickname", principal.getNickname());
        }
        return "login";
    }
}
