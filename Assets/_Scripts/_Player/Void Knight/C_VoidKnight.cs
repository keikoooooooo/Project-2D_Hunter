using DG.Tweening;
using Spine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class C_VoidKnight : PlayerController
{
    [Header("Private Methods ---")]
    [SerializeField] Image fillHold;

    bool _isAttack = false;
    bool _isAttackHold = false;

    bool _isAttackNormal1 = false;
    bool _isAttackNormal2 = false;   
    int _countAttackNormal = 0;

    float _speedHold;
    float _speedDrop;
    int _currentHealth;

    // phần trăm cộng thêm của các kĩ năng
    float _percentHeal = 0;
    float _percentDeathStrike = 0;
    float _percentLifeSteal = 0;

    TrackEntry _trackEntryHold = null;


    #region Private Methods
    protected override void Awake()
    {
        base.Awake();
        Initialized();
    }
    protected override void Start()
    {
        base.Start();
        _currentHealth = Status.currentHealth;
        _speedDrop = base.stats_SO.MoveSpeed;
        _speedHold = _speedDrop / 2;

        StartCoroutine(base.HealRepeat((int)_percentHeal * _currentHealth, 2));
    }
    void Update()
    {
        if (isPaused || isDie) return;

        base.InputMove();
        InputAttack();
    }
    void FixedUpdate() => base.Move(_isAttackHold || _isAttackNormal1 || _isAttackNormal2);
    #endregion


    #region Public Methods
    void Initialized()
    {

    }
    protected override void Die()
    {
        base.Die();
        fillHold.gameObject.SetActive(false);
    }
    void InputAttack()
    {
        if (_isAttackNormal1 || _isAttackNormal2)
            return;

        AttackHold();
        AttackDrop();
        AttackNormal();
    }
    void AttackHold()
    {
        if (_isAttack) return;
        if (Input.GetMouseButtonDown(0))
        {
            fillHold.fillAmount = 0;
            fillHold.DOFillAmount(1, 2f);

            _isAttack = true;
            _isAttackHold = true;
            base.PlayerAnimation.AttackHold();
            _trackEntryHold = PlayerAnimation.GetTrackEntry();
            base.Status.moveSpeed = _speedHold;
        }
    }
    void AttackDrop()
    {
        if (!_isAttackHold) return;
        if (Input.GetMouseButtonUp(0) && _trackEntryHold != null)
        {
            fillColldown.StartColldown(base.stats_SO.AttackCooldown);
            StartCoroutine(DelayAttack());
            base.PlayerAnimation.AttackDrop();
            base.Status.moveSpeed = _speedDrop;
            _trackEntryHold = null;
        }
    }
    void AttackNormal()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _countAttackNormal += 1;
            if (_countAttackNormal <= 3)
            {
                _isAttackNormal1 = true;
                base.PlayerAnimation.Attack(Status.attackSpeed);
            }
            else
            {
                _countAttackNormal = 0;
                _isAttackNormal2 = true;
                base.PlayerAnimation.AttackAdd(Status.attackSpeed);
            }
        }
    }
    protected override void CompleteAnimation(TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case "Attack":
            case "Attack Add":
                _isAttackNormal1 = false;
                _isAttackNormal2 = false;
                base.AfterAttack();
                break;
            case "Drop Attack":
                _isAttackHold = false;
                _isAttackNormal1 = false;
                _isAttackNormal2 = false;
                base.AfterAttack();
                break;

            case "Dead":
                base.EndAnimationDead();
                break;
        }
    }
    IEnumerator DelayAttack()
    {     
        yield return new WaitForSeconds(base.stats_SO.AttackCooldown);
        _isAttack = false;
    }
    protected override void EventsAnimation(TrackEntry trackEntry, Spine.Event e)
    {
        int dmg = 0;
        float percent = Random.value;
        switch (e.Data.Name)
        {
            case "Attack":
                dmg = percent < _percentDeathStrike ? 999999 : base.Status.damageNormal;
                Attack(dmg);           
                break;
            case "Attack Add":
                dmg = percent < _percentDeathStrike ? 999999 : base.Status.damageNormal + 15;
                Attack(dmg);        
                break; 
            case "Attack Special":
                dmg = percent < _percentDeathStrike ? 999999 : base.Status.damageSpecial;
                int dmgBonus = (int)Math.Round(dmg * fillHold.fillAmount);
                Attack(dmg + dmgBonus);
                DOTween.Kill(fillHold);
                fillHold.DOFillAmount(0, .5f);
                break;
        }
    }
    void Attack(int dmg)
    {
        var hits = RaycastMuilt();

        if (hits == null) return;
        foreach (var hit in hits)   
        {
            EnemyController e = hit.GetComponent<EnemyController>();

            if(e.Status.currentHealth <= dmg)
            {
                e.E_EnemyDie += Steal;
            }
            e.TakeDamage(dmg);
        }
    }
    void Steal(EnemyController e)
    {
        float randHeal = Random.value;
        float enemyHealth = e.Status.maxHealth * 0.2f;   // lấy số máu tối đa của enemy vừa va chạm
        if (randHeal < _percentLifeSteal)
        {
            base.Heal(Mathf.RoundToInt(enemyHealth));
        }
        e.E_EnemyDie -= Steal;
    }
    #endregion


    #region Ability Handler
    public override void OnSelectAbility(AbilityType abilityType)
    {
        float value1 = base.GetValueAbility(abilityType, 0);
        switch (abilityType)
        {
            // Base
            case AbilityType.Health:
                Abi_Health(value1, Mode.Increase);
                break;
            case AbilityType.MovementSpeed:     
                Abi_MovementSpeed(value1); 
                break;
            case AbilityType.AttackSpeed:       
                Abi_AttackSpeed(value1);   
                break;
            case AbilityType.Damage:           
                Abi_Damage(value1, Mode.Increase);       
                break;
            case AbilityType.MoreXP:            
                Abi_MoreXP(value1);        
                break;
            // Other
            case AbilityType.Regen:             
                Abi_Regen(value1);     
                break;
            case AbilityType.DemonPact:
                float value2 = base.GetValueAbility(abilityType, 1);
                Abi_DemonPact(value1, value2);    
                break;
            case AbilityType.DeathStrike:       
                Abi_DeathStrike(value1);   
                break;
            case AbilityType.LifeSteal:         
                Abi_LifeSteal(value1);    
                break;
        }
    }
    protected override void Abi_Health(float value, Mode mode)
    {
        base.Abi_Health(value, mode);
        _currentHealth = Status.currentHealth;
    }
    protected override void Abi_MovementSpeed(float value)
    {
        base.Abi_MovementSpeed(value);
        _speedDrop = Status.moveSpeed;
        _speedHold = _speedDrop / 2;
    }
    private void Abi_Regen(float value)
    {
        _percentHeal += value;
    }
    private void Abi_DemonPact(float value1, float value2)
    {
        base.Abi_Damage(value1, Mode.Increase);
        base.Abi_Health(value2, Mode.Subtract);
    }
    private void Abi_DeathStrike(float value)
    {
        _percentDeathStrike += value;
    }
    private void Abi_LifeSteal(float value)
    {
        _percentLifeSteal += value;
    }
    #endregion

}
