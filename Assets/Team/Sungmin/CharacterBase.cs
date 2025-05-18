using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 캐릭터의 기본 데이터를 담는 추상 클래스
/// </summary>
public abstract class CharacterBase : MonoBehaviour
{
    [Header("기본 능력치")]
    public string characterName = "캐릭터 이름";

    public int maxHP = 50;
    public int currentHP = 50;

    [Header("자원")]
    public int attackPoint = 0;   // 공격 자원
    public int defensePoint = 0;  // 방어 자원

    [Header("전투 능력치")]
    public int speed = 10;
    public int attackPower = 0;
    public int defensePower = 0;

    [Header("전투 상태")]
    public int barrier = 0; // 장벽 수치 (슬더슬의 보호와 동일)

    [Header("스킬 슬롯")]
    public List<Skill> skills = new List<Skill>(4);

    /// <summary>
    /// 캐릭터의 초기화를 담당합니다.
    /// </summary>
    public virtual void Init()
    {
        currentHP = maxHP;
        barrier = 0;
    }

    /// <summary>
    /// 피해를 입습니다. 장벽이 있을 경우 장벽부터 피해를 흡수합니다.
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        if (barrier > 0)
        {
            int absorbed = Mathf.Min(barrier, remainingDamage);
            barrier -= absorbed;
            remainingDamage -= absorbed;
            Debug.Log($"{characterName}의 장벽이 {absorbed} 피해를 막았습니다. 남은 장벽: {barrier}");
        }

        if (remainingDamage > 0)
        {
            currentHP -= remainingDamage;
            Debug.Log($"{characterName}이(가) {remainingDamage}의 피해를 입었습니다. 현재 HP: {currentHP}");
        }

        if (currentHP <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// 장벽을 생성합니다. (defensePower만큼 증가)
    /// </summary>
    public virtual void GainBarrier()
    {
        barrier += defensePower;
        Debug.Log($"{characterName}이(가) 방어력 {defensePower}만큼 장벽을 생성했습니다. 현재 장벽: {barrier}");
    }

    /// <summary>
    /// 턴 종료 시 장벽 초기화
    /// </summary>
    public virtual void ResetBarrier()
    {
        barrier = 0;
        Debug.Log($"{characterName}의 장벽이 초기화되었습니다.");
    }

    /// <summary>
    /// 캐릭터 사망 처리
    /// </summary>
    protected virtual void OnDeath()
    {
        Debug.Log($"{characterName}이(가) 사망했습니다.");
        // 필요 시 여기서 비활성화, 제거 등 처리
    }
}

/// <summary>
/// 스킬 타입 열거형
/// </summary>
public enum SkillType
{
    Attack,   // 공격형 스킬
    Defense,  // 방어형 스킬 (장벽 생성 등)
    Special   // 특수 스킬 (에너지 회복, 버프 등)
}

/// <summary>
/// 스킬 정보를 담는 클래스
/// </summary>
[System.Serializable]
public class Skill
{
    public string skillName;
    public string description;
    public SkillType type;     // 스킬 타입
    public int value;          // 수치 값 (공격력, 방어력, 효과량 등)
    public int cooldown;       // 쿨다운
    public int cost;           // 자원 소비량
}
