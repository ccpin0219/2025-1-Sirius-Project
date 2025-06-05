using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        Debug.Log("게임 시작합니다...");

        // GameManager 초기화 및 게임 시작
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }
}
