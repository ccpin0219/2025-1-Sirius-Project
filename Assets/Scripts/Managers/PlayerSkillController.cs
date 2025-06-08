using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public SkillCardUI[] cardUIs;       // 4개의 카드 UI
    private Character activeCharacter;  // 현재 턴 캐릭터
    private List<Skill> selectedSkills = new();
    private int selectedIndex = 0;

    private void Update()
    {
        if (activeCharacter == null || activeCharacter.skills == null || activeCharacter.skills.Count == 0)
            return;

        HandleInput();
    }

    private void HandleInput()
    {
        bool moved = false;

        if (cardUIs == null || activeCharacter == null || activeCharacter.skills == null)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex + cardUIs.Length - 1) % cardUIs.Length;
            moved = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % cardUIs.Length;
            moved = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedIndex >= activeCharacter.skills.Count)
            {
                Debug.LogWarning($"선택 인덱스 {selectedIndex}가 스킬 개수보다 큽니다 ({activeCharacter.skills.Count})");
                return;
            }

            Skill selectedSkill = activeCharacter.skills[selectedIndex];

            if (selectedSkills.Contains(selectedSkill))
                selectedSkills.Remove(selectedSkill);
            else
                selectedSkills.Add(selectedSkill);

            Debug.Log($"[카드 선택] {selectedSkill.skillName} {(selectedSkills.Contains(selectedSkill) ? "선택됨" : "해제됨")}");
            UpdateCardUI();
        }

        if (moved)
        {
            UpdateCardUI();
        }
    }


    private void UpdateCardUI()
    {
        if (activeCharacter == null || cardUIs == null)
            return;

        for (int i = 0; i < cardUIs.Length; i++)
        {
            if (i < activeCharacter.skills.Count && cardUIs[i] != null)
            {
                Skill skill = activeCharacter.skills[i];
                cardUIs[i].SetCard(skill, activeCharacter);

                bool isFocused = (i == selectedIndex);
                bool isSelected = selectedSkills.Contains(skill);

                cardUIs[i].SetFocus(isFocused);
                cardUIs[i].SetSelected(isSelected);
            }
            else if (cardUIs[i] != null)
            {
                // 스킬이 없는 슬롯은 비우기
                cardUIs[i].SetCard(null, null);
                cardUIs[i].SetFocus(false);
                cardUIs[i].SetSelected(false);
            }
        }
    }

    public void SetActiveCharacter(Character character)
    {
        activeCharacter = character;
        selectedIndex = 0;
        selectedSkills.Clear();
        UpdateCardUI();
    }

    public List<Skill> GetSelectedSkills() => selectedSkills;

    public Character GetActiveCharacter() => activeCharacter;

    public void ClearSelectedSkills()
    {
        selectedSkills.Clear();
        UpdateCardUI();
    }
}
