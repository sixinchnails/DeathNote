package com.goat.deathnote.domain.member.repository;

import com.goat.deathnote.domain.member.entity.Member;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface MemberRepository extends JpaRepository<Member, Long> {
    public Optional<Member> findByName(String name);

    Member findByEmail(String email);
}