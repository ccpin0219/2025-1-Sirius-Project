using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCardUI : MonoBehaviour
{
    public Image cardImage;
    public Image artImage;
    public TextMeshProUGUI cardNameText;

    private bool isSelected = false;
    private bool isFocused = false;
    private float liftHeight = 20f;
    private float baseY;
    private Character owner;

    private void Awake()
    {
        baseY = transform.localPosition.y;
    }

    public void SetCard(Skill skill, Character characterOwner)
    {
        owner = characterOwner;

        if (artImage != null && skill.artSprite != null)
            artImage.sprite = skill.artSprite;
    }


    public void SetFocus(bool focused)
    {
        isFocused = focused;
        ApplyVisuals();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        Vector3 pos = transform.localPosition;
        pos.y = isSelected ? baseY + liftHeight : baseY;
        transform.localPosition = pos;

        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        if (cardImage == null || owner == null) return;

        if (isFocused)
        {
            cardImage.color = Color.green; // 현재 커서 위치
        }
        else if (isSelected)
        {
            cardImage.color = Color.red; // 선택된 카드
        }
        else
        {
            if (owner.characterName == "전사")
                cardImage.color = new Color(1f, 1f, 1f, 0f);  // 노랑 투명
            else if (owner.characterName == "마법사")
                cardImage.color = new Color(1f, 1f, 1f, 0f); // 보라 투명
            else
                cardImage.color = new Color(1f, 1f, 1f, 0f); // 완전 투명
        }
    }
}
