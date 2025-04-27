using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    private Enemy currentTarget;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetTarget(Enemy enemy)
    {
        currentTarget = enemy;

        Debug.Log($"🎯 타겟 지정: {enemy.data.enemyName}");
    }

    public Enemy GetCurrentTarget()
    {
        return currentTarget;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
