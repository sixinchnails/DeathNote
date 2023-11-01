package com.goat.deathnote.domain.collection.service;

import com.goat.deathnote.domain.collection.entity.Collection;
import com.goat.deathnote.domain.collection.repository.CollectionRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class CollectionService {
    
    private final CollectionRepository collectionRepository;

    public CollectionService(CollectionRepository collectionRepository) {
        this.collectionRepository = collectionRepository;
    }

    public Collection saveMusic (Collection music){
            return collectionRepository.save(music);
        }

        public List<Collection> getAllMusics () {
            return collectionRepository.findAll();
        }

        public Optional<Collection> getMusicById (Long id){
            return collectionRepository.findById(id);
        }

//        public void deleteMusic (Long id){
//            musicRepository.deleteById(id);
//        }
    }
