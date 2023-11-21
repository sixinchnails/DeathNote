package com.goat.deathnote.domain.log.repository;

import com.goat.deathnote.domain.log.entity.Log;
import io.lettuce.core.dynamic.annotation.Param;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.List;
import java.util.Optional;

public interface LogRepository extends JpaRepository<Log, Long> {
    Optional<Log> findByIdAndMember_Id(Long id, Long memberId);

    @Query("SELECT l FROM Log l WHERE l.member.nickname = :nickname")
    List<Log> findByMemberNickname(@Param("nickname") String nickname);

    List<Log> findByCode(Long code);

    List<Log> findByMemberId(Long id);

    @Query("SELECT l FROM Log l WHERE l.code = :code ORDER BY l.score DESC")
    List<Log> findByCodeOrderByScoreDesc(@Param("code") Long code);
}
