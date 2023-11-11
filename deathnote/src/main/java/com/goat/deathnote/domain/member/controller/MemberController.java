package com.goat.deathnote.domain.member.controller;

import com.goat.deathnote.domain.member.dto.LogInRequest;
import com.goat.deathnote.domain.member.dto.MemberWithSoulResDto;
import com.goat.deathnote.domain.member.dto.SignUpRequest;
import com.goat.deathnote.domain.member.dto.UpdateMemberDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.service.MemberNotFoundException;
import com.goat.deathnote.domain.member.service.MemberService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.util.List;

@Controller
@RequiredArgsConstructor
@RequestMapping("/members")
public class MemberController {

    private final MemberService memberService;

    // 로그인하면 이동하는페이지
    @GetMapping("/login")
    public String login(HttpServletRequest req, Model model) {
        // 예제로, id를 "12345"라고 설정합니다
        String email = (String)req.getAttribute("email");
        model.addAttribute("id", email);
        return "login";
    }

    // 회원가입 (이메일, 닉네임)
    @PostMapping("/signup")
    public ResponseEntity<?> signUp(@RequestBody SignUpRequest signUpRequest) {
        return ResponseEntity.ok(memberService.signUp(signUpRequest.getEmail(), signUpRequest.getNickname()));
    }

    // 로그인
    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LogInRequest logInRequest) {
        return ResponseEntity.ok(memberService.login(logInRequest.getEmail()));
    }

    // 유저 전체조회
    @GetMapping
    public ResponseEntity<List<Member>> getAllMembers() {
        return ResponseEntity.ok(memberService.getAllMembers());
    }

    // 유저 조회 가지고있는 정령까지 싹다
    @GetMapping("/{id}")
    public ResponseEntity<MemberWithSoulResDto> getDetailMember(@PathVariable Long id) {
       return ResponseEntity.ok(memberService.getMemberById(id));
    }

    // 닉네임 변경
    @PatchMapping("/{id}/nickname")
    public ResponseEntity<Member> updateNickname(@PathVariable Long id, @RequestBody UpdateMemberDto newNickname) {
        return ResponseEntity.ok(memberService.updateNicknameById(id, newNickname));
    }


    // 유저 정보 업데이트
    @PatchMapping("/{memberId}")
    public ResponseEntity<?> updateMember(@PathVariable Long memberId, @RequestBody UpdateMemberDto updateMemberDto) {
        try {
            memberService.updateMember(memberId, updateMemberDto);
            return ResponseEntity.ok("업데이트 성공");
        } catch (MemberNotFoundException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body("멤버를 찾을 수 없음");
        }
    }
//    @DeleteMapping("/{memberId}")
//    public ResponseEntity<Void> deleteMember(@PathVariable Long memberId) {
//        if (memberService.getMemberById(memberId).isPresent()) {
//            memberService.deleteMember(memberId);
//            return new ResponseEntity<>(HttpStatus.NO_CONTENT);
//        } else {
//            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
//        }
//    }
}