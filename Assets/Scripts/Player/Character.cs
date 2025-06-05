using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;
    public int CurrentHP;
    public bool IsAlive => CurrentHP > 0;
    public List<Skill> Skills;

    public Character(string name, int hp, List<Skill> skills)
    {
        Name = name;
        CurrentHP = hp;
        Skills = skills;
    }

    public void UseSkill(Skill skill, Character target)
    {
        if (skill.Damage >= 0)
        {
            // ���� ��ų
            target.CurrentHP -= skill.Damage;
            Debug.Log($"{Name}��(��) {target.Name}���� '{skill.Name}' ���! ���� {skill.Damage}");
        }
        else
        {
            // ȸ�� ��ų
            CurrentHP -= skill.Damage; // ���̳ʽ� ������ = ȸ��
            Debug.Log($"{Name}��(��) �ڽſ��� '{skill.Name}' ���! ȸ�� {-skill.Damage}");
        }

        if (target.CurrentHP < 0) target.CurrentHP = 0;
        Debug.Log($"{target.Name}�� ���� ü��: {target.CurrentHP}");
    }
}

