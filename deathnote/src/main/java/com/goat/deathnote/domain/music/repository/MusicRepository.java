package com.goat.deathnote.domain.music.repository;

import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.world.entity.World;
import org.springframework.data.jpa.repository.JpaRepository;

public interface MusicRepository extends JpaRepository<Music, Long> {
}
