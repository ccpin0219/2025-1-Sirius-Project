using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Player player;
    private List<Enemy> enemies = new();
    private CardManager cardManager;

    private enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }
    private BattleState currentState;

    public void SetPlayer(Player p) => player = p;
    public void SetEnemies(List<Enemy> e) => enemies = e;
    public void SetCardManager(CardManager cm) => cardManager = cm;

    public void StartBattle()
    {
        Debug.Log("===== [플레이어 턴 시작] =====");
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        currentState = BattleState.PlayerTurn;

        player.ResetEnergy();
        player.ResetBlock();
        cardManager.RefillHand();
    }

    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        Debug.Log("[E키 입력] 턴 종료");
        Debug.Log("플레이어 턴 종료!\n");

        cardManager.DiscardHand();
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        currentState = BattleState.EnemyTurn;

        Debug.Log("===== [에너미 턴 시작] =====");

        foreach (Enemy e in enemies)
        {
            if (e != null && e.currentHealth > 0)
            {
                e.AttackPlayer(player);
            }
        }

        CheckBattleEnd();

        if (currentState != BattleState.Win && currentState != BattleState.Lose)
        {
            Debug.Log("===== [플레이어 턴 시작] =====");
            StartPlayerTurn();
        }
    }

    void CheckBattleEnd()
    {
        bool allEnemiesDead = enemies.TrueForAll(e => e.currentHealth <= 0);

        if (allEnemiesDead)
        {
            currentState = BattleState.Win;
            Debug.Log("🎉 전투 승리!");
        }
        else if (player.currentHealth <= 0)
        {
            currentState = BattleState.Lose;
            Debug.Log("💀 전투 패배...");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentState == BattleState.PlayerTurn)
        {
            EndPlayerTurn();
        }
    }
}
