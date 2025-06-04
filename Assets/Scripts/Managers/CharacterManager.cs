using System;
using System.Collections.Generic;

public class CharacterManager
{
    public List<Character> Allies { get; private set; }   // �Ʊ� ����Ʈ
    public List<Character> Enemies { get; private set; }  // ���� ����Ʈ

    // ĳ���� �ʱ�ȭ �Լ�
    public void LoadCharacters()
    {
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
    }
}