# DeathNote
<img src="/image/로고.gif" alt="로고" width="550" height="275"/>
## 프로젝트 개요
모바일 육성 시뮬레이션 리듬게임

## 프로젝트 기간
2023.10.10 ~ 2023.11.17 (6주)

## 프로젝트 내용



### 소셜로그인
<img width="350" height="185" src="/image/카카오.gif"/>
- 카카오 소셜로그인 활용

### 스토리
<img width="350" height="185" src="/image/대화.gif"/>
- 악마가 악보(데스노트)를 떨어뜨리면서 시작되는 스토리
<img width="350" height="185" src="/image/엔딩.gif"/>
- 최종 노래 플레이 시 작곡 해금

### 리듬게임 시스템
<img width="350" height="185" src="/image/맵.gif"/>
- Redis 및 Spring Scheduler 기반 랭킹 시스템 적용
- offset을 활용한 박자 싱크 조정
<img width="350" height="185" src="/image/리듬게임.gif"/>
- 장착한 세션에 따라 나오는 정령과 배경의 톤이 변경
- 장착한 세션의 확률에 따라 스킬이 발동하여 점수가 높아짐


### 정원 시스템
<img width="350" height="185" src="/image/정원변경.gif"/>
- 리듬게임을 통해 얻은 '영감'으로 정령 혹은 정원스킨을 구매
<img width="350" height="185" src="/image/정령구매.gif"/>
- 정령은 바디 7가지, 눈 3가지에 각 파트의 색깔 11가지로 총 2541가지 외형 조합이 가능
- 정령은 30가지 스킬 중 3개씩 보유
<img width="350" height="185" src="/image/이동.gif"/>
- 정령을 누를 경우 카메라가 계속 추적


### 정령 육성 시스템
<img width="350" height="185" src="/image/이름변경.gif"/>
- 정령 이름을 자유롭게 변경 가능
<img width="350" height="185" src="/image/환생.gif"/>
- 육성으로 정령의 능력치 향상, 외형 및 스킬 변경 가능
<img width="350" height="185" src="/image/세션.gif"/>
- 세션을 통해 라운드마다 6마리의 정령과 플레이


### 미니게임
<img width="350" height="185" src="/image/미니게임.gif"/>
- 정원에서 정령을 3초 이상 누르면 간단한 미니게임 진행
- 정령을 클릭하여 음표장애물을 피하는 게임


### 작곡(노래추천)
<img width="350" height="185" src="/image/작곡.gif"/>
- 엔딩 이후에 작곡 컨텐츠 해금
<br>
- 사용자 데이터로부터 특성을 추출하여 맞춤형 노래 추천(개발중)

## 프로젝트 인원

### Unity(4명)
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

### Infra(1명)
최재용 (인프라 구축, 데이터 분석)
- AWS, Git, Docker, Jenkins 기반 배포 자동화
- 서버 도메인 등록, HTTPS 및 Nginx 적용
- ML 모델 Flask 서비스 배포(Gradient Boosting Multioutput Regressor, Sequential Model)
- Flask REST API 서버로 작곡 추천 API 구현

### Backend(1명)
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
