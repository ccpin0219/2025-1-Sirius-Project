using UnityEngine;
using System.Linq;

public class TurnManager
{
    private int turnCount = 1;

    public void StartBattle()
    {
        Debug.Log("전투 시작!");

        while (!IsBattleOver())
        {
            Debug.Log($"===== 턴 {turnCount} 시작 =====");

            foreach (var ally in GameManager.Instance.characterManager.Allies.Where(a => a.IsAlive))
            {
                var enemy = GameManager.Instance.characterManager.Enemies.FirstOrDefault(e => e.IsAlive);
                ally.UseSkill(ally.Skills[0], enemy);
            }

            foreach (var enemy in GameManager.Instance.characterManager.Enemies.Where(e => e.IsAlive))
            {
                var ally = GameManager.Instance.characterManager.Allies.FirstOrDefault(a => a.IsAlive);
                enemy.UseSkill(enemy.Skills[0], ally);
            }

            turnCount++;
        }

        Debug.Log("전투 종료!");
    }

    private bool IsBattleOver()
    {
        var allies = GameManager.Instance.characterManager.Allies;
        var enemies = GameManager.Instance.characterManager.Enemies;

        return allies.All(a => !a.IsAlive) || enemies.All(e => !e.IsAlive);
    }
}

