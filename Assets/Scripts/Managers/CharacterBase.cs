using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterBase")]
public class CharacterBase : ScriptableObject
{
    public CharacterStats stats;
    public Sprite characterSprite;

    public List<Skill> skillList; //  각 캐릭터의 스킬을 정의
}