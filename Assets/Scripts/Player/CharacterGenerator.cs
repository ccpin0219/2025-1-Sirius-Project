using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "RPG/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public int speed;
    public int maxHp; // <--- 이 필드가 필요합니다!
    public int defense; // <--- 이 필드가 필요합니다!
    // public GameObject unitPrefab; // UnitBase를 붙일 프리팹 참조는 없어도 됨 (UnitBase가 데이터를 참조하므로)
    public List<CardData> defaultCards;
}
