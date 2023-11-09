package com.goat.deathnote.domain.garden.repository;

import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.log.entity.Log;
import org.springframework.data.jpa.repository.JpaRepository;

public interface GardenRepository extends JpaRepository<Garden, Long> {
}
