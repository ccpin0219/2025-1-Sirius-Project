using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Init(CharacterStats stats)
    {
        base.Init(stats);
        Debug.Log($"{characterName} 초기화 완료 (적)");
    }

    public void PerformAction()
    {
        StartCoroutine(PerformWithAnimation());
    }

    private IEnumerator PerformWithAnimation()
    {
        SetAttack();  // ✅ 공격 스프라이트 표시
        yield return new WaitForSeconds(0.5f);  // ✅ 공격 연출 대기

        List<Character> playerCharacters = new List<Character>
        {
            GameManager.Instance.spawner.warriorObj.GetComponent<Character>(),
            GameManager.Instance.spawner.mageObj.GetComponent<Character>()
        };

        Character target = ChooseTarget(playerCharacters);
        float hpRatio = (float)hp / 50f;

        if (hpRatio > 0.5f)
        {
            int roll = Random.Range(0, 2);
            if (roll == 0) Bite(target);
            else Howl(playerCharacters);
        }
        else
        {
            int roll = Random.Range(0, 2);
            if (roll == 0) Bite(target);
            else BloodChase(target);
        }

        yield return new WaitForSeconds(0.2f);  // idle로 돌아가기 전에 잠깐 대기
        SetIdle();  // 다시 서 있는 상태로
    }

    Character ChooseTarget(List<Character> players)
    {
        Character warrior = players.Find(p => p.characterName == "전사");
        Character mage = players.Find(p => p.characterName == "마법사");

        int roll = Random.Range(0, 100);
        if (roll < 60 && warrior != null) return warrior;
        if (mage != null) return mage;
        return warrior ?? players[0];
    }

    void Bite(Character target)
    {
        Debug.Log($"{characterName}이 {target.characterName}을 물어뜯습니다! (8 피해)");
        target.TakeDamage(8, characterName);
    }

    void Howl(List<Character> targets)
    {
        Debug.Log($"{characterName}이 울부짖습니다! (모든 아군에게 5 피해)");
        foreach (Character t in targets)
        {
            t.TakeDamage(5, characterName);
        }
    }

    void BloodChase(Character target)
    {
        Debug.Log($"{characterName}이 {target.characterName}을 피의 추격으로 공격합니다! (6 피해 ×2)");
        target.TakeDamage(6, characterName);
        target.TakeDamage(6, characterName);
    }
}

