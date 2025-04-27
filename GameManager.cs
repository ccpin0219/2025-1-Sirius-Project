using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("전투 덱 설정")]
    public DeckPreset defaultDeck;

    [System.Serializable]
    public class EncounterData
    {
        public string label;
        public List<EnemyData> enemies;
    }

    [Header("Encounter 설정")]
    public List<EncounterData> encounters;

    [Header("전투 프리팹 & 오브젝트")]
    public GameObject enemyPrefab;
    public Transform enemyParent;

    public Player player;
    public CardManager cardManager;
    public TurnManager turnManager;

    private List<Enemy> activeEnemies = new();

    void Start()
    {
        LoadEncounter(1); // 테스트용: 전투1-2(슬라임1, 거미1)
    }

    public void LoadEncounter(int index)
    {
        if (index < 0 || index >= encounters.Count)
        {
            Debug.LogError("Encounter 인덱스 범위 초과!");
            return;
        }

        EncounterData selected = encounters[index];
        Debug.Log($"[전투 시작] 선택된 전투: {selected.label}");

        SetupBattle(selected.enemies);
    }

    private void SetupBattle(List<EnemyData> enemyDatas)
    {
        // 기존 적 제거
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }
        activeEnemies.Clear();

        // ===== 적 위치 설정 (카메라 기준 정렬) =====
        float spacing = 0.2f; // Viewport 비율 간격
        float yPos = 0.6f; // 세로 위치 (0 = 화면 아래, 1 = 위)
        float zDistance = 10f; // 카메라로부터 거리

        int enemyCount = enemyDatas.Count;
        float center = 0.5f;
        float startX = center - ((enemyCount - 1) * spacing * 0.5f); // 중앙 기준 좌우 정렬

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 viewportPos = new Vector3(startX + spacing * i, yPos, zDistance);
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);

            GameObject go = Instantiate(enemyPrefab, enemyParent);
            go.transform.position = worldPos;

            Enemy enemy = go.GetComponent<Enemy>();
            enemy.data = enemyDatas[i];
            activeEnemies.Add(enemy);
        }

        // ===== 카드 매니저 설정 =====
        cardManager.SetDeck(defaultDeck);
        cardManager.SetPlayer(player);
        cardManager.SetEnemies(activeEnemies);
        cardManager.Initialize();

        // ===== 턴 매니저 설정 =====
        turnManager.SetPlayer(player);
        turnManager.SetEnemies(activeEnemies);
        turnManager.SetCardManager(cardManager);

        // ===== 전투 시작 딜레이 =====
        Invoke(nameof(StartTurnAfterSetup), 0.1f);
    }

    private void StartTurnAfterSetup()
    {
        turnManager.StartBattle();
    }
}
