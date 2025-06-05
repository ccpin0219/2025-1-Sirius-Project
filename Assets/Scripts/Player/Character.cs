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
            // 공격 스킬
            target.CurrentHP -= skill.Damage;
            Debug.Log($"{Name}이(가) {target.Name}에게 '{skill.Name}' 사용! 피해 {skill.Damage}");
        }
        else
        {
            // 회복 스킬
            CurrentHP -= skill.Damage; // 마이너스 데미지 = 회복
            Debug.Log($"{Name}이(가) 자신에게 '{skill.Name}' 사용! 회복 {-skill.Damage}");
        }

        if (target.CurrentHP < 0) target.CurrentHP = 0;
        Debug.Log($"{target.Name}의 남은 체력: {target.CurrentHP}");
    }
}

