using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{  
    BoxCollider2D boxCollider; // vị trí spawn -> nằm trong chiều dài và rộng của collider

    [SerializeField] int maxCount;          // số lượng tối đa spawn
    [SerializeField] float delaySpawn;      // thời gian chờ đợt spawn tiếp theo
    [SerializeField] float radiusCheck;     // bán kính check va chạm 
    [SerializeField] LayerMask layerMask;   // kiểm tra va chạm với layer nào ?
    [Space]

    [SerializeField] List<EnemyController> enemies;
    ObjectPool<EnemyController> enemiesPool;


    int countEnemy = 0; // số lượng enemy spawn ra


    float posX, posY;


    void Awake() => boxCollider = GetComponent<BoxCollider2D>();

    void Start()
    {
        countEnemy = 0;
        enemiesPool = new ObjectPool<EnemyController>(enemies, transform, enemies.Count);

        StartCoroutine(SpawnCoroutine());
    }


    IEnumerator SpawnCoroutine()
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

    void Spawn()
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


    void GetEnemy(Fx_Circle fx_Circle)
    {
        EnemyController e = enemiesPool.Get();
        e.transform.position = fx_Circle.transform.position;
        e.aiDestination.boxCollider = boxCollider;
        e.E_EnemyDie += EnemyDie;

        fx_Circle.E_EndCircleEffect -= GetEnemy;
    }

    void EnemyDie(EnemyController e)
    {
        countEnemy--;
        e.E_EnemyDie -= EnemyDie;
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(new Vector3(posX, posY, 0), radiusCheck);
    //}

}
