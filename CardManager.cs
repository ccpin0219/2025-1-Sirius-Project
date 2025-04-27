using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    private List<CardData> drawPile = new();
    private List<CardData> discardPile = new();
    private List<CardData> hand = new();

    private Player player;
    private List<Enemy> enemies;
    private DeckPreset startingDeckPreset;

    public void SetPlayer(Player p) => player = p;
    public void SetEnemies(List<Enemy> e) => enemies = e;
    public void SetDeck(DeckPreset preset) => startingDeckPreset = preset;
    public Transform handArea;
    public GameObject cardUIPrefab;
    private List<CardUI> cardUIs = new List<CardUI>();

    public void Initialize()
    {
        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();

        foreach (DeckPreset.CardEntry entry in startingDeckPreset.cards)
        {
            for (int i = 0; i < entry.count; i++)
            {
                drawPile.Add(Instantiate(entry.card));
            }
        }

        ShuffleDeck(drawPile);
        Debug.Log("📦 덱 초기화 완료");
    }

    public void RefillHand()
    {
        int drawCount = 5;
        DrawCards(drawCount);
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawPile.Count == 0)
            {
                if (discardPile.Count > 0)
                {
                    Debug.Log("<color=yellow>🔄 덱 셔플 발생!</color>");
                    drawPile.AddRange(discardPile);
                    discardPile.Clear();
                    ShuffleDeck(drawPile);
                }
                else
                {
                    Debug.Log("❗ 카드 더미가 모두 비어 있음");
                    return;
                }
            }

            CardData drawn = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(drawn);

            Debug.Log($"🃏 카드 드로우: {drawn.cardName}");

            GameObject cardUIObj = Instantiate(cardUIPrefab, handArea);
            CardUI ui = cardUIObj.GetComponent<CardUI>();
            ui.SetCardData(drawn);
            cardUIs.Add(ui); // 카드 UI 리스트에 추가
        }

        RepositionCards(); // 드로우 후 재배치
    }

// DiscardHand()
    public void DiscardHand()
    {
        if (hand.Count == 0) return;

        Debug.Log($"🗑️ 카드 {hand.Count}장 버림: [{string.Join(", ", hand.Select(c => c.cardName))}]");

        foreach (CardUI ui in cardUIs)
        {
            if (ui != null)
                ui.PlayDiscardAnimation();
        }

        discardPile.AddRange(hand);
        hand.Clear();
        cardUIs.Clear(); // 중요: 리스트 정리
    }

    public void ShuffleDeck(List<CardData> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]);
        }

        Debug.Log("<color=yellow>🌀 덱 셔플 완료</color>");
    }

    // RepositionCards()
    private void RepositionCards()
    {
        Debug.Log($"[CardManager] 재배치 시작. 카드 수: {cardUIs.Count}");
        int cardCount = cardUIs.Count;
        if (cardCount == 0) return;

        float maxWidth = 1300f; // 펼칠 수 있는 최대 길이
        float spacing = Mathf.Min(130f, maxWidth / cardCount); // spacing은 너무 좁아지지 않도록 제한

        float startX = -(spacing * (cardCount - 1)) / 2f; // 중앙 기준으로 시작 위치 계산

        for (int i = 0; i < cardCount; i++)
        {
            Vector2 targetPos = new Vector2(startX + i * spacing, -85f);
            Debug.Log($"[CardManager] 카드 {i} 위치: {targetPos}");

            cardUIs[i].SetOriginalPosition();     // (위치 기억)
            cardUIs[i].MoveToPosition(targetPos); // (이동 명령)
        }
    }
}
