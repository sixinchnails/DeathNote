package com.goat.deathnote.domain.soul.repository;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface SoulRepository extends JpaRepository<Soul, Long> {

    List<Soul> findBySoulName(String name);

    List<Soul> findByMemberId(Long memberId);
}
