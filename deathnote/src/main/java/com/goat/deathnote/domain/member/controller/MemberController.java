package com.goat.deathnote.domain.member.controller;

import com.goat.deathnote.domain.member.dto.MemberWithSoulResDto;
import com.goat.deathnote.domain.member.dto.UpdateMemberDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.service.MemberNotFoundException;
import com.goat.deathnote.domain.member.service.MemberService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/members")
public class MemberController {

    private final MemberService memberService;

    @GetMapping("/login/{id}")
    public String login(Model model, @PathVariable String id) {
        // 예제로, id를 "12345"라고 설정합니다
        model.addAttribute("id", id);
        return "login";
    }

    @GetMapping
    public ResponseEntity<List<Member>> getAllMembers() {
        return ResponseEntity.ok(memberService.getAllMembers());
    }

    @GetMapping("/{id}")
    public ResponseEntity<MemberWithSoulResDto> getDetailMember(@PathVariable Long id) {
       return ResponseEntity.ok(memberService.getMemberById(id));
    }

    @PutMapping("/{email}/nickname")
    public ResponseEntity<Member> updateNickname(@PathVariable String email, @RequestBody UpdateMemberDto newNickname) {
        return ResponseEntity.ok(memberService.updateNicknameByEmail(email, newNickname));
    }


    @PatchMapping("/{memberId}")
    public ResponseEntity<String> updateMember(@PathVariable Long memberId, @RequestBody UpdateMemberDto updateMemberDto) {
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