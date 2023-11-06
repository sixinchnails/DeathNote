package com.goat.deathnote.domain.stage.service;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.stage.dto.MemberRankingDTO;
import com.goat.deathnote.domain.stage.entity.Stage;
import com.goat.deathnote.domain.stage.repository.StageRepository;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Optional;

@Service
public class StageService {

    private final StageRepository stageRepository;

    public StageService(StageRepository stageRepository) {
        this.stageRepository = stageRepository;
    }

    public Stage saveStage(Stage stage) {
        return stageRepository.save(stage);
    }

    public List<Stage> getAllStages() {
        return stageRepository.findAll();
    }

    public Optional<Stage> getStageById(Long id) {
        return stageRepository.findById(id);
    }

//    public List<MemberRankingDTO> getRankingForStage(Stage stage) {
//        List<Member> members = stage.getMembers();
//        List<MemberRankingDTO> rankingList = new ArrayList<>();
//
//        // 멤버들의 랭킹을 계산하고 DTO에 저장
//        for (Member member : members) {
//            int ranking = calculateMemberRanking(member);
//            MemberRankingDTO dto = new MemberRankingDTO(member, ranking);
//            rankingList.add(dto);
//        }
//
//        // 랭킹순으로 정렬
//        rankingList.sort(Comparator.comparing(MemberRankingDTO::getRanking));
//
//        return rankingList;
//    }
//
//    private int calculateMemberRanking(Member member) {
//        // 멤버의 랭킹을 계산하는 로직을 구현
//        // 예: 멤버의 점수를 기반으로 랭킹을 계산
//        return member.getScore(); // 이 예시에서는 멤버의 점수를 랭킹으로 사용
//    }

}
