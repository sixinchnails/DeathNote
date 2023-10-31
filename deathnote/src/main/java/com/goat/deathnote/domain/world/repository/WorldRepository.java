package com.goat.deathnote.domain.world.repository;

import com.goat.deathnote.domain.world.entity.World;
import org.springframework.data.jpa.repository.JpaRepository;

public interface WorldRepository extends JpaRepository<World, Long> {
}
