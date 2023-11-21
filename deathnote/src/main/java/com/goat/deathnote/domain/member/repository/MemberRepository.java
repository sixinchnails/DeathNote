package com.goat.deathnote.domain.member.repository;

import com.goat.deathnote.domain.member.entity.Member;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.Optional;

public interface MemberRepository extends JpaRepository<Member, Long> {

    Optional<Member> findByEmail(String email);
    Member findByName(String name);
    Optional<Member> findByNickname(String nickname);

    @Query("select m from Member m where m.email = :email")
    Optional<Member> findOneByEmail(String email);
}