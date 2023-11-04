package com.goat.deathnote.domain.log.repository;

import com.goat.deathnote.domain.log.entity.Log;
import org.springframework.data.jpa.repository.JpaRepository;

public interface SoulRepository extends JpaRepository<Log, Long> {
}
