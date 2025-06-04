using System;

public class GameManager
{
    public static GameManager Instance { get; private set; }
    public CharacterManager characterManager;  // ĳ���� ������ ���� �Ŵ���
    public TurnManager turnManager;            // �� ���� �Ŵ���

    public GameManager()
    {
        Instance = this;
        characterManager = new CharacterManager();  // ĳ���� �Ŵ��� �ʱ�ȭ
        turnManager = new TurnManager();            // �� �Ŵ��� �ʱ�ȭ
    }

    // ���� ����
    public void StartGame()
    {
        Console.WriteLine("�ε���...");

        characterManager.LoadCharacters(); // ĳ���� ���� �ҷ�����
        turnManager.StartBattle();         // ���� ����

        Console.WriteLine("������ ���۵Ǿ����ϴ�.");
    }
}
