using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    public List<Character> Allies { get; private set; }
    public List<Character> Enemies { get; private set; }

    public void LoadCharacters()
    {
        Debug.Log("ĳ���� ������ �ҷ����� ��...");

        Allies = new List<Character>
        {
            new Character("�Ʊ�1", 100, new List<Skill> { new Skill("����", 20) }),
            new Character("�Ʊ�2", 90, new List<Skill> { new Skill("ġ��", -15) }),
        };

        Enemies = new List<Character>
        {
            new Character("����1", 120, new List<Skill> { new Skill("����", 18) }),
            new Character("����2", 80, new List<Skill> { new Skill("���", 22) }),
        };

        Debug.Log("ĳ���� �ε� �Ϸ� (�Ʊ� " + Allies.Count + "��, ���� " + Enemies.Count + "��)");
    }
}
