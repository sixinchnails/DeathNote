package com.goat.deathnote.domain.User.repository;

import com.goat.deathnote.domain.User.dto.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UserRepository extends JpaRepository<User, Long> {
}
