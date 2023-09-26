using Pathfinding;
using Spine;
using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPool<EnemyController>
{
    public event Action<EnemyController> E_EnemyDie;
    Action<EnemyController> action; // Event trả Enemy về Pool

    [Header("Reference")/*Tham chiếu*/]
    public EnemyStats_SO stats_SO;
    public Enemy_Status Status;
    [SerializeField] HealthBar healthBar;
    [SerializeField] LayerMask layerMask;
    [HideInInspector] public BoxCollider2D boxCollider;
    protected EnemyAnimation enemyAnimation;

    protected AIPath aiPath;
    [HideInInspector] public AIDestinationSetter aiDestination;
    protected PlayerController player;
    protected Transform slotsVFX;

    [Header("Variables")/*Biến kiểm tra điều kiện chung*/]
    protected bool isAttack = false;

    private bool isMovingFirst = false;
    private bool isMovingLast = false;
    private bool isDead = false;
    private bool playerIsDead = false;

    protected event Action E_AttackSkill;
    protected int level;
    int countDead = 0;

    Coroutine _checkCollisionCoroutine;
    TrackEntry trackCurrent; // animation hiện tại đang active?

    float timerAttack;

    #region -------------- Private Methods -------------- 
    protected virtual void Awake() 
    { 
        aiPath = GetComponent<AIPath>();
        aiDestination = GetComponent<AIDestinationSetter>();
        enemyAnimation = GetComponent<EnemyAnimation>();    
        slotsVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;


        if (GamePlayManager.Instance)
        {
            player = GamePlayManager.Instance.player; 
            player.E_Die += HandlerPlayerDie;
            aiDestination.target = player.transform;
        }
    }
    protected virtual void OnEnable()
    {
        isDead = false;

        _checkCollisionCoroutine = StartCoroutine(CheckCollision(.65f));
    }
    protected virtual void Start()
    {
        Initialized();

        // đăng kí event  
        Status.OnHealthChanged += healthBar.SetValue;
        enemyAnimation.animationState.Complete += CompleteAnimation;
        enemyAnimation.animationState.Event += EventsAnimation;

        if (GameManager.Instance) E_EnemyDie += GameManager.Instance.UpdateDataInBestiary;
        if (KillNotification.Instance) E_EnemyDie += KillNotification.Instance.Notification;
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
    }
    protected virtual void OnDisable() => Initialized();

    private void OnDestroy()
    {
        if (player != null)  player.E_Die -= HandlerPlayerDie;

        Status.OnHealthChanged -= healthBar.SetValue;
        enemyAnimation.animationState.Event -= EventsAnimation;
        enemyAnimation.animationState.Complete -= CompleteAnimation;

        if(E_EnemyDie != null)
        {
            if(GameManager.Instance) E_EnemyDie -= GameManager.Instance.UpdateDataInBestiary;
            if(KillNotification.Instance) E_EnemyDie -= KillNotification.Instance.Notification;  
        }
    }
    #endregion


    #region -------------- Public Methods -------------- 
    public void Init(Action<EnemyController> action) => this.action = action; // Tạo 1 event đề khi Enemy dead -> sẽ trả Enemy về lại Pool

    private void Initialized() // Khởi tạo ??
    {
        if      (countDead <= 1) level = 1;
        else if (countDead <= 2) level = 2;
        else                     level = 3;
        Status.SetStats(stats_SO, level);
        aiPath.maxSpeed = Status.moveSpeed;
        healthBar.Init(Status.maxHealth);
        timerAttack = stats_SO.AttackCooldown;
        SetDistance();
    }


    // Action
    private void HandlerPlayerDie()
    {
        playerIsDead = true;
        isAttack = false;
        aiPath.canMove = false;
        enemyAnimation.Idle();
    }

    private void Move() 
    {
        if(isDead || playerIsDead)   return;
        Flip();
       
        isMovingLast = aiPath.desiredVelocity.x != 0 || aiPath.desiredVelocity.y != 0;
        if (isMovingFirst != isMovingLast)
        {
            if (isMovingLast)  
                enemyAnimation.Walk();
            else 
                enemyAnimation.Idle();
            isMovingFirst = isMovingLast;
        }
    }

    private void Flip()
    {
        if (player != null && DistanceToPlayer())
        {
            aiDestination.isTargetRandom = false;
            float x = player.transform.position.x > enemyAnimation.skeletonAnimation.transform.position.x ? 1 : -1;
            enemyAnimation.Flip(x);
        }
        else   
            enemyAnimation.Flip(aiPath.desiredVelocity.x);
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
        E_EnemyDie?.Invoke(this);
        trackCurrent = null;
        isDead = true;
        isAttack = false;
        countDead += 1;
        action(this);
        aiPath.canMove = false;
        enemyAnimation.Idle();
        enemyAnimation.SetSkin(level);

        if (_checkCollisionCoroutine != null) StopCoroutine(_checkCollisionCoroutine);
    }

    private void Attack()
    {
        if (isAttack && !isDead && !playerIsDead && trackCurrent == null && !player.isDie)
        {
            aiPath.canMove = false;
            enemyAnimation.Attack();
            trackCurrent = enemyAnimation.GetTrackEntry();
        }
    }


    private IEnumerator CheckCollision(float timer) // Kiểm tra va chạm với player sau timer (s) 
    {
        yield return new WaitForSeconds(3f);
        aiPath.canMove = false;

        while (!playerIsDead)
        {
            if (DistanceToPlayer()) 
            {
                aiDestination.isTargetRandom = false;
                if (Vector2.Distance(transform.position, player.transform.position) <= Status.rangeAttack)
                {
                    isAttack = true;
                }              
            }
            else
            {
                if (trackCurrent == null)
                {
                    aiDestination.isTargetRandom = true;
                }
                isAttack = false;
            }
            yield return new WaitForSeconds(timer);

            if (player.isDie || isDead) 
                break;
        }
    }


    // Get & Set
    private bool DistanceToPlayer() => player.CompareTag("Player") && 
                                       Vector2.Distance(transform.position, player.transform.position) <= Status.rangeAttack + 2;

    private void SetDistance() // set khoảng cách của enemy với player
    {
        int radius = 0;
        switch (level)
        {
            case 1: radius = stats_SO.RangeAttack[0]; break;
            case 2: radius = stats_SO.RangeAttack[1]; break;
            case 3: radius = stats_SO.RangeAttack[2]; break;
        }
        Status.rangeAttack = radius;
        aiPath.endReachedDistance = radius;
        aiPath.slowdownDistance = radius;
        aiPath.radius = radius;
    }

    protected Collider2D Raycast() => Physics2D.OverlapCircle(transform.position, Status.rangeAttack, layerMask);
    protected Collider2D Raycast(Transform posAttack) => Physics2D.OverlapCircle(posAttack.position, Status.rangeAttack, layerMask);
    

    // Events
    private void EventsAnimation(TrackEntry trackEntry, Spine.Event e) => E_AttackSkill?.Invoke();// Trigger 1 Event trong animation

    private void CompleteAnimation(TrackEntry trackEntry) // Hoàn thành 1 animation
    {
        if (trackEntry.Animation.Name == "Attack")
        {
            aiPath.canMove = true;
            trackCurrent = null;
            isAttack = false;

            if (DistanceToPlayer()) // nếu enemy đang trong tầm đánh ?
            {
                enemyAnimation.Idle();
            }
            else
            {
                enemyAnimation.Walk();
            }
        }
    }

    #endregion



#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black; Gizmos.DrawWireSphere(transform.position, Status.rangeAttack + 2);
   
        Gizmos.color = Color.green; Gizmos.DrawWireSphere(transform.position, stats_SO.RangeAttack[0]); 
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, stats_SO.RangeAttack[1]);
        Gizmos.color = Color.magenta; Gizmos.DrawWireSphere(transform.position, stats_SO.RangeAttack[2]);
    }
#endif

}
