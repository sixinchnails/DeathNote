package com.goat.deathnote.domain.log.entity;

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
    @JoinColumn(name = "stage_id")
    private Stage stage;

    @ManyToOne
    @JoinColumn(name = "soul_name")
    private Soul soul;

    @CreatedDate
    @Column(name = "log_time")
    private LocalDateTime log;
}
