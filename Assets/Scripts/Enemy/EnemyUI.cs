using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public GameObject enemyPrefab;             // 적 프리팹
    public Transform enemyPanel;               // 적 유닛이 붙을 부모 (보통 UI Panel 등)
    public List<EnemyData> enemyCharacters;    // 등장할 적 데이터 리스트 (Inspector에서 할당)

    void Start() // 예시로 Start에서 호출, 실제로는 BattleManager 등에서 호출될 수 있음
    {
        if (enemyPanel == null)
        {
            Debug.LogError("[EnemyUI] EnemyPanel이 할당되지 않았습니다!", this);
            enemyPanel = this.transform; // 할당 안됐으면 자기 자신을 부모로 사용 (임시방편)
            Debug.LogWarning("[EnemyUI] EnemyPanel이 없어 현재 GameObject를 부모로 사용합니다.", this);
        }
        // SpawnAll(); // BattleManager.Start() 등에서 호출되도록 변경 권장
    }

    public void SpawnAll()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[EnemyUI] EnemyPrefab이 할당되지 않았습니다! 적을 스폰할 수 없습니다.", this);
            return;
        }
        if (enemyCharacters == null || enemyCharacters.Count == 0)
        {
            Debug.LogWarning("[EnemyUI] enemyCharacters 리스트가 비어있거나 null입니다. 스폰할 적이 없습니다.", this);
            return;
        }

        // 기존 적 오브젝트 정리 (패널에 있는 모든 자식 오브젝트 제거)
        foreach (Transform child in enemyPanel)
        {
            if (child != null) // 혹시 모를 null 체크
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var data in enemyCharacters) // 변수명을 enemyData에서 data로 변경하여 명확성 향상
        {
            if (data == null)
            {
                Debug.LogWarning("[EnemyUI] enemyCharacters 리스트에 null인 EnemyData가 포함되어 있습니다. 해당 적은 스킵합니다.", this);
                continue;
            }

            GameObject obj = Instantiate(enemyPrefab, enemyPanel);
            obj.name = $"Enemy_{data.EnemyName}"; // 디버깅 용이하게 이름 설정

            // UnitBase 컴포넌트 가져오기
            UnitBase unit = obj.GetComponent<UnitBase>();
            if (unit == null)
            {
                Debug.LogError($"[EnemyUI] 생성된 적 프리팹 '{enemyPrefab.name}'에 UnitBase 컴포넌트가 없습니다! 적 데이터: {data.EnemyName}", obj);
                Destroy(obj); // UnitBase 없으면 의미 없으므로 파괴
                continue;
            }

            // UnitBase 정보 세팅
            unit.IsPlayer = false;          // 1. 적 유닛으로 설정
            unit.enemyData = data;          // 2. EnemyData ScriptableObject 참조 할당 (소문자 'e')

            // 3. HP 초기화 (UnitBase의 InitializeHp 메서드 사용)
            //    InitializeHp는 내부적으로 MaxHp를 참조하여 currentHp를 설정합니다.
            unit.InitializeHp();

            // EnemyView 세팅 (선택 사항, EnemyView 스크립트가 있다면)
            EnemyView view = obj.GetComponent<EnemyView>();
            if (view != null)
            {
                view.SetEnemy(data); // EnemyView에 EnemyData 전달
            }
            else
            {
                // Debug.Log($"[EnemyUI] 생성된 적 유닛 '{obj.name}'에 EnemyView 컴포넌트가 없습니다. (선택 사항)", obj);
            }
        }
        Debug.Log($"[EnemyUI] {enemyCharacters.Count}명의 적 스폰 완료.");
    }
}