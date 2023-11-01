package com.goat.deathnote.global.jwt;

import io.jsonwebtoken.*;
import io.jsonwebtoken.io.Decoders;
import io.jsonwebtoken.security.Keys;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Component;

import java.security.Key;
import java.util.Date;

@Slf4j
@Component
public class JwtTokenProvider {
    private static final String AUTHORITIES_KEY = "Auth";
    private static final String BEARER_TYPE = "Bearer";
    private static final long ACCESS_TOKEN_EXPIRE_TIME = 30 * 24 * 60 * 60 * 1000L;
    private static final long REFRESH_TOKEN_EXPIRE_TIME = 30 * 24 * 60 * 60 * 1000L;
    private static final int REFRESH_TOKEN_EXPIRE_TIME_COOKIE = 365 * 24 * 60 * 60;
    private Key key;

    public JwtTokenProvider() {
        byte[] keyBytes = Decoders.BASE64.decode("adnkansdclsnacklncasnklasdckasddasasdqeqweqwe");
        this.key = Keys.hmacShaKeyFor(keyBytes);
    }

    public static int getRefreshTokenExpireTimeCookie() {
        return REFRESH_TOKEN_EXPIRE_TIME_COOKIE;
    }

    public Token createToken(String email, String role) {
        long now = new Date().getTime();

        String accessToken = Jwts
                .builder()
                .setSubject(email)
                .setIssuedAt(new Date())
                .claim(AUTHORITIES_KEY, role)
                .signWith(key, SignatureAlgorithm.HS256)
                .setExpiration(new Date(now + ACCESS_TOKEN_EXPIRE_TIME))
                .compact();

        String refreshToken = Jwts
                .builder()
                .setSubject(email)
                .setIssuedAt(new Date())
                .claim(AUTHORITIES_KEY, role)
                .signWith(key, SignatureAlgorithm.HS256)
                .setExpiration(new Date(now + REFRESH_TOKEN_EXPIRE_TIME))
                .compact();

        return Token
                .builder()
                .grantType(BEARER_TYPE)
                .accessToken(accessToken)
                .refreshToken(refreshToken)
                .expireTime(REFRESH_TOKEN_EXPIRE_TIME)
                .build();
    }

    public Claims parseClaims(String accessToken) {
        try {
            return Jwts
                    .parser()
                    .setSigningKey(key)
                    .parseClaimsJws(accessToken)
                    .getBody();
        } catch (Exception e) {
            log.error("JWT parsing failed: {}", e.getMessage());
            throw new RuntimeException("JWT parsing failed");
        }
    }

    public String validateToken(String token) {
        try {
            Jwts
                    .parser()
                    .setSigningKey(key)
                    .parseClaimsJws(token);
            return "valid";
        } catch (Exception e) { // 에러처리
            log.error("JWT validation failed: {}", e.getMessage());
            if (e instanceof ExpiredJwtException) { // 만료됨
                return "isExpired";
            } else if (e instanceof MalformedJwtException) { // 손상됨
                return "invalid";
            } else if (e instanceof UnsupportedJwtException) { // 지원 안함
                return "isUnsupported";
            } else if (e instanceof IllegalArgumentException) {
                return "isIllegal";
            } else if (e instanceof SignatureException) {
                return "isSignature";
            } else {
                return "invalid";
            }
        }
    }

    public boolean getIsExpired(String accessToken) {
        Date expiration = Jwts
                .parser()
                .setSigningKey(key)
                .parseClaimsJws(accessToken)
                .getBody()
                .getExpiration();

        long now = new Date().getTime();
        return expiration.getTime() - now <= 0;
    }

    public String getMemberUUID(String accessToken) {
        String jwtToken = accessToken.split("Bearer ")[1];
        Claims claims = this.parseClaims(jwtToken);
        return claims.getSubject();
    }
}