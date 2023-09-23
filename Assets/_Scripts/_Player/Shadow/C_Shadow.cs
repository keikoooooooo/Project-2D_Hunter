using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class C_Shadow : PlayerController
{

    [Header("Private Methods ---")]

    BoxCollider2D boxCollider;
    bool _isAttackHold;
    bool _isAttack;

    ExposedList<Slot> slots;

    float _percentDeathStrike;
    float _percentLifeSteal;

    #region Private Methods
    protected override void Awake()
    {
        base.Awake();

        Initialized();
    }
    protected override void Start()
    {
        base.Start();

    }
    void Update()
    {
        if (base.isPaused || base.isDie) return;

        base.InputMove();
        InputAttack();

    }
    void FixedUpdate()
    {
        base.Move(_isAttack);
    }
    #endregion



    #region Public Methods
    void Initialized()
    {
        slots = PlayerAnimation.skeletonAnimation.skeleton.Slots; // lấy toàn bộ slot trong spine -> setcolor

        boxCollider = GetComponent<BoxCollider2D>();
    }
    void InputAttack()
    {
        AttackSpecial();
        AttackNormal();
    }
    void AttackSpecial()
    {
        if (_isAttackHold) return;

        if (Input.GetMouseButtonDown(0))
        {
            Invisible(.6f);
            _isAttackHold = true;
        }
    }
    void AttackNormal()
    {
        if (_isAttack) return;

        if (Input.GetMouseButtonDown(1))
        {
            Invisible(1f);
            base.PlayerAnimation.Attack(Status.attackSpeed);
            _isAttack = true;
        }
    }

    void Invisible(float amount) // giảm opacity => vô hình
    {
        foreach (Slot slot in slots)
        {
            slot.SetColor(new Color(1, 1, 1, amount));
        }
        if (amount == 1)
        {
            boxCollider.enabled = true;
            gameObject.tag = "Player";
            if (_isAttackHold)
            {
                float AttackCooldown = stats_SO.AttackCooldown;
                base.fillColldown.StartColldown(AttackCooldown);
                StartCoroutine(CooldownAttackCoroutine(AttackCooldown));
            }
        }
        else
        {
            boxCollider.enabled = false;
            gameObject.tag = "Untagged";
        }
    }
    IEnumerator CooldownAttackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttackHold = false;
    }

    protected override void CompleteAnimation(TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case "Attack":
                _isAttack = false;
                base.AfterAttack();
                break;

            case "Dead":
                base.EndAnimationDead();
                break;
        }
    }
    protected override void EventsAnimation(TrackEntry trackEntry, Spine.Event e)
    {
        if (trackEntry.Animation.Name == "Attack")
        {
            Attack();
        }
    }
    void Attack()
    {
        var hits = base.RaycastMuilt();
        if (hits != null)
        {
            foreach (var hit in hits)
            {
                float percent = Random.value;
                int dmg = percent < _percentDeathStrike ? 999999 : base.Status.damageNormal;

                EnemyController e = hit.GetComponent<EnemyController>();
                if(e.Status.currentHealth <= dmg)
                {
                    e.E_EnemyDie += Steal;
                }
                e.TakeDamage(dmg);
            }
        }
    }
    void Steal(EnemyController e)
    {
        float randHeal = Random.value;
        int enemyHealth = e.Status.maxHealth;   // lấy số máu tối đa của enemy vừa va chạm
        if (randHeal < _percentLifeSteal)
        {
            base.Heal(enemyHealth);
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
            case AbilityType.Health:
                base.Abi_Health(value1, Mode.Increase);
                break;
            case AbilityType.MovementSpeed:
                base.Abi_MovementSpeed(value1);
                break;
            case AbilityType.AttackSpeed:
                base.Abi_AttackSpeed(value1);
                break;
            case AbilityType.Damage:
                base.Abi_Damage(value1, Mode.Increase);
                break;
            case AbilityType.MoreXP:
                base.Abi_MoreXP(value1);
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
    void Abi_DemonPact(float value1, float value2)
    {
        base.Abi_Damage(value1, Mode.Increase);
        base.Abi_Health(value2, Mode.Subtract);
    }
    void Abi_DeathStrike(float value)
    {
        _percentDeathStrike += value;
    }
    void Abi_LifeSteal(float value)
    {
        _percentLifeSteal += value;
    }   
    #endregion

}
