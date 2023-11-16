package com.goat.deathnote.domain.music.controller;

import com.goat.deathnote.domain.music.dto.MusicDto;
import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.service.MusicService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Base64;
import java.util.List;
import java.util.Optional;

@Controller
@RequiredArgsConstructor
@RequestMapping("/musics")
public class MusicController {

    private final MusicService musicService;

    // 지금 안씀
    @GetMapping("/test")
    public String testMethod() {
        return "test";
    }

    // 얘도 테스트
    @GetMapping("/test/audio")
    public String testAudio(Model model) {
        try{
            MusicDto musicDto = MusicDto.builder()
                    .danceability(0.5)
                    .energy(0.5)
                    .tempo(120)
                    .acousticness(0.5)
                    .liveness(0.5)
                    .valence(0.5)
                    .loudness(30)
                    .liveness(0.5)
                    .instrumentalness(0.5)
                    .build();

            byte[] audioBytes = musicService.getAudioFile(musicDto);

            String base64Audio = Base64.getEncoder().encodeToString(audioBytes);
            model.addAttribute("audioData", "data:audio/wav;base64," + base64Audio);
            return "audioplay";
        } catch(Exception e) {
            return "errorPage";
        }
    }

    // 실제 사용 api
    @PostMapping("/play/audio")
    public String playAudio(@RequestBody MusicDto musicDto, Model model) {
        try {
            byte[] audioBytes = musicService.getAudioFile(musicDto); // Fetch WAV file from Python server
            String base64Audio = Base64.getEncoder().encodeToString(audioBytes);
            model.addAttribute("audioData", "data:audio/wav;base64," + base64Audio);
            return "audioplay";
        } catch (Exception e) {
            // Handle exceptions
            return "errorPage";
        }
    }

    @PostMapping
    public ResponseEntity<Music> createMusic(@RequestBody Music music) {
        return ResponseEntity.ok(musicService.saveMusic(music));
    }

    @GetMapping
    public ResponseEntity<List<Music>> getAllMusics() {
        return ResponseEntity.ok(musicService.getAllMusics());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Optional<Music>> getMusicById(@PathVariable Long id) {
        return ResponseEntity.ok(musicService.getMusicById(id));
    }
}