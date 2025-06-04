using System;
using System.Collections.Generic;

public class Character
{
    public string Name;               // 이름
    public int CurrentHP;            // 현재 체력
    public bool IsAlive => CurrentHP > 0; // 생존 여부
    public List<Skill> Skills;       // 보유 스킬 목록

    // 캐릭터 생성자
    public Character(string name, int hp, List<Skill> skills)
    {
        Name = name;
        CurrentHP = hp;
        Skills = skills;
    }

    // 스킬 사용 함수
    public void UseSkill(Skill skill, Character target)
    {
        if (skill.Damage >= 0)
        {
            // 공격 스킬
            target.CurrentHP -= skill.Damage;
            Console.WriteLine($"{Name}이(가) {target.Name}에게 '{skill.Name}' 사용! 피해 {skill.Damage}");
        }
        else
        {
            // 방어 스킬
            CurrentHP -= skill.Damage; // 마이너스 데미지는 회복
            Console.WriteLine($"{Name}이(가) 자신에게 '{skill.Name}' 사용! 회복 {-skill.Damage}");
        }

        // 체력이 마이너스 되는 것 방지
        if (target.CurrentHP < 0) target.CurrentHP = 0;
    }
}
