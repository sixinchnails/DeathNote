package com.goat.deathnote.domain.member.controller;

import com.goat.deathnote.domain.member.dto.MemberWithSoulResDto;
import com.goat.deathnote.domain.member.dto.UpdateMemberNicknameDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.service.MemberService;
import com.goat.deathnote.domain.soul.service.SoulService;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequiredArgsConstructor
@RequestMapping("/members")
public class MemberController {

    private final MemberService memberService;

    @Autowired
    public MemberController(MemberService memberService, SoulService soulService) {
        this.memberService = memberService;
    }

    @GetMapping
    public ResponseEntity<List<Member>> getAllMembers() {
        return ResponseEntity.ok(memberService.getAllMembers());
    }

    @GetMapping("/{memberId}")
    public ResponseEntity<MemberWithSoulResDto> getDetailMember(@PathVariable Long memberId) {
       return ResponseEntity.ok(memberService.getMemberById(memberId));
    }


    @PutMapping("/{email}/nickname")
    public Member updateNickname(@PathVariable String email, @RequestBody UpdateMemberNicknameDto newNickname) {
        return memberService.updateNicknameByEmail(email, newNickname);
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