package com.goat.deathnote.domain.user.controller;

import com.goat.deathnote.domain.user.dto.UserDto;
import com.goat.deathnote.domain.user.entity.User;
import com.goat.deathnote.domain.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@Controller
@RequiredArgsConstructor
public class UserController {

    private final UserRepository userRepository;
//    private final UserService userService;

    @PostMapping("/users/save")
    public void userSave(@RequestBody UserDto userDto){
        User user  = User.builder()
                .name(userDto.getName())
                .level(userDto.getLevel())
                .experienceValue(userDto.getExperienceValue())
                .progress(userDto.getProgress())
                .build();
        userRepository.save(user);
    }
}
