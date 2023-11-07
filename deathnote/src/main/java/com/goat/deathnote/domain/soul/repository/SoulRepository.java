package com.goat.deathnote.domain.soul.repository;

import com.goat.deathnote.domain.soul.entity.Soul;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface SoulRepository extends JpaRepository<Soul, Long> {
    Optional<Soul> findBySoulName(String name);

}
