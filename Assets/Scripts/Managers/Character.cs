using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public string characterName;
    public int hp;
    public int armor;
    public int speed => stats.speed; // stats에서 가져옴

    public CharacterStats stats;
    public List<Skill> skills = new();

    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite hitSprite;

    protected SpriteRenderer spriteRenderer;

    public virtual void Init(CharacterStats stats)
    {
        this.stats = stats;                 // stats 자체를 연결
        characterName = stats.characterName;
        hp = stats.maxHP;
        armor = 0;
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void TakeDamage(int amount, string attackerName)
    {
        SetHit();

        int effectiveDamage = Mathf.Max(amount - armor, 0);
        armor = Mathf.Max(armor - amount, 0);
        hp -= effectiveDamage;

        Debug.Log($"{attackerName}의 공격! → {characterName}이(가) {effectiveDamage} 피해를 입었습니다. " +
                  $"[남은 체력: {hp}, 남은 방어도: {armor}]");

        StartCoroutine(BackToIdleAfterDelay());
    }



    private System.Collections.IEnumerator BackToIdleAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SetIdle();
    }

    public virtual void AddArmor(int amount)
    {
        armor += amount;
        Debug.Log($"{characterName}이(가) {amount} 방어도를 얻었습니다. [남은 체력: {hp}, 방어도: {armor}]");
    }


    public void SetIdle()
    {
        if (spriteRenderer && idleSprite)
            spriteRenderer.sprite = idleSprite;
    }

    public void SetAttack()
    {
        if (spriteRenderer && attackSprite)
            spriteRenderer.sprite = attackSprite;
    }

    public void SetHit()
    {
        if (spriteRenderer && hitSprite)
            spriteRenderer.sprite = hitSprite;
    }
}
