using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private BoxCollider2D boxCollider; // vị trí spawn -> nằm trong chiều dài và rộng của collider

    [SerializeField] private int maxCount;          // số lượng tối đa spawn
    [SerializeField] private float delaySpawn;      // thời gian chờ đợt spawn tiếp theo
    [SerializeField] private float radiusCheck;     // bán kính check va chạm 
    [SerializeField] private LayerMask layerMask;   // kiểm tra va chạm với layer nào ?
    [Space]

    [SerializeField]
    private List<EnemyController> enemies;

    private ObjectPool<EnemyController> enemiesPool;


    private int countEnemy = 0; // số lượng enemy spawn ra


    float posX, posY;


    private void Awake() => boxCollider = GetComponent<BoxCollider2D>();

    private void Start()
    {
        countEnemy = 0;
        enemiesPool = new ObjectPool<EnemyController>(enemies, transform, enemies.Count);

        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if(countEnemy < maxCount)
            {
                Spawn();
            }            
            yield return new WaitForSeconds(delaySpawn); // mỗi delaySpawn(s) sẽ gọi hàm và spawn 1 enemy     
        }
    }

    private void Spawn()
    {
        posX = Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
        posY = Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y);
        var hit = Physics2D.OverlapCircle(new Vector3(posX, posY, 0), radiusCheck, layerMask);

        if (hit == null) // nếu vị trí spawn không va chạm với vật cạn + số lượng enemy của khu vực hiện tại nhỏ hơn tổng số lượng được phép spawn
        {
            countEnemy++;
            Vector3 target = new Vector3(posX, posY, 0f);
            Fx_Circle fx_Circle = SpawnVFX.Instance.Get_CircleFX(target);
            fx_Circle.E_EndCircleEffect += GetEnemy;
        }
    }


    private void GetEnemy(Fx_Circle fx_Circle)
    {
        EnemyController e = enemiesPool.Get();
        e.transform.position = fx_Circle.transform.position;
        e.aiDestination.boxCollider = boxCollider;
        e.E_EnemyDie += EnemyDie;

        fx_Circle.E_EndCircleEffect -= GetEnemy;
    }

    private void EnemyDie(EnemyController e)
    {
        countEnemy--;
        e.E_EnemyDie -= EnemyDie;
    }


}
