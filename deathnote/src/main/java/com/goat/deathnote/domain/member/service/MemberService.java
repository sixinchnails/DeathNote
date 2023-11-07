package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.member.dto.MemberDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.persistence.EntityNotFoundException;
import java.util.List;
import java.util.Optional;

@Service
public class MemberService {

    private final MemberRepository memberRepository;

    @Autowired
    public MemberService(MemberRepository memberRepository) {
        this.memberRepository = memberRepository;
    }

    public List<Member> getAllMembers() {
        // 모든 멤버 목록을 조회하는 로직을 추가하세요.
        return memberRepository.findAll();
    }

    public Optional<Member> getMemberById(Long memberId) {
        // ID를 사용하여 특정 멤버 조회 로직을 추가하세요.
        return memberRepository.findById(memberId);
    }

    public void deleteMember(Long memberId) {
        // ID를 사용하여 멤버를 삭제하는 로직을 추가하세요.
        memberRepository.deleteById(memberId);
    }

    public Member updateNicknameByEmail(String email, MemberDto newNickname) {
        Member member = memberRepository.findByEmail(email);

        if (member != null) {
            member.setNickName(newNickname.getNickName());
            return memberRepository.save(member);
        }

        // 유저를 찾을 수 없을 때 예외처리 (여기에서는 null을 반환하거나 예외를 던질 수 있음)
        return null;
    }
}
