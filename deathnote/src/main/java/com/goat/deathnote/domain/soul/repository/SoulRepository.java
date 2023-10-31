package com.goat.deathnote.domain.soul.repository;

import com.goat.deathnote.domain.soul.entity.Soul;
import org.springframework.data.jpa.repository.JpaRepository;

public interface SoulRepository extends JpaRepository<Soul, Long> {
}
