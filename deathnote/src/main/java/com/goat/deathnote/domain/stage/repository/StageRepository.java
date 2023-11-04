package com.goat.deathnote.domain.stage.repository;

import com.goat.deathnote.domain.stage.entity.Stage;
import org.springframework.data.jpa.repository.JpaRepository;

public interface StageRepository extends JpaRepository<Stage, Long> {
}
