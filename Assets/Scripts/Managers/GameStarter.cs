using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        Debug.Log("���� �����մϴ�...");

        // GameManager �ʱ�ȭ �� ���� ����
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }
}
