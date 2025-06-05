using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    public List<Character> Allies { get; private set; }
    public List<Character> Enemies { get; private set; }

    public void LoadCharacters()
    {
        Debug.Log("캐릭터 데이터 불러오는 중...");

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

        Debug.Log("캐릭터 로딩 완료 (아군 " + Allies.Count + "명, 적군 " + Enemies.Count + "명)");
    }
}
