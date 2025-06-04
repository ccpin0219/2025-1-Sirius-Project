using System;
using System.Linq;

public class TurnManager
{
    private int turnCount = 1;  // 현재 턴 수

    // 전투 시작
    public void StartBattle()
    {
        Console.WriteLine("전투 시작!");

        // 게임이 끝날 때까지 반복
        while (!IsBattleOver())
        {
            Console.WriteLine($"\n===== 턴 {turnCount} 시작 =====");

            // 아군 행동
            foreach (var ally in GameManager.Instance.characterManager.Allies.Where(a => a.IsAlive))
            {
                var enemy = GameManager.Instance.characterManager.Enemies.FirstOrDefault(e => e.IsAlive);
                ally.UseSkill(ally.Skills[0], enemy); // 첫 번째 스킬 사용
            }

            // 적군 행동
            foreach (var enemy in GameManager.Instance.characterManager.Enemies.Where(e => e.IsAlive))
            {
                var ally = GameManager.Instance.characterManager.Allies.FirstOrDefault(a => a.IsAlive);
                enemy.UseSkill(enemy.Skills[0], ally); // 첫 번째 스킬 사용
            }

            turnCount++;
        }

        Console.WriteLine("\n전투 종료!");
    }

    // 게임 종료 조건
    private bool IsBattleOver()
    {
        var allies = GameManager.Instance.characterManager.Allies;
        var enemies = GameManager.Instance.characterManager.Enemies;

        return allies.All(a => !a.IsAlive) || enemies.All(e => !e.IsAlive);
    }
}
