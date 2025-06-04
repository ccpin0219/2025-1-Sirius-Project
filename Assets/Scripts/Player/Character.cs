using System;
using System.Collections.Generic;

public class Character
{
    public string Name;               // �̸�
    public int CurrentHP;            // ���� ü��
    public bool IsAlive => CurrentHP > 0; // ���� ����
    public List<Skill> Skills;       // ���� ��ų ���

    // ĳ���� ������
    public Character(string name, int hp, List<Skill> skills)
    {
        Name = name;
        CurrentHP = hp;
        Skills = skills;
    }

    // ��ų ��� �Լ�
    public void UseSkill(Skill skill, Character target)
    {
        if (skill.Damage >= 0)
        {
            // ���� ��ų
            target.CurrentHP -= skill.Damage;
            Console.WriteLine($"{Name}��(��) {target.Name}���� '{skill.Name}' ���! ���� {skill.Damage}");
        }
        else
        {
            // ��� ��ų
            CurrentHP -= skill.Damage; // ���̳ʽ� �������� ȸ��
            Console.WriteLine($"{Name}��(��) �ڽſ��� '{skill.Name}' ���! ȸ�� {-skill.Damage}");
        }

        // ü���� ���̳ʽ� �Ǵ� �� ����
        if (target.CurrentHP < 0) target.CurrentHP = 0;
    }
}
