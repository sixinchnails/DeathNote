package com.goat.deathnote.domain.garden.service;

import com.goat.deathnote.domain.garden.dto.GardenPostDto;
import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.garden.repository.GardenRepository;
import com.goat.deathnote.domain.log.entity.Log;
import com.goat.deathnote.domain.log.repository.LogRepository;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class GardenService {

    private final GardenRepository gardenRepository;
    private final MemberRepository memberRepository;

    public Garden saveGarden(GardenPostDto gardenPostDto) {
        Member member = memberRepository.findById(gardenPostDto.getMemberId()).orElseThrow();
        Garden garden = Garden.builder()
                .member(member)
                .name(gardenPostDto.getName())
                .build();
        return gardenRepository.save(garden);
    }

    public List<Garden> getAllGardens() {
        return gardenRepository.findAll();
    }

    public Optional<Garden> getGardenById(Long id) {
        return gardenRepository.findById(id);
    }

//        public void deleteSoul (Long id){
//            soulRepository.deleteById(id);
//        }
}
