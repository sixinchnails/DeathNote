package com.goat.deathnote;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.PropertySource;

@SpringBootApplication
@PropertySource("classpath:custom.properties")
public class DeathnoteApplication {

	public static void main(String[] args) {
		SpringApplication.run(DeathnoteApplication.class, args);
	}

}
