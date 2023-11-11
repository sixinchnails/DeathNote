package com.goat.deathnote.domain.log.repository;

import com.goat.deathnote.domain.log.entity.Log;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface LogRepository extends JpaRepository<Log, Long> {
    Optional<Log> findByIdAndMember_Id(Long id, Long memberId);
}
