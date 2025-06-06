using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "RPG/Enemy")]
public class EnemyData : ScriptableObject
{
    public string EnemyName; // 프로퍼티와 이름 일치시킴
    public Sprite portrait;
    public int speed;
    public int maxHp; // <--- 이 필드가 필요합니다!
    public int defense; // <--- 이 필드가 필요합니다!
}
