// --- START OF FILE CardSelector.cs ---
using UnityEngine;
using UnityEngine.UI; // Image, Text 등 UI 요소를 직접 다룬다면 필요
using System.Collections.Generic;

public class CardSelector : MonoBehaviour
{
    public TargetSelector targetSelector;   // 타겟 선택 UI (필요하다면 연결)
    public GameObject cardPrefab;           // 카드 슬롯 프리팹 (루트 GameObject)
    public Transform cardParent;            // 카드들이 붙는 패널 (이 Transform의 자식으로 카드 슬롯이 생성됨)

    [HideInInspector]
    public List<CardData> cardDataList = new List<CardData>(); // 현재 턴의 카드 데이터 리스트

    // CardView 대신 생성된 카드 슬롯 GameObject 자체를 저장
    List<GameObject> cardSlotObjects = new List<GameObject>();
    int currentIndex = 0;

    // 카드 선택 확정 콜백 (선택된 카드 데이터, 선택된 타겟 유닛)
    System.Action<CardData, UnitBase> onCardChosenCallback;

    void Start()
    {
        // 처음엔 꺼두기(플레이어 턴일 때만 활성) - 현재는 주석 처리되어 BattleManager가 제어
        // gameObject.SetActive(false);
        // TargetSelector가 있다면 초기에는 비활성화
        if (targetSelector != null)
        {
            targetSelector.gameObject.SetActive(false);
        }
    }

    // 외부(BattleManager)에서 카드 리스트와 콜백을 등록
    public void SetCards(List<CardData> newCards, System.Action<CardData, UnitBase> onSelected)
    {
        cardDataList = newCards ?? new List<CardData>(); // null일 경우 빈 리스트로 초기화
        onCardChosenCallback = onSelected;

        GenerateCards(cardDataList.Count);
        currentIndex = 0;
        UpdateSelectionVisuals(); // 이름 변경: UpdateSelection -> UpdateSelectionVisuals (선택 시각화 업데이트 명시)
        gameObject.SetActive(true); // CardSelector 패널 자체를 활성화
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) // activeSelf 대신 activeInHierarchy로 확인 (부모가 꺼져있을 수도 있으므로)
        {
            // Debug.LogWarning("[CardSelector] Update: Not active in hierarchy. Frame: " + Time.frameCount); // 필요시 로그 활성화
            return;
        }
        if (cardSlotObjects.Count == 0)
        {
            // Debug.LogWarning("[CardSelector] Update: No card slots available. Frame: " + Time.frameCount); // 필요시 로그 활성화
            return;
        }

