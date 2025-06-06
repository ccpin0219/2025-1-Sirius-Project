// --- START OF FILE BattleManager.cs ---
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("프리팹/데이터")]
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;

    public List<CharacterData> playerParty; // 플레이어 데이터 리스트
    public List<EnemyData> enemyParty;      // 적 데이터 리스트

    public Transform playerRoot; // 플레이어 유닛 부모(위치)
    public Transform enemyRoot;  // 적 유닛 부모(위치)

    [Header("UI 연결")]
    public CardSelector cardSelector; // 수정된 CardSelector를 참조

    [Header("런타임 유닛 리스트 (자동생성)")]
    public List<UnitBase> allUnits = new List<UnitBase>();

    public CharacterUI characterManager; // 아군 유닛 생성/관리 UI
    public EnemyUI enemyUI;             // 적군 유닛 생성/관리 UI

    private List<UnitBase> turnOrder = new List<UnitBase>();
    private int currentTurnIndex = 0;

    public static BattleManager Instance { get; private set; }

    // 타겟 선택 시 사용될 수 있는 유닛 리스트 반환 (예: 현재 턴 유닛에 따라 적절한 타겟 리스트)
    public List<UnitBase> GetTargetList()
    {
        UnitBase currentUnit = turnOrder[currentTurnIndex];
        if (currentUnit.IsPlayer)
        {
            // 플레이어 턴이면 살아있는 적군 반환
            return allUnits.FindAll(u => !u.IsPlayer && !u.IsDead);
        }
        else
        {
            // 적군 턴이면 살아있는 플레이어 반환 (AI 로직에 따라 다를 수 있음)
            return allUnits.FindAll(u => u.IsPlayer && !u.IsDead);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 필요하다면 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 유닛 스폰은 CharacterUI, EnemyUI에 위임된 것으로 가정
        if (characterManager != null) characterManager.SpawnAll(); else Debug.LogError("[BattleManager] CharacterManager is not assigned!");
        if (enemyUI != null) enemyUI.SpawnAll(); else Debug.LogError("[BattleManager] EnemyUI is not assigned!");

        // CardSelector가 초기에는 비활성화되도록 확실히 함
        if (cardSelector != null)
        {
            cardSelector.Hide(); // 시작 시에는 카드 UI 숨김
        }
        else
        {
            Debug.LogError("[BattleManager] CardSelector is not assigned!");
            return; // CardSelector 없이는 진행 불가
        }

        StartBattle();
    }

    public void StartBattle()
    {
        // UnitBase를 가진 모든 활성 GameObject를 찾아 allUnits 리스트에 추가
        allUnits = new List<UnitBase>(FindObjectsOfType<UnitBase>(false)); // 비활성 객체는 포함 안 함
        if (allUnits.Count == 0)
        {
            Debug.LogError("[BattleManager] No units found in the scene for battle!");
            return;
        }
        turnOrder = new List<UnitBase>(allUnits);
        // 속도(Speed)에 따라 내림차순 정렬 (속도가 높은 유닛이 먼저 턴을 가짐)
        turnOrder.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        currentTurnIndex = 0;
        StartTurn();
    }

    void StartTurn()
    {
        Debug.Log($"[BattleManager] === StartTurn 호출됨. Current Turn Index: {currentTurnIndex} ===");

        if (turnOrder == null || turnOrder.Count == 0)
        {
            Debug.LogWarning("[BattleManager] Turn order is empty or null. Ending battle.");
            EndBattle(false); // 무승부 또는 오류로 전투 종료
            return;
        }

        // 모든 유닛이 죽었는지 확인 (이 경우는 거의 없지만 방어 코드)
        int aliveUnits = 0;
        foreach(var unit in turnOrder) { if(!unit.IsDead) aliveUnits++; }
        if(aliveUnits == 0) {
            Debug.LogWarning("[BattleManager] All units in turn order are dead. Ending battle.");
            EndBattle(false); // 또는 상황에 맞는 결과
            return;
        }

        // 현재 턴 유닛이 죽었다면, 다음 살아있는 유닛으로 넘김
        int checkedCount = 0;
        while (turnOrder[currentTurnIndex].IsDead)
        {
            Debug.Log($"[BattleManager] Unit {turnOrder[currentTurnIndex].UnitName} is dead. Skipping turn.");
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            checkedCount++;
            if (checkedCount >= turnOrder.Count) // 모든 유닛을 확인했으나 살아있는 유닛이 없는 경우
            {
                Debug.LogWarning("[BattleManager] All units checked and found dead. Ending battle.");
                EndBattle(false); // 또는 상황에 맞는 결과
                return;
            }
        }

        UnitBase currentUnit = turnOrder[currentTurnIndex];
        Debug.Log($"[BattleManager] ========== 턴 시작 [{currentTurnIndex + 1}/{turnOrder.Count}] ==========");
        Debug.Log($"[BattleManager] [{(currentUnit.IsPlayer ? "플레이어" : "적")}] 턴: {currentUnit.UnitName} (HP: {currentUnit.currentHp})");

        if (currentUnit.IsPlayer)
        {
            if (currentUnit.CardList == null || currentUnit.CardList.Count == 0)
            {
                // 카드가 없으면 바로 턴을 넘기거나, 다른 액션을 유도할 수 있음
                // 여기서는 임시로 턴을 넘기도록 처리 (실제 게임에서는 다른 UI 표시 등 필요)
                NextTurn(); 
                return;
            }
            cardSelector.SetCards(currentUnit.CardList, OnPlayerCardSelected);
            // CardSelector.SetCards 내부에서 gameObject.SetActive(true)를 호출하므로 여기서 중복 호출 필요 없음
        }
        else // 적 턴
        {
            Debug.Log($"[BattleManager] 적 유닛 ({currentUnit.UnitName}) 턴 시작. 카드 UI 숨김.");
            cardSelector.Hide(); // 적 턴에는 카드 셀렉터와 카드 오브젝트 모두 숨기기/제거
            StartCoroutine(EnemyAction(currentUnit));
        }
    }

    // 플레이어가 카드를 선택하고 타겟을 지정했을 때 CardSelector로부터 호출됨
    void OnPlayerCardSelected(CardData card, UnitBase target)
    {
        UnitBase caster = turnOrder[currentTurnIndex]; // 현재 턴 유닛
        if (card == null) { Debug.LogError("[BattleManager] OnPlayerCardSelected: CardData is null!"); NextTurn(); return; }
        // target이 null일 수 있음 (광역기 등). CardData.ApplyEffect에서 처리 필요.
        
        Debug.Log($"[BattleManager] {caster.UnitName}이(가) {(target != null ? target.UnitName : "전체")}에게 {card.cardName} 사용 시도!");
        card.ApplyEffect(caster, target); // 카드 효과 적용
        
        // 여기에 카드 사용 애니메이션, 사운드 등 연출 추가 가능
        // yield return new WaitForSeconds(연출시간); 

        NextTurn(); // 다음 턴으로
    }

    // 적 유닛의 행동 처리 코루틴
    System.Collections.IEnumerator EnemyAction(UnitBase enemy)
    {
        Debug.Log($"[BattleManager] {enemy.UnitName} 행동 시작...");
        yield return new WaitForSeconds(1f); // AI 생각하는 시간 또는 연출 대기

        if (enemy.CardList == null || enemy.CardList.Count == 0)
        {
            Debug.LogWarning($"[BattleManager] 적 유닛({enemy.UnitName})의 CardList가 비어있거나 null입니다. 행동 불가.");
            NextTurn();
            yield break;
        }

        // 간단한 AI: 랜덤 카드, 랜덤 플레이어 타겟
        CardData cardToUse = enemy.CardList[Random.Range(0, enemy.CardList.Count)];
        UnitBase targetPlayer = PickRandomPlayerTarget();

        if (targetPlayer == null)
        {
            Debug.LogWarning($"[BattleManager] {enemy.UnitName}이(가) 공격할 플레이어 타겟을 찾지 못했습니다 (모두 사망?).");
            NextTurn();
            yield break;
        }
        
        Debug.Log($"[BattleManager] {enemy.UnitName}이(가) {targetPlayer.UnitName}에게 {cardToUse.cardName} 사용!");
        cardToUse.ApplyEffect(enemy, targetPlayer);

        // 여기에 적 카드 사용 애니메이션, 사운드 등 연출 추가 가능
        yield return new WaitForSeconds(1f); // 연출 대기
        NextTurn();
    }

    void NextTurn()
    {
        Debug.Log("[BattleManager] NextTurn 호출됨. 현재 카드 UI 정리.");
        cardSelector.Hide(); // 다음 턴 시작 전에 현재 턴의 카드 셀렉터 비활성화 및 카드 오브젝트 제거

        if (IsBattleOver()) // 이름 변경: IsBattleEnd -> IsBattleOver (더 명확)
        {
            // EndBattle() 내부에서 승패 판정 후 결과 처리
            return; 
        }

        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        StartTurn(); // 다음 유닛의 턴 시작
    }

    // 전투 종료 조건 확인
    bool IsBattleOver()
    {
        bool allEnemiesDead = allUnits.FindAll(u => !u.IsPlayer && !u.IsDead).Count == 0;
        bool allPlayersDead = allUnits.FindAll(u => u.IsPlayer && !u.IsDead).Count == 0;

        if (allEnemiesDead)
        {
            Debug.Log("[BattleManager] 모든 적군 사망. 플레이어 승리!");
            EndBattle(true); // 플레이어 승리
            return true;
        }
        if (allPlayersDead)
        {
            Debug.Log("[BattleManager] 모든 플레이어 사망. 플레이어 패배!");
            EndBattle(false); // 플레이어 패배
            return true;
        }
        return false; // 전투 계속
    }

    // 전투 종료 처리
    void EndBattle(bool playerWon)
    {
        Debug.Log($"[BattleManager] 전투 종료! 플레이어 승리: {playerWon}");
        cardSelector.Hide(); // 전투 종료 시 카드 UI 확실히 숨김
        // 여기에 결과 화면 표시, 보상 지급, 씬 이동 등의 로직 추가
        // Time.timeScale = 0f; // 게임 일시 정지 등
    }

    // 살아있는 플레이어 중 랜덤 타겟 선택
    UnitBase PickRandomPlayerTarget()
    {
        List<UnitBase> alivePlayers = allUnits.FindAll(u => u.IsPlayer && !u.IsDead);
        if (alivePlayers.Count > 0)
        {
            return alivePlayers[Random.Range(0, alivePlayers.Count)];
        }
        return null; // 살아있는 플레이어가 없음
    }
}
// --- END OF FILE BattleManager.cs ---