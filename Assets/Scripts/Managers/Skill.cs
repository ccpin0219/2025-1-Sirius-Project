using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName;
    public bool isAttack;
    public bool isAOE;
    public int power;

    // 카드에 표시할 이미지
    public Sprite artSprite;

    public void Use(List<Character> targets)
    {
        Character user = GameManager.Instance.turnManager.currentCharacter;
        user.SetAttack();

        foreach (var t in targets)
        {
            if (isAttack)
            {
                t.SetHit();
                t.TakeDamage(power, user.characterName);
            }
            else
            {
                t.AddArmor(power);
            }
        }

        GameManager.Instance.StartCoroutine(ResetSpritesAfterDelay(user, targets));
    }

    public void Use(Character target)
    {
        Character user = GameManager.Instance.turnManager.currentCharacter;
        user.SetAttack();

        if (isAttack)
        {
            target.SetHit();
            target.TakeDamage(power, user.characterName);
        }
        else
        {
            target.AddArmor(power);
        }

        GameManager.Instance.StartCoroutine(ResetSpritesAfterDelay(user, new List<Character> { target }));
    }


    private IEnumerator ResetSpritesAfterDelay(Character user, List<Character> targets)
    {
        yield return new WaitForSeconds(0.5f);

        user.SetIdle();
        foreach (var t in targets)
        {
            t.SetIdle();
        }
    }
}