        // 좌/우 키로 카드 선택 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateSelectionVisuals();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(cardSlotObjects.Count - 1, currentIndex + 1);
            UpdateSelectionVisuals();
        }

        // 엔터 키로 카드 선택 및 타겟 지정 시작
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) // 엔터키 (일반/숫자패드)
        {
            if (cardSlotObjects.Count > 0 && currentIndex >= 0 && currentIndex < cardSlotObjects.Count)
            {
                Debug.Log($"[CardSelector] Enter pressed. Selected card index: {currentIndex}. Card: {cardDataList[currentIndex].cardName}", this);
                if (targetSelector != null)
                {
                    // 예시: 아군이면 적군만 리스트업 (BattleManager에 의존)
                    List<UnitBase> targetList = BattleManager.Instance.GetTargetList();
                    targetSelector.SetTargets(targetList, OnTargetChosenBySelector); // 콜백 이름 변경
                    targetSelector.gameObject.SetActive(true); // 타겟 선택 UI 활성화
                    gameObject.SetActive(false); // 카드 선택 패널은 숨김
                }
                else
                {
                    // TargetSelector가 없으면, 현재 선택된 카드와 null 타겟(또는 기본 타겟)으로 즉시 처리할 수도 있음
                    Debug.LogWarning("[CardSelector] TargetSelector is not assigned. Proceeding without target selection.", this);
                    OnTargetChosenBySelector(null); // 타겟 없이 즉시 콜백 (게임 로직에 따라 수정 필요)
                }
            }
            else
            {
                Debug.LogWarning("[CardSelector] Enter pressed but no valid card selected.", this);
            }
        }
    }

    // TargetSelector로부터 타겟이 선택되었을 때 호출되는 콜백
    void OnTargetChosenBySelector(UnitBase target)
    {
        // 선택된 카드와 타겟을 BattleManager로 전달
        if (currentIndex >= 0 && currentIndex < cardDataList.Count)
        {
            onCardChosenCallback?.Invoke(cardDataList[currentIndex], target);
        }
        else
        {
            Debug.LogError($"[CardSelector] Invalid currentIndex ({currentIndex}) when target was chosen. cardDataList count: {cardDataList.Count}", this);
        }
        // TargetSelector를 사용했다면, 작업 완료 후 비활성화 할 수 있음
        if (targetSelector != null)
        {
            targetSelector.gameObject.SetActive(false);
        }
        // 이 시점에서 CardSelector 자체를 다시 숨길지 여부는 게임 흐름에 따름
        // gameObject.SetActive(false); // 이미 Enter 키 처리에서 숨겼음
    }

    void GenerateCards(int count)
    {
        ClearGeneratedCards(); // 기존 카드 슬롯 제거 로직 분리

        for (int i = 0; i < count; i++)
        {
            GameObject slotInstance = Instantiate(cardPrefab, cardParent);
            slotInstance.name = $"CardSlot_{i}_{cardDataList[i].cardName}"; // 디버깅 용이하게 이름 설정
            CardView view = slotInstance.GetComponentInChildren<CardView>();

            if (view == null)
            {
                Destroy(slotInstance);
                continue;
            }
            if (cardDataList == null || i >= cardDataList.Count || cardDataList[i] == null)
            {
                 Destroy(slotInstance);
                 continue;
            }

            view.SetCard(cardDataList[i]);
            cardSlotObjects.Add(slotInstance); // 생성된 "슬롯 GameObject"를 리스트에 추가
        }
    }

    // 선택된 카드의 시각적 표시 업데이트
    void UpdateSelectionVisuals()
    {
        if (cardSlotObjects.Count == 0) return;

        for (int i = 0; i < cardSlotObjects.Count; i++)
        {
            if (cardSlotObjects[i] != null)
            {
                CardView view = cardSlotObjects[i].GetComponentInChildren<CardView>();
                if (view != null)
                {
                    view.SetSelected(i == currentIndex);
                }
                else
                {
                    Debug.LogWarning($"[CardSelector] CardView not found on card slot object: {cardSlotObjects[i].name} during UpdateSelectionVisuals.", cardSlotObjects[i]);
                }
            }
        }
    }

    // 외부(UI 버튼 등)에서 직접 카드 선택 및 타겟팅 시작을 위해 호출될 수 있음 (현재 미사용)
    // 이 함수는 BattleManager의 OnPlayerCardSelected와 역할이 겹칠 수 있어 주의 필요
    public void OnConfirmButtonClick(UnitBase selectedTarget)
    {
        Debug.LogError($"[CardSelector] OnConfirmButtonClick called with target: {(selectedTarget != null ? selectedTarget.UnitName : "null")}. This function might be deprecated or needs review.", this);
        if (onCardChosenCallback != null && currentIndex >= 0 && currentIndex < cardDataList.Count)
        {
            onCardChosenCallback(cardDataList[currentIndex], selectedTarget);
            gameObject.SetActive(false); // 선택 끝나면 숨김
        }
        else
        {
             Debug.LogError($"[CardSelector] OnConfirmButtonClick: Cannot confirm. Callback null or index out of bounds. Index: {currentIndex}, CardDataCount: {cardDataList.Count}", this);
        }
    }

    // UI에서 특정 카드를 직접 클릭하여 선택하는 기능 (선택 사항)
    public void SelectCardByIndex(int index)
    {
        if (index >= 0 && index < cardSlotObjects.Count)
        {
            currentIndex = index;
            UpdateSelectionVisuals();
            Debug.Log($"[CardSelector] Card selected by index: {index}", this);
            // 여기서 바로 타겟 선택으로 넘어갈 수도 있음
            // if (Input.GetKeyDown(KeyCode.Return)) ... 와 유사한 로직 수행
        }
        else
        {
            Debug.LogWarning($"[CardSelector] SelectCardByIndex: Invalid index {index}. Total slots: {cardSlotObjects.Count}", this);
        }
    }

    // CardSelector가 비활성화될 때
    void OnDisable()
    {
        // 필요하다면, 비활성화될 때 자식 카드들도 모두 정리할 수 있습니다.
        // ClearGeneratedCards(); // 만약 BattleManager의 Hide()와 중복 호출될 수 있다면 주의
    }

    // CardSelector가 활성화될 때
    void OnEnable()
    {
        // 활성화될 때 선택 상태를 다시 업데이트하거나 초기화할 수 있음
        if(cardSlotObjects.Count > 0)
        {
            UpdateSelectionVisuals();
        }
    }

    // 카드 슬롯 오브젝트들을 정리하고, CardSelector 패널 자체를 숨김
    public void Hide()
    {
        ClearGeneratedCards();
        gameObject.SetActive(false);
        if (targetSelector != null && targetSelector.gameObject.activeSelf)
        {
            targetSelector.gameObject.SetActive(false); // 타겟 선택 UI도 확실히 숨김
        }
    }

    // 생성된 카드 슬롯 GameObject들만 제거
    public void ClearGeneratedCards()
    {
        foreach (var slotGO in cardSlotObjects)
        {
            if (slotGO != null) // 파괴되기 전에 null 체크
            {
                Destroy(slotGO);
            }
        }
        cardSlotObjects.Clear(); // 리스트 비우기
    }
}
