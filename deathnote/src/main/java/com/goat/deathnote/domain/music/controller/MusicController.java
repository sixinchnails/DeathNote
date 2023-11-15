package com.goat.deathnote.domain.music.controller;

import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.service.MusicService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.util.Base64;
import java.util.List;
import java.util.Optional;

@Controller
@RequiredArgsConstructor
@RequestMapping("/musics")
public class MusicController {

    private final MusicService musicService;

    @GetMapping("/play-audio")
    public String playAudio(HttpServletRequest req, Model model) {
        try {
            byte[] audioBytes = musicService.getAudioFile(); // Fetch WAV file from Python server
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
