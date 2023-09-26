using Spine;
using System;
using UnityEngine;
using System.Collections;

public class CutSceneEnemy : MonoBehaviour
{
    public event Action E_EnemyDie;

    public CutScenePlayer player;
    public EnemyStats_SO stats_SO;
    public Enemy_Status Status;
    public EnemyAnimation enemyAnimation;
    [SerializeField] HealthBar healthBar;
    [SerializeField] LayerMask layerMask;

    [Header("Variables")/*Biến kiểm tra điều kiện chung*/]
    protected bool isAttack = false;
    bool isDead = false;
    bool playerIsDead = false;
    bool isDistance = false;

    TrackEntry trackCurrent; // animation hiện tại đang active?


    private void Start()
    {
        Initialized();
    }

    private void FixedUpdate()
    {
        Flip();
    }

    private void OnDestroy()
    {
        Status.OnHealthChanged -= healthBar.SetValue;
        enemyAnimation.animationState.Complete -= CompleteAnimation;
        enemyAnimation.animationState.Event -= EventAnimation;
    }


    private void Initialized() // Khởi tạo ??
    {
        enemyAnimation.Idle();
        Status.SetStats(stats_SO, 1);
        Status.damage += 10;
        healthBar.Init(Status.maxHealth);

        Status.OnHealthChanged += healthBar.SetValue;
        enemyAnimation.animationState.Complete += CompleteAnimation;
        enemyAnimation.animationState.Event += EventAnimation;

        StartCoroutine(CheckTriggerPlayer());
    }

    private void Flip()
    {
        if (isDistance)
        {
            if (player != null && player.transform.position.x < transform.position.x)
            {
                enemyAnimation.Flip(-1);
            }
            else
                enemyAnimation.Flip(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        Status.Subtract(amount);

        SpawnVFX.Instance.Get_TextHandler(TextHandler.Damage, transform.position, amount);
        if (Status.isDie())
        {
            Die();
        }
    }

    private void Die()
    {
        E_EnemyDie?.Invoke();
        trackCurrent = null;
        isDead = true;
        isAttack = false;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    private void Attack()
    {
        if (!isAttack || isDead || playerIsDead || trackCurrent != null || player.isDie || player.isOutOfBlood ||
            !player.CompareTag("Player")) return;
        
        enemyAnimation.Attack();
        trackCurrent = enemyAnimation.GetTrackEntry();
    }

    private IEnumerator CheckTriggerPlayer()
    {
        while (true)
        {
            var hit = Raycast();
            if (hit != null)
            {
                isDistance= true;
                isAttack = true;
                Attack();

            }else 
                isDistance = false;

            yield return new WaitForSeconds(.5f);
        }

    }

    private Collider2D Raycast() => Physics2D.OverlapCircle(transform.position,2, layerMask);

    private void CompleteAnimation(TrackEntry trackEntry) // Hoàn thành 1 animation
    {
        if (trackEntry.Animation.Name == "Attack")
        {
            trackCurrent = null;
            isAttack = false;
            enemyAnimation.Idle();
        }
    }

    private void EventAnimation(TrackEntry trackEntry, Spine.Event e)
    {
        var hit = Raycast();
        if(hit != null && hit.TryGetComponent<CutScenePlayer>(out var p))
        {
            p.TakeDamage(Status.damage);
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
