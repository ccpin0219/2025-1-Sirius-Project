using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;
    public Image artworkImage;

    // 선택 연출 (레이아웃그룹 때문에 anchoredPosition 사용)
    public void SetSelected(bool selected)
    {
        // 20만큼 위로 튀어오르기
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = selected ? new Vector2(0, 20f) : Vector2.zero;
    }

    public void SetCard(CardData data)
    {
        cardNameText.text = data.cardName;
        descriptionText.text = data.description;
        artworkImage.sprite = data.artwork;
    }
}
