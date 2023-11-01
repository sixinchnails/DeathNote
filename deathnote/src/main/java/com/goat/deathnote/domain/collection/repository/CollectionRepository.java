package com.goat.deathnote.domain.collection.repository;

import com.goat.deathnote.domain.collection.entity.Collection;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CollectionRepository extends JpaRepository<Collection, Long> {
}
