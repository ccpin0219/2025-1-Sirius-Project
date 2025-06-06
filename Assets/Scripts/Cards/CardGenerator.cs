using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card Game/Card")]
public class CardData : ScriptableObject
{
    public string cardName;          // 카드 이름
    [TextArea]
    public string description;       // 카드 효과 설명
    public int cost;                 // 카드 코스트(마나 등)
    public CardType type;            // 카드 타입(공격/방어 등)
    public Sprite artwork;           // 카드 이미지(일러스트)

    // 필요하면 추가 속성(스킬, 사운드, 효과 등)
    // 옵션별 수치
    public int attackPower;   // Attack일 때만
    public int defensePower;  // Defense일 때만
    public int healPower;     // Heal일 때만

    // CardData.cs에
    public void ApplyEffect(UnitBase from, UnitBase to)
    {
        switch (type)
        {
            case CardType.Attack:
                to.currentHp -= attackPower;
                break;
            case CardType.Defense:
                from.currentHp += defensePower;
                break;
            case CardType.Heal:
                from.currentHp += healPower;
                break;
            // Buff, Debuff 등
        }
    }

}



public enum CardType { Attack, Defense, Heal, Buff, Debuff }
