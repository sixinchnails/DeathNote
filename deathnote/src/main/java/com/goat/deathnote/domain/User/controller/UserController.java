package com.goat.deathnote.domain.User.controller;

import com.goat.deathnote.domain.User.dto.User;
import com.goat.deathnote.domain.User.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;

@Controller
@RequiredArgsConstructor
public class UserController {

    private final UserRepository userRepository;

    public void userSave(@RequestBody User user){
        userRepository.save(user);
    }
}
