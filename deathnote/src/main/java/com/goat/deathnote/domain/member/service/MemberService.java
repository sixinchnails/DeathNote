package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.member.dto.MemberWithSoulResDto;
import com.goat.deathnote.domain.member.dto.UpdateMemberDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.domain.soul.dto.SoulDetailsDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

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

    public Member updateNicknameById(Long id, UpdateMemberDto updateMemberDto) {
        Member member = memberRepository.findById(id).orElseThrow();

        if (member != null) {
            member.setNickname(updateMemberDto.getNickname());
            return memberRepository.save(member);
        }

        // 유저를 찾을 수 없을 때 예외처리 (여기에서는 null을 반환하거나 예외를 던질 수 있음)
        return null;
    }

    @Transactional
    public void updateMember(Long memberId, UpdateMemberDto updateMemberDto) {
        Member member = memberRepository.findById(memberId)
                .orElseThrow(() -> new MemberNotFoundException("멤버를 찾지 못했습니다."));

        // 업데이트할 필드들만 업데이트
        if (updateMemberDto.getLevel() != null) {
            member.setLevel(updateMemberDto.getLevel());
        }

        if (updateMemberDto.getGold() != null) {
            member.setGold(updateMemberDto.getGold());
        }

        if (updateMemberDto.getProgress() != null) {
            member.setProgress(updateMemberDto.getProgress());
        }

        if (updateMemberDto.getNickname() != null) {
            member.setNickname(updateMemberDto.getNickname());
        }

        // 저장
        memberRepository.save(member);
    }

}
