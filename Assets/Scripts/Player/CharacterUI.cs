using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public GameObject characterPrefab;           // 플레이어 프리팹
    public Transform characterPanel;             // 캐릭터가 붙을 부모(보통 자기 자신)
    public List<CharacterData> playerCharacters; // 플레이어 데이터 리스트

    public void SpawnAll()
    {
        // 기존 자식 오브젝트 정리
        foreach (Transform child in characterPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var charData in playerCharacters)
        {
            GameObject obj = Instantiate(characterPrefab, characterPanel);

            // CharacterView 세팅(안 쓰면 생략)
            CharacterView view = obj.GetComponent<CharacterView>();
            if (view != null)
                view.SetCharacter(charData);

            // UnitBase 정보 세팅
            UnitBase unit = obj.GetComponent<UnitBase>();
            unit.characterData = charData;
            unit.IsPlayer = true;
            unit.currentHp = charData.maxHp;
        }
    }

}
