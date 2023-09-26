using UnityEngine;

public class KillNotification : SingletonManager<KillNotification>
{
    [SerializeField] private BoxKilled boxKilledPrefab;
    private ObjectPool<BoxKilled> poolKilled;

    public int Count { get; private set; }

    private void Start()
    {
        poolKilled = new ObjectPool<BoxKilled>(boxKilledPrefab, transform, 0);
    }


    public void Notification(EnemyController enemyController)
    {
        var boxKilled = poolKilled.Get();
        boxKilled.SetStats(enemyController.stats_SO.Information.Sprite);
        Count += 1;
        XPManager.Instance.IncreaseXP(Random.Range(1,4));
        SpawnVFX.Instance.Get_FXEnemyDie(enemyController.transform.position);
    }



}
