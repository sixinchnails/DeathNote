package com.goat.deathnote.domain.soul.service;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.domain.soul.dto.SoulPostDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class SoulService {

    private final SoulRepository soulRepository;
    private final MemberRepository memberRepository;

    public Soul saveSoul(SoulPostDto soulPostDto) {
        Member member = memberRepository.findById(soulPostDto.getMemberId()).orElseThrow();
        Soul soul = Soul.builder()
                .member(member)
                .soulName(soulPostDto.getSoulName())
                .equip(soulPostDto.getEquip())
                .critical(soulPostDto.getCritical())
                .bonus(soulPostDto.getBonus())
                .combo(soulPostDto.getCombo())
                .weight(soulPostDto.getWeight())
                .beat(soulPostDto.getBeat())
                .blue(soulPostDto.getBlue())
                .mood(soulPostDto.getMood())
                .peace(soulPostDto.getPeace())
                .heart(soulPostDto.getHeart())
                .magna(soulPostDto.getMagna())
                .body(soulPostDto.getBody())
                .eyes(soulPostDto.getEyes())
                .acc(soulPostDto.getAcc())
                .skill(soulPostDto.getSkill())
                .revive(soulPostDto.getRevive())
                .build();

        return soulRepository.save(soul);
    }

    public Optional<Soul> getSoulById(Long id){
        return soulRepository.findById(id);
    }
    public List<Soul> getAllSouls() {
        return soulRepository.findAll();
    }

    public List<Soul> getSoulByName(String name) {
        return soulRepository.findBySoulName(name);
    }

    public void deleteSoul (Long id){
        soulRepository.deleteById(id);
    }

    public List<Soul> getSoulByMemberId(Long id) {
        return soulRepository.findByMemberId(id);
    }
}
