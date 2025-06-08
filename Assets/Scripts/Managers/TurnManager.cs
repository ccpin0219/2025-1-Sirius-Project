using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> turnOrder = new();
    private int turnIndex = 0;

    public Character currentCharacter;

    public void InitTurnOrder(List<Character> allCharacters)
    {
        turnOrder = allCharacters.OrderByDescending(c => c.speed).ToList();
        turnIndex = 0;
        StartTurn();
    }

    public void StartTurn()
    {
        if (turnOrder.Count == 0)
        {
            Debug.LogWarning("턴 순서가 비어 있습니다.");
            return;
        }

        currentCharacter = turnOrder[turnIndex];
        Debug.Log($"턴 시작: {currentCharacter.characterName}");

        SetActiveCharacter(currentCharacter);

        if (currentCharacter is Enemy enemy)
        {
            StartCoroutine(EnemyActionWithDelay(enemy));
        }
    }

    public void EndPlayerTurn()
    {
        Debug.Log("턴 종료 버튼 눌림");

        ExecutePlayerSkills();

        // 딜레이 후 다음 턴으로 넘어감
        StartCoroutine(DelayedStartNextTurn());
    }

    public void NextTurn()
    {
        // 일반적인 직접 호출 대신 Coroutine으로 분기
        StartCoroutine(DelayedStartNextTurn());
    }

    private IEnumerator DelayedStartNextTurn()
    {
        yield return new WaitForSeconds(1.5f); // 1.5초 대기
        turnIndex = (turnIndex + 1) % turnOrder.Count;
        StartTurn();
    }

    private IEnumerator EnemyActionWithDelay(Enemy enemy)
    {
        yield return new WaitForSeconds(1.5f); // 적 턴 시작 전 1.5초 대기
        enemy.PerformAction();

        yield return new WaitForSeconds(1.5f); // 적 턴 끝나고 1.5초 대기
        NextTurn();
    }


    public void SetActiveCharacter(Character character)
    {
        currentCharacter = character;

        if (character.CompareTag("Player"))
        {
            GameManager.Instance.playerSkillController.SetActiveCharacter(character);
        }
    }

    private void ExecutePlayerSkills()
    {
        var skillController = GameManager.Instance.playerSkillController;
        var skills = skillController.GetSelectedSkills();
        var user = skillController.GetActiveCharacter();

        Debug.Log($"[스킬 발동] {user.characterName}의 스킬 {skills.Count}개 실행");

        foreach (var skill in skills)
        {
            if (skill.isAOE)
            {
                if (skill.isAttack)
                {
                    // 전체 공격: 적 전체
                    skill.Use(GameManager.Instance.enemies.Cast<Character>().ToList());
                }
                else
                {
                    // 전체 방어: 아군 전체
                    List<Character> allies = new()
                    {
                        GameManager.Instance.spawner.warriorObj.GetComponent<Character>(),
                        GameManager.Instance.spawner.mageObj.GetComponent<Character>()
                    };
                    skill.Use(allies);
                }
            }
            else
            {
                // 단일 대상
                Character target = skill.isAttack
                    ? GameManager.Instance.enemies[0]
                    : user;

                skill.Use(target);
            }
        }

        skillController.ClearSelectedSkills();
    }
}
