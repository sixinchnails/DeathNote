package com.goat.deathnote.domain.log.entity;

import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.stage.entity.Stage;
import lombok.*;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import javax.persistence.*;
import java.time.LocalDateTime;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Builder
@EntityListeners(AuditingEntityListener.class)
public class Log {

    @Id @GeneratedValue
    @Column(name = "log_id")
    private Long id;

    @ManyToOne
    @JoinColumn(name = "member_id", nullable = false)
    private Member member;

    @Column(name = "log_score")
    private Long score;

    @Column(name = "log_story")
    private Long story;

    @Column(name = "log_grade")
    private Float grade;

    @CreatedDate
    @Column(name = "log_time", nullable = false)
    private LocalDateTime log;
}
