using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("Reference")/*Tham chiếu*/]
    public PlayerStats_SO stats_SO;
    public PlayerAnimation PlayerAnimation;
    public Player_Status Status;
    public HealthBar healthBar;

    [SerializeField] protected GameObject RangeLine;
    [SerializeField] protected LayerMask layerMasks;
    [SerializeField] protected Transform posAttack;
    [SerializeField] protected FillAttackColldown fillColldown;

    Rigidbody2D rb;
    Vector3 moveInput;


    [Header("Variables")]
    bool isMovingFirst = false; // di chuyển ban đầu
    bool isMovingLast = false;  // di chuyển hiện tại
    [HideInInspector] public bool isPaused = false;

    public event Action E_EndAnimationDie;
    public event Action E_Die;
    public bool isDie = false;
    public event Action E_TakeDamage;

    private Dictionary<AbilityType, AbilityBase_SO> _abilities;


    #region  -------------- Private Methods -------------- 
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    
    }
    protected virtual void Start() => Initialized();
    void OnDestroy()
    {
        isPaused = false;
        PlayerAnimation.animationState.Complete -= CompleteAnimation;
        PlayerAnimation.animationState.Event -= EventsAnimation;
        Status.OnHealthChanged -= healthBar.SetValue;
    }
    #endregion


    #region  -------------- Public Methods -------------- 
    void Initialized() // Khởi tạo ??
    {
        Status.SetStats(stats_SO);

        healthBar.Init(Status.maxHealth);
        // đăng kí event
        Status.OnHealthChanged += healthBar.SetValue;
        PlayerAnimation.animationState.Complete += CompleteAnimation;
        PlayerAnimation.animationState.Event += EventsAnimation;

        _abilities = new Dictionary<AbilityType, AbilityBase_SO>();
        foreach (var abi in stats_SO.ProactiveAbility)
        {
            _abilities.Add(abi.abilityType, abi);
        }
        if (stats_SO.PassiveAbility1 != null) OnSelectAbility(stats_SO.PassiveAbility1.abilityType);
        if (stats_SO.PassiveAbility2 != null) OnSelectAbility(stats_SO.PassiveAbility2.abilityType);
    }

    // Action
    protected void InputMove() 
    {
        if (isDie) return;
        
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }
    protected void Move(bool isBlockMoivng) // Di chuyển
    {
        if(isDie)
        {
            Status.moveSpeed= 0;
            return;
        }

        PlayerAnimation.Flip(moveInput.x);

        rb.velocity = Status.moveSpeed * moveInput.normalized;
        isMovingLast = moveInput.x != 0 || moveInput.y != 0;

        if((isMovingFirst != isMovingLast) && !isBlockMoivng) // set animation 
        {
            if (isMovingLast)   PlayerAnimation.Walk();
            else                PlayerAnimation.Idle();
            isMovingFirst = isMovingLast;   
        }
    }
    public virtual void TakeDamage(int amount) 
    { 
        if(stats_SO.Armor != 0)
        {
            float armor = stats_SO.Armor / 100.0f;
            amount -= (int)(amount * armor);
            if(amount < 0) amount = 0;
        }
        E_TakeDamage?.Invoke();
        Status.Subtract(amount);
        SpawnVFX.Instance.Get_TextHandler(TextHandler.Damage, transform.position, amount);

        if (Status.isDie())  Die();
    }
    protected virtual void Die()
    {
        E_Die?.Invoke();
        isDie = true;
        fillColldown.StopColldown();
        PlayerAnimation.Dead();
        Debug.Log("Player die");
    }
    public void Heal(int health) 
    {
        if (!Status.IsHeal()) return;
        
        Status.Heal(health);
        healthBar.PlayAnimationHeal();
        SpawnVFX.Instance.Get_TextHandler(TextHandler.Heal, transform.position, health);   
    }
    protected IEnumerator HealRepeat(int health, int delay)
    {
        while (true)
        {
            if(health != 0)
            {
                Heal(health);
            }
            yield return new WaitForSeconds(delay);
        }
    }
    protected void AfterAttack() // sau khi attack 
    {
        if (IsMoving()) PlayerAnimation.Walk();
        else            PlayerAnimation.Idle();
    }
    protected bool IsMoving() => moveInput.x != 0 || moveInput.y != 0; // Nếu phím di chuyển đang được nhấn -> Return True
    protected void EndAnimationDead()
    {
        gameObject.SetActive(false);
        Fx_Partical fxPlayerDie = SpawnVFX.Instance.Get_FXPlayerDie(transform.position);
        E_EndAnimationDie?.Invoke();
    }

    // Get & Set
    protected void SetSpeed(float speed) => Status.moveSpeed = speed;
    protected void SetDamage(int dmg)
    {
        Status.damageNormal += dmg;
        Status.damageSpecial = (int)Math.Round(Status.damageNormal * stats_SO.SpecialMultiple);
    }
    protected Collider2D RaycastSingle() => Physics2D.OverlapCircle(posAttack.position, Status.rangeAttack, layerMasks);
    protected Collider2D[] RaycastMuilt() => Physics2D.OverlapCircleAll(posAttack.position, Status.rangeAttack, layerMasks);
    protected float GetValueAbility(AbilityType type, int indexArray) => _abilities.TryGetValue(type, out var abi) ? abi.value[indexArray] : 0;
    public void SetAttackRangeLine(bool active) => RangeLine.SetActive(active);

    // Events
    protected virtual void CompleteAnimation(TrackEntry trackEntry) { }
    protected virtual void EventsAnimation(TrackEntry trackEntry, Spine.Event e) { }
    public virtual void OnSelectAbility(AbilityType abilityType) { } // khi chọn 1 kĩ năng -> ?


    // Ability Handler
    protected enum Mode
    {
        Increase,
        Subtract
    }
    protected virtual void Abi_Health(float value, Mode mode)
    {
        switch (mode)
        {
            case Mode.Increase:  Status.maxHealth += (int)value;   break;
            case Mode.Subtract:  Status.maxHealth -= (int)value;   break;
        }
        healthBar.SetMaxValue(Status.maxHealth);
        Heal((int)value);
    }
    protected virtual void Abi_MovementSpeed(float value) => Status.moveSpeed += Status.moveSpeed * value;
    protected virtual void Abi_AttackSpeed(float value) => Status.attackSpeed += Status.attackSpeed * value;
    protected virtual void Abi_Damage(float value, Mode mode)
    {
        switch (mode)
        {
            case Mode.Increase: Status.damageNormal += (int)value; break;
            case Mode.Subtract: Status.damageNormal -= (int)value; break;
        }
        Status.damageSpecial = (int)Math.Round(Status.damageNormal * stats_SO.SpecialMultiple);
    }
    protected virtual void Abi_MoreXP(float value) => XPManager.Instance.bonusXp += value;
    #endregion





#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(posAttack.position, stats_SO.RangeAttack);
    }

#endif
}
