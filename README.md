# DeathNote
![로고](/image/로고.gif)
## 프로젝트 개요
RPG와 리듬게임을 융합한 캐주얼 모바일 게임

## 프로젝트 기간
2023.10.10 ~ 2023.11.17 (6주)

## 프로젝트 내용



### 소셜로그인
<img width="700" height="370" src="/image/카카오.gif"/>
<br>
- 카카오 소셜로그인을 기본으로 함

### 스토리
<img width="700" height="370" src="/image/스토리.png"/>
<br>
- 악마가 악보(데스노트)를 떨어뜨리면서 시작되는 스토리
- 마지막 노래 플레이시 엔딩 등장 및 작곡 해금

### 리듬게임 시스템
<img width="700" height="370" src="/image/맵.gif"/>
<br>
- Redis에서 랭킹을 최신화하여, 곡별로 랭킹 경쟁 가능
- offset을 사용자마다 조절하여 정확한 싱크를 맞출 수 있음
<br>
<img width="700" height="370" src="/image/리듬게임.gif"/>
<br>
- 장착한 세션에 따라 나오는 정령과 배경의 톤이 변경
- 장착한 세션의 확률에 따라 스킬이 발동하여 점수가 높아짐


### 정원 시스템
<img width="700" height="370" src="/image/정원변경.gif"/>
<br>
- 리듬게임을 통해 얻은 '영감'으로 정령 혹은 정원스킨을 구매
<br>
<img width="700" height="370" src="/image/정령구매.gif"/>
<br>
- 정령은 바디 7가지, 눈 3가지에 각 파트의 색깔 11가지로 총 2541가지 외형조합이 가능
- 정령은 총 30가지의 스킬 중 3개를 가질 수 있음
<br>
<img width="700" height="370" src="/image/이동.gif"/>
<br>
- 정령을 누를 경우 카메라가 계속 추적


### 정령 육성 시스템
<img width="700" height="370" src="/image/이름변경.gif"/>
<br>
- 정령의 이름을 자신의 맘대로 변경 가능
<br>
<img width="700" height="370" src="/image/환생.gif"/>
<br>
- 환생을 통해서 자신의 정령의 능력치를 향상시킬 수 있으며, 외형이나 스킬 변경 가능
<img width="700" height="370" src="/image/세션.gif"/>
<br>
- 세션을 통해 리듬게임에 데려갈 6마리의 정령을 정할 수 있음


### 미니게임
<img width="700" height="370" src="/image/미니게임.gif"/>
<br>
- 정원에서 정령을 3초이상 누를경우 간단한 미니게임 진행
- 정령을 클릭하여 음표장애물을 피하는 게임


### 작곡(노래추천)
<img width="700" height="370" src="/image/작곡.gif"/>
<br>
- 엔딩 이후에 작곡 컨텐츠 해금
- 플레이어의 플레이데이터에 따라 여러가지 패러미터를 추출하여 맞는 노래 추천(개발중)

## 프로젝트 인원

### 유니티(4명)

이세훈 (게임 시스템 구현, 팀장)
- Unity의 SoundSystem에 맞춘 Rhythmgame Scene 구현
- 정령 육성 시스템 제작, level scaling
- 전체 Scene 총괄 및 연결
- Unity Webview 및 HTTPS 통신 구현
- 캐릭터 및 이펙트 Sprite set 제작

김라현 (UI, UX구현)
- 스토리에 맞는 Opening, Ending Scene 구현
- UI / UX에 맞는 Asset 제작
- 생성형 AI를 이용한 Illust 추출
- 게임 전체 아트 컨셉 제작 및 총괄
- 데이터 선별 및 ERD 제작

강 현 (부가 시스템 구현)
- 정령을 키울 수 있는 Garden Scene 구현
- 타이밍에 맞춰 점프하는 Minigame Scene 구현
- 음원 박자 분석 및 노트 데이터 추출
- 음원 싱크 맞추기 및 노트 배치
- Unity particle, object 애니메이션 구현

유민국 (게임 데이터 제작)
- 맵을 이동하는 Worldmap Scene, Stage Scene 구현
- 음원 박자 분석 및 노트데이터 추출
- 음원 싱크 맞추기 및 노트 배치
- 게임 씬 간 이동 및 부가 UI 제작
- UCC 구현

### 인프라(1명)
최재용 (빅데이터, 머신러닝, 인프라 구축)
- AWS, Docker, Jenkins를 활용한 인프라 구축
- 가비아 도메인 등록, HTTPS 기능, NginX 적용
- Librosa를 활용한 음악 파형 분석
- 다중 출력 모델을 활용한 노래 특성 예측 기능 구현
- 신경망을 활용한 노래 인기도 예측 기능 구현
- Flask REST API 서버로 작곡 추천 API 구현
- AI 작곡 플랫폼 ‘AIVA’로 노래 50개 작곡

### 백엔드(1명)
이경원 (소셜 로그인, DB 구축)
- Spring REST API 서버 구축
- Spring Security를 사용한 카카오 OAUTH2 소셜로그인 구축
- ERD 제작 및 DB 구현 및 관리
- Unity Webview와 연동되는 Thymeleaf 페이지 구현


## 사용 스택
  <img src="https://img.shields.io/badge/java-007396?style=for-the-badge&logo=java&logoColor=white">
  <img src="https://img.shields.io/badge/springboot-6DB33F?style=for-the-badge&logo=springboot&logoColor=white">  
  <img src="https://img.shields.io/badge/springsecurity-6DB33F?style=for-the-badge&logo=springsecurity&logoColor=white">
  <img src="https://img.shields.io/badge/thymeleaf-005F0F?style=for-the-badge&logo=thymeleaf&logoColor=white">
  <img src="https://img.shields.io/badge/python-3776AB?style=for-the-badge&logo=python&logoColor=white">
  <img src="https://img.shields.io/badge/flask-000000?style=for-the-badge&logo=flask&logoColor=white">
  <img src="https://img.shields.io/badge/unity-000000?style=for-the-badge&logo=unity&logoColor=white">
