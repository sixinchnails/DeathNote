package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class MemberService {

    private final MemberRepository memberRepository;

    @Autowired
    public MemberService(MemberRepository memberRepository) {
        this.memberRepository = memberRepository;
    }

    public Member createMember(Member member) {
        // 여기에 새로운 멤버 생성 및 저장 로직을 추가하세요.
        return memberRepository.save(member);
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

    public Member updateMember(Member updatedMember) {
        // 멤버 정보 업데이트 로직을 추가하세요.
        return memberRepository.save(updatedMember);
    }
}
