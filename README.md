<div align="center">

# 궁야 : 폐궁의 밤

<img src="https://github.com/user-attachments/assets/5013fe80-a894-4edb-832e-b66e534e5094" width="400"/>

</div>

---

## 🕹️ 프로젝트 개요

**폐궁의 밤**은 공포게임 제작 의견을 모아, **한국 전통 요소를 융합한 탈출형 1인칭 공포 어드벤처 게임**입니다.

| 항목 | 내용 |
|------|------|
| 장르 | 1인칭 공포 어드벤처 탈출맵 |
| 제작 기간 | 2025.06.11 ~ 2025.06.18 (일주일) |

---

## 🧟 게임 시나리오

> “서울 한복판, 600년 동안 봉인되어 있던 폐궁이 다시 열렸다.”

202X년, 서울.  
고궁 복원 프로젝트 중 **600년 전 조선의 비밀 궁전 ‘암궁(暗宮)’**이 발견됩니다.  
복원 작업 중 탐사팀과의 연락이 두절되고, 내부에서는 기괴한 형체와 한복 차림의 시체들이 발견됩니다.  
정부는 이를 은폐하고, **특수 생존 전문가 팀**을 투입합니다. 당신은 그 중 한 명입니다.

---

## 🧩 게임 방법

- 좌측 상단에 표시되는 힌트를 참고하여 진행합니다.
- 쓰러진 시체나 오브젝트에서 힌트를 얻어 탈출 방법을 찾아내야 합니다.
- 챕터별로 진행하며 궁전의 비밀과 탈출 방법을 파악해 탈출하세요!


---

## 🎮 조작 방법

| 조작 | 키 |
|------|----|
| 이동 | WASD |
| 마우스 회전 | 마우스 |
| 상호작용 | E |
| 점프 | Space Bar |
| 달리기 | Shift |
| 앉기 | Ctrl |
| 설정 | Esc |
| 인벤토리 | Tab |
| 장착 아이템 사용 | 마우스 우클릭 |

---

### 🎮 게임 구조 다이어그램
<img src="https://github.com/user-attachments/assets/f054395e-b608-4b93-814e-c6b09310a781" alt="Game Structure Diagram" width="400"/>

---

## 🔧 게임 기능

| 번호 | 기능 설명 |
|------|-----------|
| 1 | FSM 패턴을 통한 플레이어 이동 및 기본 동작 |
| 2 | 인터페이스를 활용한 다양한 오브젝트 상호작용 |
| 3 | Physics.OverlapSphere 및 Ray를 활용한 적과 상호작용 |
| 4 | 설정, 크레딧, 인벤토리 등 UI/UX 패널 구현 |
| 5 | 인트로/아웃트로 영상의 Cinemachine 카메라 연출 |
| 6 | 몬스터가 플레이어를 쫓는 도중 장애물이 있을 때 파괴 |
| 7 | 다양한 무기 및 아이템 구현 |
| 8 | 다양한 좀비 종류 (걷는, 뛰는, 쓰러진 좀비) |
| 9 | 주기적 천둥 효과를 통한 어두운 맵 밝기 보정 |
| 10 | 3D 공간 사운드 및 오브젝트별 효과음 |
| 11 | 피격 시 피 파티클, CameraShake 등 연출 효과 |
| 12 | 후처리(PostProcessing): Noise, Motion Blur, 색 보정 |
| 13 | 데시벨에 따른 오브젝트 상호작용 |
| 14 | 게임 전체 진행 안내 시스템 (챕터 가이드) |
| 15 | 시야각 기반 몬스터 오브젝트 풀링으로 메모리 최적화 |
| 16 | 랜덤한 위치의 몬스터 스폰 |
| 17 | 지정된 포인트에 랜덤 상호작용 오브젝트 스폰 |

---

1. FSM 패턴을 통한 플레이어 이동 및 기본 동작
    - StateMachine 구조를 활용.
    - 각 State에서 필요한 애니메이션 키는 Hash로 캐싱 후 불러오기
    //TODO : 코드 필요.
<div align="center">
<img src="https://github.com/user-attachments/assets/137f0bcf-7632-4c31-a59a-2ca6036ca332" alt="플레이어 " width="600"/>
</div>


2. 인터페이스를 활용하여 다양한 오브젝트 상호작용
   - IInteractable이라는 스크립트와 레이어를 활용하여 상호작용 판별.
   - IInteractable 중에서도 Item이 있는지에 따라, 예외처리를 통해 인벤토리 아이템 추가
<div align="center">
<img src="https://github.com/user-attachments/assets/31074593-ea07-4b04-a78c-37cc8e26dc73" alt="상호작용 " width="600"/>
</div>

3. Physics.OverlapSphere 및 Ray를 활용한 적과 상호작용
   - Physics.OverlapSphere로 적을 발견하면 추적 상태,
   - 더 작은 크기의 Physcis.OverlapSphere와 Ray가 닿으면 공격 상태,
   - 공격 시 피격 받는 순간, Ray를 통해 플레이어가 존재한다면 공격 판정.
  //TODO : 코드 필요.

