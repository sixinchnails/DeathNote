package com.goat.deathnote.domain.collection.entity;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import lombok.*;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import javax.persistence.*;
import java.time.LocalDateTime;

@Entity
@Getter
@NoArgsConstructor
@AllArgsConstructor
@EntityListeners(AuditingEntityListener.class)
@Builder
public class Collection {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) // AUTO_INCREMENT
    @Column(name = "collection_id") // 기본 키
    private Long id;

    @ManyToOne
    @Column(name = "member_id")
    private Member memberId;

    @ManyToOne
    @Column(name = "soul_id")
    private Soul soulId;

    @CreatedDate
    @Column(name = "soul_collect_day")
    private LocalDateTime collectDay;

    @Builder.Default
    @Column(name = "soul_favorites")
    private boolean favorites = false;

}