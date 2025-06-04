using System;
using System.Collections.Generic;

public class CharacterManager
{
    public List<Character> Allies { get; private set; }   // 아군 리스트
    public List<Character> Enemies { get; private set; }  // 적군 리스트

    // 캐릭터 초기화 함수
    public void LoadCharacters()
    {
        Allies = new List<Character>
        {
            new Character("아군1", 100, new List<Skill> { new Skill("베기", 20) }),
            new Character("아군2", 90, new List<Skill> { new Skill("치료", -15) }),
        };

        Enemies = new List<Character>
        {
            new Character("적군1", 120, new List<Skill> { new Skill("물기", 18) }),
            new Character("적군2", 80, new List<Skill> { new Skill("찌르기", 22) }),
        };
    }
}