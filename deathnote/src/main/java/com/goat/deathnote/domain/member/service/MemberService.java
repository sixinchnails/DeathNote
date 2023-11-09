package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.member.dto.MemberWithSoulResDto;
import com.goat.deathnote.domain.member.dto.UpdateMemberNicknameDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.domain.soul.dto.SoulDetailsDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class MemberService {

    private final MemberRepository memberRepository;
    private final SoulRepository soulRepository;

    public List<Member> getAllMembers() {
        return memberRepository.findAll();
    }

    public MemberWithSoulResDto getMemberById(Long memberId) {
        Member member = memberRepository.findById(memberId).orElseThrow();

        List<Soul> souls = soulRepository.findByMemberId(member.getId());
        List<SoulDetailsDto> soulDetails = new ArrayList<>();
        for (Soul s : souls) {
            SoulDetailsDto soulDetailsDto = new SoulDetailsDto(s);
            soulDetails.add(soulDetailsDto);
        }

        MemberWithSoulResDto memberWithSoulResDto = new MemberWithSoulResDto(member, soulDetails);
        return memberWithSoulResDto;
    }

    public void deleteMember(Long memberId) {
        memberRepository.deleteById(memberId);
    }

    public Member updateNicknameByEmail(String email, UpdateMemberNicknameDto newNickname) {
        Member member = memberRepository.findByEmail(email);

        if (member != null) {
            member.setNickname(newNickname.getNickName());
            return memberRepository.save(member);
        }

        // 유저를 찾을 수 없을 때 예외처리 (여기에서는 null을 반환하거나 예외를 던질 수 있음)
        return null;
    }

}
