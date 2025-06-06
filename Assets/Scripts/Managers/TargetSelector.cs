using UnityEngine;
using UnityEngine.UI; // Image 색상 변경 시 필요할 수 있으나, UnitBase에서 직접 처리
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour
{
    public List<UnitBase> targetList = new List<UnitBase>();
    int currentTargetIndex = 0; // 변수명을 currentTarget에서 currentTargetIndex로 변경 (인덱스임을 명확히)
    System.Action<UnitBase> onTargetChosenCallback; // 변수명을 onTargetChosen에서 onTargetChosenCallback으로 변경 (콜백임을 명확히)

    [Header("Targeting Visuals")]
    public Color playerHighlightColor = new Color(1f, 1f, 0.7f, 1f); // 예: 밝은 노란색 (플레이어 타겟 시)
    public Color enemyHighlightColor = new Color(1f, 0.7f, 0.7f, 1f);  // 예: 밝은 빨간색 (적 타겟 시)


    private UnitBase previouslySelectedUnit = null; // 이전에 선택되었던 유닛을 추적하기 위함

    // 외부(CardSelector)에서 호출: 타겟 후보 리스트와 선택 완료 시 실행될 콜백을 받음
    public void SetTargets(List<UnitBase> targets, System.Action<UnitBase> onChosen)
    {
        // 이전 선택 상태 초기화
        if (previouslySelectedUnit != null)
        {
            previouslySelectedUnit.SetTargeted(false, GetHighlightColor(previouslySelectedUnit)); // 이전 하이라이트 제거
            previouslySelectedUnit = null;
        }

        targetList = targets ?? new List<UnitBase>(); // null일 경우 빈 리스트로
        onTargetChosenCallback = onChosen;

        if (targetList.Count == 0)
        {
            Debug.LogWarning("[TargetSelector] SetTargets: Provided target list is empty. Closing target selector.", this);
            gameObject.SetActive(false); // 타겟이 없으면 바로 닫음
            return;
        }

        currentTargetIndex = 0;       // 첫 번째 타겟을 기본으로 선택
        gameObject.SetActive(true);   // 타겟 선택 UI 활성화
        UpdateTargetVisuals();        // 초기 선택 타겟 시각적 업데이트
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return; // activeSelf 대신 activeInHierarchy 사용 권장
        if (targetList.Count == 0) return;

        bool selectionChanged = false;

        // 좌/우 타겟 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentTargetIndex > 0) // 현재가 첫번째 타겟이 아닐 때만 이동
            {
                currentTargetIndex--;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentTargetIndex < targetList.Count - 1) // 현재가 마지막 타겟이 아닐 때만 이동
            {
                currentTargetIndex++;
                selectionChanged = true;
            }
        }

        if (selectionChanged)
        {
            UpdateTargetVisuals(); // 선택이 변경되었으면 시각적 효과 업데이트
        }

        // 엔터 누르면 선택 완료
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (currentTargetIndex >= 0 && currentTargetIndex < targetList.Count)
            {
                UnitBase chosenUnit = targetList[currentTargetIndex];
                if (chosenUnit != null && !chosenUnit.IsDead) // 살아있는 유닛인지 한번 더 확인
                {
                    Debug.Log($"[TargetSelector] Target chosen: {chosenUnit.UnitName}", this);
                    onTargetChosenCallback?.Invoke(chosenUnit);

                    // 선택 완료 후, 현재 선택된 타겟의 하이라이트는 유지할 수도 있고, 끌 수도 있습니다.
                    // 여기서는 끄는 것으로 가정 (턴이 넘어가거나 할 때 정리되도록)
                    // chosenUnit.SetTargeted(false, GetHighlightColor(chosenUnit));
                    // previouslySelectedUnit = null; // 다음 SetTargets 호출 시 정리되므로 여기서 꼭 필요하진 않음
                }
                else
                {
                    Debug.LogWarning($"[TargetSelector] Chosen target is null or dead: Index {currentTargetIndex}", this);
                }
            }
            else
            {
                Debug.LogError($"[TargetSelector] currentTargetIndex ({currentTargetIndex}) is out of bounds for targetList (Count: {targetList.Count})", this);
            }
            gameObject.SetActive(false); // 타겟 선택 창 닫기 (성공/실패 여부와 관계없이)
        }

        // ESC 키 등으로 타겟 선택 취소
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[TargetSelector] Target selection cancelled by ESC.", this);
            // onTargetChosenCallback?.Invoke(null); // 취소 시 null을 전달할 수도 있음 (선택사항)
            gameObject.SetActive(false); // 타겟 선택 창 닫기
        }
    }

    // 선택된 타겟의 시각적 표시 업데이트 (이름 변경: UpdateSelection -> UpdateTargetVisuals)
    void UpdateTargetVisuals()
    {
        if (targetList.Count == 0) return;

        // 1. 이전에 선택되었던 유닛의 하이라이트 해제
        if (previouslySelectedUnit != null)
        {
            // previouslySelectedUnit이 여전히 targetList에 있는지, 또는 다른 이유로 null이 될 수 있는지 확인
            // 여기서는 간단히 SetTargeted(false) 호출
            previouslySelectedUnit.SetTargeted(false, GetHighlightColor(previouslySelectedUnit));
        }

        // 2. 현재 선택된 유닛(currentTargetIndex)에 하이라이트 적용
        if (currentTargetIndex >= 0 && currentTargetIndex < targetList.Count)
        {
            UnitBase currentUnitToHighlight = targetList[currentTargetIndex];
            if (currentUnitToHighlight != null && !currentUnitToHighlight.IsDead) // 살아있는 유닛만 하이라이트
            {
                currentUnitToHighlight.SetTargeted(true, GetHighlightColor(currentUnitToHighlight));
                previouslySelectedUnit = currentUnitToHighlight; // 현재 하이라이트된 유닛을 기억
                // Debug.Log($"[TargetSelector] Highlighting target: {currentUnitToHighlight.UnitName}", this);
            }
            else
            {
                // 현재 인덱스의 유닛이 null이거나 죽었으면, 이전에 선택된 유닛도 null로 처리하여 하이라이트 없도록 함
                previouslySelectedUnit = null;
            }
        }
        else
        {
            Debug.LogError($"[TargetSelector] UpdateTargetVisuals: currentTargetIndex ({currentTargetIndex}) is out of bounds.", this);
            previouslySelectedUnit = null; // 안전을 위해
        }
    }

    // 유닛 타입(플레이어/적)에 따라 적절한 하이라이트 색상 반환
    Color GetHighlightColor(UnitBase unit)
    {
        if (unit == null) return Color.clear; // 또는 기본색
        return unit.IsPlayer ? playerHighlightColor : enemyHighlightColor;
        // 또는 return defaultHighlightColor; // 단일 하이라이트 색상 사용 시
    }

    // TargetSelector GameObject가 비활성화될 때 호출됨
    void OnDisable()
    {
        // 타겟 선택 UI가 닫힐 때, 마지막으로 하이라이트된 유닛의 효과를 제거
        if (previouslySelectedUnit != null)
        {
            previouslySelectedUnit.SetTargeted(false, GetHighlightColor(previouslySelectedUnit));
            previouslySelectedUnit = null; // 상태 초기화
        }
        // targetList = new List<UnitBase>(); // 필요하다면 리스트도 초기화 (다음 SetTargets에서 어차피 새로 받음)
        Debug.Log("[TargetSelector] Disabled. All target highlights cleared.", this);
    }
}