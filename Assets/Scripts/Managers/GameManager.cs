using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TurnManager turnManager;
    public CharacterSpawner spawner;
    public PlayerSkillController playerSkillController;

    public List<Enemy> enemies = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (spawner == null || turnManager == null || playerSkillController == null)
        {
            Debug.LogError("GameManager에 필요한 컴포넌트가 연결되지 않았습니다.");
            return;
        }

        spawner.SpawnCharacters();

        // 아군 + 적군 리스트 생성
        List<Character> allCharacters = new List<Character>
        {
            spawner.warriorObj.GetComponent<Character>(),
            spawner.mageObj.GetComponent<Character>(),
            spawner.wolf1Obj.GetComponent<Enemy>(),
            spawner.wolf2Obj.GetComponent<Enemy>()
        };

        turnManager.InitTurnOrder(allCharacters);
    }
}