4. 설정, 크레딧, 인벤토리 등 UI/UX 패널 구현
<div align="center">
<img src="https://github.com/user-attachments/assets/9b26ef47-db96-47b1-bf28-1ee29c82e26b" alt="UI " width="600"/>
</div>

6. 인트로, 아웃트로 영상의 CineMachine 카메라 연출
   - Cinemachine 을 활용한 자연스러운 영상 연출
<div align="center">
<img src="https://github.com/user-attachments/assets/2f288483-297e-4d72-977d-96b7c8ac987c" alt="인트로 " width="600"/>
</div>

7. 몬스터가 플레이어를 쫓는 도중 장애물이 있을 때 파괴
   - 플레이어가 사정거리 안에 있고 OverlapCircle로 장애물이 판별되면 이동 후 파괴.


8. 다양한 무기 및 아이템 구현
   - ScriptableObject로 구현.
   - 장착 아이템과 소비아이템, Quest 아이템으로 구분

9. 다양한 좀비 종류
   - 걷는 좀비, 뛰는 좀비, 쓰러진 좀비.
<div align="center">
<img src="https://github.com/user-attachments/assets/48e64d16-3599-4628-96e7-8b1118497d4a" alt="적 " width="600"/>
</div>


10. 주기적 천둥 효과를 통한 어두운 맵 밝기 보정
   - Directional Light의 수치를 Curve를 통해 조정.
   - Coroutine으로 주기를 설정.
<div align="center">
<img src="https://github.com/user-attachments/assets/d5e06508-86d9-43cc-988e-88ba0e611ec9" alt="번개" width="600"/>
</div>
  
11. 3D 공간 사운드 및 오브젝트별 효과음.
    - 각 오브젝트에 사운드를 각각 할당하여 3D Volume을 통해 소리 증감.
    - 사운드에 반응하는 몬스터 로직도 존재.

12. 피격 시, 피 파티클과 CameraShake 등 연출 효과
    - 맞았을 때, 잠깐의 경직과 함께 파티클 생성과 CameraShake.
    - Coroutine을 통해 약간의 시간 후에 다시 원 상태로 복구.
   
13. 후처리 PostProcess를 활용하여 Noise, MotionBlur, 색 보정
    - 보다 공포스럽고 현실적인 연출을 위한 Noise, MotionBlur를 지정.
   
14. 데시벨에 따른 오브젝트 상호작용
    - 우측 하단에 사운드 데시벨이 커지면 좀비가 감지함.
   
15. 게임 전체 진행 안내 시스템 (챕터 가이드)
    - Zone, Item, Door에 ChapterEvent를 통해 ChapterManager의 이벤트를 진행.
   
16. 시야각 기반 몬스터 오브젝트 풀링으로 메모리 최적화
    - 각 구역을 정해 처음에 전부 생성하고 플레이어가 보지 못하는 구역에는 몬스터를 회수함.
    - 회수한 몬스터는 구역을 이동할 때마다 구역에 맞는 곳에 좀비들을 생성.

17. 랜덤한 위치의 몬스터 스폰
    - 위 내용처럼 구역 내에 몬스터가 랜덤하게 생성.

18. 지정된 포인트에 랜덤 상호작용 오브젝트 스폰
    - 지정된 포인트 중에 랜덤으로 오브젝트 생성.
<div align="center">
<img src="https://github.com/user-attachments/assets/ff93b1be-d327-45d4-bebc-62c2123de887" alt="번개" width="600"/>
</div>


## 🛠️ 기술 스택

| 기술 | 설명 |
|------|------|
| FSM | 상태 관리 패턴으로 플레이어, 몬스터 행동 제어 |
| AI Navigation | Unity 내장 네비게이션 시스템 |
| Async Operation | 비동기 연산으로 로딩 최적화 |
| Object Pooling | 오브젝트 풀링으로 퍼포먼스 최적화 |

---

## 🧪 트러블슈팅

| 이름 | 이슈 내용 |
|------|-----------|
| 손유민 | 프로퍼티 직렬화 실패 |
| 이기안 | 씬 전환 시 검은 화면 로딩 문제 |
| 김예지 | 오브젝트 풀 위치 재조정 문제 |
| 이수민 | 적 추적 중 문 타격 미작동 문제 |
| 김가인 | PostProcessing 애니메이션 문제 |

---

## 👨‍👩‍👧‍👦 제작자

| 이름 | 역할 |
|------|------|
| 손유민 | 팀장, 인벤토리 구조 설계, 아이템 시스템, 챕터 구간 생성, 랜덤 오브젝트 구현 |
| 이기안 | 플레이어 FSM 및 이동 구현, UI/UX, 사운드 |
| 김예지 | 상호작용 오브젝트 구성, 레벨 디자인, 오브젝트 풀링, FSM 캐싱 |
| 이수민 | 적 FSM, AI NavMesh, 게임 디자인 |
| 김가인 | 인트로/엔딩 씬 연출 (Cinemachine), 특수 Enemy 연출, 데시벨 상호작용 |

---
