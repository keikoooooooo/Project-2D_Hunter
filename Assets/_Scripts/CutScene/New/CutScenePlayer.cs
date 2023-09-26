using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CutScenePlayer : PlayerController
{
    public UnityEvent E_OutOfBlood;
    public bool isOutOfBlood;

    [Header("Private Methods ---")] private BoxCollider2D boxCollider;

    private bool _isAttackHold;
    private bool _isAttack;

    private ExposedList<Slot> slots;

    private bool _isSpecialAttack;

    private int _currentHealth;

    // phần trăm cộng thêm của các kĩ năng
    private readonly float _percentRegen = 0;


    #region Private Methods
    protected override void Start()
    {
        base.Start();
        Initialized();

        _currentHealth = Status.currentHealth;
        StartCoroutine(base.HealRepeat((int)_percentRegen * _currentHealth, 2));
    }

    private void Update()
    {
        if (isPaused || isDie) return;

        base.InputMove();
        InputAttack();
    }

    private void FixedUpdate() => base.Move(_isSpecialAttack || _isAttack);
    #endregion


    #region Public Methods

    private void Initialized()
    {
        slots = PlayerAnimation.skeletonAnimation.skeleton.Slots; // lấy toàn bộ slot trong spine -> setcolor

        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void InputAttack()
    {
        AttackSpecial();
        AttackNormal();
    }

    private void AttackSpecial()
    {
        if (_isAttackHold) return;

        if (Input.GetMouseButtonDown(0))
        {
            Invisible(.6f);
            _isAttackHold = true;
        }
    }

    private void AttackNormal()
    {
        if (_isAttack) return;

        if (Input.GetMouseButtonDown(1))
        {
            Invisible(1f);
            base.PlayerAnimation.Attack(Status.attackSpeed);
            _isAttack = true;
        }
    }

    private void Invisible(float amount) // giảm opacity => vô hình
    {
        foreach (Slot slot in slots)
        {
            slot.SetColor(new Color(1, 1, 1, amount));
        }
        if (amount == 1)
        {
            //boxCollider.enabled = true;
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
            //boxCollider.enabled = false;
            gameObject.tag = "PlayerTest";
        }
    }

    private IEnumerator CooldownAttackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttackHold = false;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        float maxH = Status.maxHealth;
        if(Status.currentHealth <= maxH / 2)
        {
            isOutOfBlood = true;
            E_OutOfBlood?.Invoke();
        }

    }

    public bool CheckHealth() => Status.currentHealth >= Status.maxHealth;

    protected override void CompleteAnimation(TrackEntry trackEntry)
    {
        switch (trackEntry.Animation.Name)
        {
            case "Attack":
            case "Drop Attack":
                _isAttack = false;
                _isSpecialAttack = false;
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
                CutSceneEnemy e = hit.GetComponent<CutSceneEnemy>();
                e.TakeDamage(Status.damageNormal);
            }
        }
    }
    #endregion


    #region Ability Handler
    public override void OnSelectAbility(AbilityType abilityType)
    {
        float value1 = base.GetValueAbility(abilityType, 0);
        switch (abilityType)
        {
            case AbilityType.Damage:
                base.Abi_Damage(value1, Mode.Increase);
                break;
            case AbilityType.MoreXP:
                base.Abi_MoreXP(value1);
                break;
        }
    }
    protected override void Abi_Health(float value, Mode mode)
    {
        base.Abi_Health(value, mode);
        _currentHealth = Status.currentHealth;
    }
    #endregion
}
