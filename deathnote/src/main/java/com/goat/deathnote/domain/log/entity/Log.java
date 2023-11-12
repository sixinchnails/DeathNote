package com.goat.deathnote.domain.log.entity;

import com.goat.deathnote.domain.member.entity.Member;
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
    @Column(name = "log_id", nullable = false)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "member_id", nullable = false)
    private Member member;

    @Column(name = "log_code", nullable = false)
    private Long code;

    @Column(name = "log_score", nullable = false)
    private Long score;

    @Column(name = "log_grade", nullable = false)
    private Float grade;

    @Column(name = "log_data")
    private String data;

    @CreatedDate
    @Column(name = "log_date", nullable = false)
    private LocalDateTime logDate;
}
