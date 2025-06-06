// --- START OF FILE UnitBase.cs ---

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image 사용을 위해 네임스페이스가 이미 포함되어 있네요.

public class UnitBase : MonoBehaviour
{
    public CharacterData characterData; // 플레이어일 경우 참조
    public EnemyData enemyData;         // 적일 경우 참조

    public bool IsPlayer;
    public int currentHp;

    // --- 프로퍼티들 ---
    // (기존 프로퍼티들: UnitName, Portrait, Speed, MaxHp, CardList, IsDead - 변경 없음)
    public string UnitName => IsPlayer ?
                              (characterData != null ? characterData.characterName : "N/A_Player") :
                              (enemyData != null ? enemyData.EnemyName : "N/A_Enemy");

    public Sprite Portrait => IsPlayer ?
                              (characterData != null ? characterData.portrait : null) :
                              (enemyData != null ? enemyData.portrait : null);

    public int Speed => IsPlayer ?
                        (characterData != null ? characterData.speed : 0) :
                        (enemyData != null ? enemyData.speed : 0);

    public int MaxHp => IsPlayer ?
                        (characterData != null ? characterData.maxHp : 1) :
                        (enemyData != null ? enemyData.maxHp : 1);

    public List<CardData> CardList
    {
        get
        {
            if (IsPlayer)
            {
                return characterData != null ? characterData.defaultCards : null;
            }
            else
            {
                return null;
            }
        }
    }

    public bool IsDead => currentHp <= 0;


    // --- 타겟팅 관련 필드 ---
    private Image uiImage;          // 이 유닛의 포트레이트 등을 표시하는 UI Image 컴포넌트
    private Color originalUiColor;  // uiImage의 원래 색상
    private bool isTargeted = false; // 현재 이 유닛이 타겟으로 지정되었는지 여부

    // --- 메서드들 ---

    void Awake()
    {
        // uiImage 참조 찾기 (GetComponent는 자기 자신, GetComponentInChildren은 자식 포함)
        uiImage = GetComponent<Image>();
        if (uiImage == null)
        {
            // 예시: 자식 GameObject 중에 "PortraitDisplayImage"라는 이름의 Image를 찾을 수도 있습니다.
            // 또는 특정 태그를 가진 자식 Image를 찾을 수도 있습니다.
            // 여기서는 가장 일반적인 자식 중 첫 번째 Image를 찾는 방식으로 남겨둡니다.
            // 만약 특정 Image를 지정해야 한다면 public Image portraitImage; 로 만들고 Inspector에서 할당하는 것이 더 좋습니다.
            uiImage = GetComponentInChildren<Image>();
        }

        if (uiImage != null)
        {
            originalUiColor = uiImage.color; // 초기 색상 저장
            // 초기 포트레이트 설정 (만약 uiImage의 sprite가 비어있다면)
            if (uiImage.sprite == null && Portrait != null)
            {
                uiImage.sprite = Portrait;
            }
        }
        else
        {
            Debug.LogWarning($"Unit ({gameObject.name}) has no UI Image component found (self or children). Targeting color change will not work for UI.", this);
        }
    }

    public void InitializeHp()
    {
        if (IsPlayer && characterData == null)
        {
            Debug.LogError($"Player Unit ({gameObject.name}) has no CharacterData assigned. Cannot initialize HP.", this);
            currentHp = 1;
            return;
        }
        if (!IsPlayer && enemyData == null)
        {
            Debug.LogError($"Enemy Unit ({gameObject.name}) has no EnemyData assigned. Cannot initialize HP.", this);
            currentHp = 1;
            return;
        }
        currentHp = MaxHp;
        Debug.Log($"[UnitBase] {UnitName} HP initialized to: {currentHp}/{MaxHp}", this);
    }

    // 타겟으로 지정되거나 해제될 때 호출될 메서드
    public void SetTargeted(bool targeted, Color highlightColor)
    {
        if (uiImage == null) // uiImage가 없으면 아무것도 하지 않음
        {
            // Debug.LogWarning($"[UnitBase] SetTargeted called on {UnitName}, but uiImage is null.", this);
            return;
        }
        if (IsDead && targeted) // 죽은 유닛은 타겟팅될 수 없음 (하이라이트 안 함)
        {
            Debug.Log($"[UnitBase] Cannot target dead unit: {UnitName}", this);
            return;
        }

        isTargeted = targeted;

        if (isTargeted)
        {
            uiImage.color = highlightColor; // 하이라이트 색상으로 변경
            // Debug.Log($"[UnitBase] {UnitName} is now targeted. Color: {highlightColor}", this);
        }
        else
        {
            uiImage.color = originalUiColor; // 원래 색상으로 복원
            // Debug.Log($"[UnitBase] {UnitName} is no longer targeted. Color restored to: {originalUiColor}", this);
        }
    }


    public virtual void TakeDamage(int damage)
    {
        if (IsDead) return;

        int defense = 0;
        if (IsPlayer && characterData != null) defense = characterData.defense;
        else if (!IsPlayer && enemyData != null) defense = enemyData.defense;

        int actualDamage = Mathf.Max(0, damage - defense);
        currentHp -= actualDamage;

        Debug.Log($"{UnitName} takes {actualDamage} damage (Original: {damage}, Defense: {defense}). Current HP: {currentHp}/{MaxHp}", this);

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log($"{UnitName} has died.", this);
        // 사망 시 타겟팅 효과도 해제 (원래 색상으로)
        if (isTargeted && uiImage != null)
        {
            uiImage.color = originalUiColor; // 또는 사망 시 특별한 색 (예: 회색)
            isTargeted = false; // 타겟팅 상태도 해제
        }
        // 추가적인 사망 처리 로직
    }
}
