package com.goat.deathnote.domain.music.controller;

import com.goat.deathnote.domain.music.dto.MusicDto;
import com.goat.deathnote.domain.music.entity.Music;
import com.goat.deathnote.domain.music.service.MusicService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpHeaders;
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

    @GetMapping("/test")
    public String testMethod() {
        return "test";
    }

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

            ResponseEntity<byte[]> response = musicService.getAudioFile(musicDto);

            HttpHeaders headers = response.getHeaders();

            String title = headers.getFirst("Title");
            model.addAttribute("Title", title);

            String duration = headers.getFirst("Duration");

            int castDuration = (int)Double.parseDouble(duration);
            String newDuration = ((int) castDuration / 60) + ":" + ((int) castDuration % 60);
            model.addAttribute("Duration", newDuration);

            byte[] audioBytes = response.getBody();
            String base64Audio = Base64.getEncoder().encodeToString(audioBytes);
            model.addAttribute("audioData", "data:audio/wav;base64," + base64Audio);

            return "audioplay";
        } catch(Exception e) {
            return "errorPage";
        }
    }

    @GetMapping("/play/audio")
    public String playAudio(Model model,
                            @RequestParam(name="acousticness") Double acousticness,
                            @RequestParam(name="instrumentalness") Double instrumentalness,
                            @RequestParam(name="energy") Double energy,
                            @RequestParam(name="valence") Double valence,
                            @RequestParam(name="liveness") Double liveness,
                            @RequestParam(name="loudness") Double loudness,
                            @RequestParam(name="tempo") Double tempo,
                            @RequestParam(name="danceability") Double danceability,
                            @RequestParam(name="speechiness") Double speechiness) {

        try{
            MusicDto musicDto = MusicDto.builder()
                    .danceability(danceability)
                    .energy(energy)
                    .tempo(tempo)
                    .acousticness(acousticness)
                    .liveness(liveness)
                    .valence(valence)
                    .loudness(loudness)
                    .instrumentalness(instrumentalness)
                    .speechiness(speechiness)
                    .build();

            ResponseEntity<byte[]> response = musicService.getAudioFile(musicDto);

            HttpHeaders headers = response.getHeaders();

            String title = headers.getFirst("Title");
            model.addAttribute("Title", title);

            String duration = headers.getFirst("Duration");

            int castDuration = (int)Double.parseDouble(duration);
            String newDuration = ((int) castDuration / 60) + ":" + ((int) castDuration % 60);
            model.addAttribute("Duration", newDuration);

            byte[] audioBytes = response.getBody();
            String base64Audio = Base64.getEncoder().encodeToString(audioBytes);
            model.addAttribute("audioData", "data:audio/wav;base64," + base64Audio);

            return "audioplay";
        } catch(Exception e) {
            return "errorPage";
        }
    }

    @PostMapping("/play/audio")
    public String playAudio(@RequestBody MusicDto musicDto, Model model) {
        try {
            ResponseEntity<byte[]> responseEntity = musicService.getAudioFile(musicDto);

            String title = responseEntity.getHeaders().getFirst("Title");
            model.addAttribute("Title", title);

            String duration = responseEntity.getHeaders().getFirst("Duration");

            int castDuration = (int)Double.parseDouble(duration);
            String newDuration = ((int) castDuration / 60) + ":" + ((int) castDuration % 60);
            model.addAttribute("Duration", newDuration);

            byte[] audioBytes = responseEntity.getBody(); // Fetch WAV file from Python server
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