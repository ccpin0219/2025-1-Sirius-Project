using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyView : MonoBehaviour
{
    public Image portraitImage;
    //public TMP_Text nameText;
    //public TMP_Text hpText;

    public void SetEnemy(EnemyData data)
    {
        portraitImage.sprite = data.portrait;
        //nameText.text = data.characterName;
        //hpText.text = "HP: " + data.maxHp.ToString();
        // 필요하면 추가 정보도 표시!
    }
}
