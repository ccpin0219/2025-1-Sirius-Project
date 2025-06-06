# 2025-1-Sirius-Project

# 프로젝트 README

### Managers

게임의 핵심 로직과 전체 흐름을 제어하는 스크립트들이 위치합니다.

*   **`BattleManager.cs`**
    *   전투의 시작, 진행, 종료를 관리하는 메인 컨트롤러/
    *   유닛들의 턴 순서를 결정하고, 현재 턴인 유닛에게 행동 기회를 부여.
    *   플레이어의 행동(카드 사용, 타겟 지정) 또는 적의 AI 로직을 실행시키는 중심점.
        - [ ] AI 미구현
        - [ ] 공격 주고 받을시 HP 증감 미구현
        - [ ] 적 공격 미구현
    *   전투 승리/패배 조건을 확인 
    > 우리팀 전멸 || 상대팀 전멸

*   **`CardSelect.cs`**:
    *   플레이어가 현재 턴에 사용할 카드를 선택하는 UI
    *   `BattleManager`로부터 사용 가능한 카드 목록을 받아와 `CardView.cs`를 통해 시각적으로 드러냄

*   **`TargetSelector.cs`**:
    *   선택한 카드나 스킬을 사용할 대상을 지정
    *   `BattleManager`로부터 현재 타겟팅 가능한 유닛 리스트(`UnitBase` 목록)를 받아옴.
    *   플레이어가 타겟을 선택하면, 해당 `UnitBase` 정보를 `BattleManager`나 카드 사용 로직으로 전달
    *   `TargetView.cs`를 통해 각 타겟 후보를 시각적으로 표시하고, 현재 선택된 타겟에 하이라이트 등의 효과를 줄 수 있습니다.

*   **`UnitBase.cs`**:
    * 데이터 대리: 실제 능력치(CharacterData/EnemyData ScriptableObject 참조)를 가져와 제공 (HP, 이름, 카드 등). 적과 아군을 동시에 관리하기 위함
    * 상태 관리: 현재 HP (currentHp), 타겟 지정 여부 (isTargeted)를 관리.
    * 핵심 행동: 데미지 받기 (TakeDamage), 죽음 처리 (Die), HP 초기화 (InitializeHp).
    * UI 연동: 타겟 지정 시 연결된 UI 이미지(Image)의 색상을 변경 (SetTargeted)하여 시각적 피드백 제공.
        - [ ] 아직 로직은 없음 -> 데미지 안받음

### 카드 시스템

카드 자체의 데이터, 생성, 시각적 표현을 담당합니다.

*   **`CardGenerator.cs`**:
    *   `CardData` ScriptableObject -> 그냥 붕어빵 틀 같은거임 카드를 정보대로 찍어낼 수 있음 
    *   `CardManager`나 `CardSelect.cs`가 필요로 하는 카드 인스턴스를 제공

*   **`CardView.cs`**:
    *   그냥 빈 물체에 옷 입히는 용도임

### 적 시스템 (Enemy)

*   **`EnemyGenerator.cs`**:
    *   카드와 같음

*   **`EnemyUI.cs`**:
    *   적 유닛들을 화면에 배치

*   **`EnemyView.cs`**:
    *   그냥 빈 물체에 옷 입히는 용도임

### 플레이어 시스템 (Player)

*   **`CharacterGenerator.cs`**:
    *   카드와 같음

*   **`CharacterUI.cs`**:
    *   아군 배치

*   **`CharacterView.cs`**:
    *   그냥 빈 물체에 옷 입히는 용도임

## 데이터 흐름

1.  **게임 시작/전투 준비**:
    *   `CharacterUI.cs`와 `EnemyUI.cs`가 각각 `CharacterGenerator.cs`와 `EnemyGenerator.cs`로 만든 데이터로 view를 통해 생성함
    *   `UnitBase`로 데이터 접근함
    *   `BattleManager.cs`가 모든 유닛 정보를 수집하고 턴 순서를 결정

2.  **플레이어 턴**:
    *   `BattleManager.cs`가 순서를 정함 -> Queue에 넣음
    *   `CardManager.cs`가 있다면 카드를 드로우하고 핸드를 구성합니다.
    *   `CardSelect.cs`가 활성화되어 플레이어에게 사용 가능한 카드를 (`CardView.cs`를 통해) 보여줌
    *   플레이어가 카드를 선택합니다.
    *   선택된 카드가 타겟을 필요로 하면, `CardSelect.cs`가 `TargetSelector.cs`를 활성화하고 타겟팅 가능한 유닛 리스트를 전달
    *   `TargetSelector.cs`는 `UnitBase` 리스트를 받아 각 유닛을 (`TargetView.cs`를 통해) 표시하고, 플레이어는 타겟을 선택
    *   선택된 카드와 타겟 정보가 `BattleManager.cs`로 전달됨
        - [ ] 마지막 행동 잘 안되는 것 같음 확인 필요

3.  **적 턴**:
    *   `BattleManager.cs`가 현재 적 유닛의 턴 확인
    - [ ] 아무로직도 짜여있는게 없음

4.  **턴 종료 및 반복**:
    *   `BattleManager.cs`가 현재 턴을 종료하고, 다음 유닛의 턴을 시작.
    *   전투 종료 조건이 충족될 때까지 이 과정 반복.
