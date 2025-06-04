using System;
using System.Linq;

public class TurnManager
{
    private int turnCount = 1;  // ���� �� ��

    // ���� ����
    public void StartBattle()
    {
        Console.WriteLine("���� ����!");

        // ������ ���� ������ �ݺ�
        while (!IsBattleOver())
        {
            Console.WriteLine($"\n===== �� {turnCount} ���� =====");

            // �Ʊ� �ൿ
            foreach (var ally in GameManager.Instance.characterManager.Allies.Where(a => a.IsAlive))
            {
                var enemy = GameManager.Instance.characterManager.Enemies.FirstOrDefault(e => e.IsAlive);
                ally.UseSkill(ally.Skills[0], enemy); // ù ��° ��ų ���
            }

            // ���� �ൿ
            foreach (var enemy in GameManager.Instance.characterManager.Enemies.Where(e => e.IsAlive))
            {
                var ally = GameManager.Instance.characterManager.Allies.FirstOrDefault(a => a.IsAlive);
                enemy.UseSkill(enemy.Skills[0], ally); // ù ��° ��ų ���
            }

            turnCount++;
        }

        Console.WriteLine("\n���� ����!");
    }

    // ���� ���� ����
    private bool IsBattleOver()
    {
        var allies = GameManager.Instance.characterManager.Allies;
        var enemies = GameManager.Instance.characterManager.Enemies;

        return allies.All(a => !a.IsAlive) || enemies.All(e => !e.IsAlive);
    }
}
