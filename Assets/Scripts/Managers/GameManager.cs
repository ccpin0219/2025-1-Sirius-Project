using System;

public class GameManager
{
    public static GameManager Instance { get; private set; }
    public CharacterManager characterManager;  // 캐릭터 데이터 관리 매니저
    public TurnManager turnManager;            // 턴 제어 매니저

    public GameManager()
    {
        Instance = this;
        characterManager = new CharacterManager();  // 캐릭터 매니저 초기화
        turnManager = new TurnManager();            // 턴 매니저 초기화
    }

    // 게임 시작
    public void StartGame()
    {
        Console.WriteLine("로딩중...");

        characterManager.LoadCharacters(); // 캐릭터 정보 불러오기
        turnManager.StartBattle();         // 전투 시작

        Console.WriteLine("게임이 시작되었습니다.");
    }
}
