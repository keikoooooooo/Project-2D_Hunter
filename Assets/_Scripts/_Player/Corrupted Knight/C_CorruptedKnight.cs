using UnityEngine;
using Spine;

public class C_CorruptedKnight : PlayerController
{
    [Header("Private Methods ---")]
    [SerializeField] Animator animatorTextMiss;

    [SerializeField] private ParticleSkills SpecialSkillPrefab;
    [SerializeField] Transform posSkillSpecial; 
    ObjectPool<ParticleSkills> poolSpecialSkill;
 
    private bool _isSpecialAttack;
    private bool _isAttack;
    private TrackEntry _trackEntry;

    private float _speedHold, _speedDrop;

    private ParticleSkills _skillCurrent;

    int _currentHealth;

    // phần trăm cộng thêm của các kĩ năng
    float _percentRegen = 0;
    float _percentDodgeChance = 0;


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
        _speedHold = 0;
        _speedDrop = base.stats_SO.MoveSpeed;

        StartCoroutine(base.HealRepeat((int)_percentRegen * _currentHealth, 2));
    }
    void Update()
    {
        if(isPaused || isDie)  return;

        base.InputMove();
        InputAttack();
    }
    void FixedUpdate() => base.Move(_isSpecialAttack || _isAttack);
    #endregion


    #region Public Methods
    private void Initialized()
    {
        poolSpecialSkill = new ObjectPool<ParticleSkills>(SpecialSkillPrefab, posSkillSpecial, 1);
    }
    void InputAttack()
    {
        AttackHold();
        AttackDrop();
        AttackNormal();
    }
    void AttackHold()
    {
        if (_isSpecialAttack || base.IsMoving()) return;

        if (Input.GetMouseButtonDown(0))
        {
            _skillCurrent = poolSpecialSkill.Get();
            base.PlayerAnimation.AttackHold();
            base.Status.moveSpeed = _speedHold;
            _trackEntry = base.PlayerAnimation.GetTrackEntry();
            _isSpecialAttack = true;
        }
    }
    void AttackDrop()
    {
        if (_trackEntry == null) return;

        if (Input.GetMouseButtonUp(0))
        {
            base.fillColldown.StartColldown(base.stats_SO.AttackCooldown);
            _trackEntry = null;
            base.Status.moveSpeed = _speedDrop;
            base.PlayerAnimation.AttackDrop();
            _skillCurrent.Action();
        }
    }
    void AttackNormal()
    {
        if(_isAttack || base.IsMoving()) return;

        if (Input.GetMouseButtonDown(1))
        {      
            base.PlayerAnimation.Attack(Status.attackSpeed);
            _isAttack = true; 
        }
    }

    public override void TakeDamage(int amount)
    {
        int reducedDamage = amount;
        if (Status.armor != 0)
        {
             reducedDamage = amount - Status.armor;     
        }
        if (_isSpecialAttack)
        {
            reducedDamage -= (reducedDamage * 50 / 100); // giảm 50% sát thương nếu đang hold
        }

        if (reducedDamage <= 0) reducedDamage = 0;

        float percentDodgeChance = Random.value;
        if(percentDodgeChance <= _percentDodgeChance)
        {
            // né tránh
            animatorTextMiss.SetTrigger("TextMiss");
            return;
        }
        base.TakeDamage(reducedDamage);
    }

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
                EnemyController e = hit.GetComponent<EnemyController>();
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
            // base
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
            // other
            case AbilityType.Regen:
                Abi_Regen(value1);
                break;
            case AbilityType.CelestialPact:
                float value2 = base.GetValueAbility(abilityType, 1);
                Abi_CelestialPact(value1, value2);
                break;
            case AbilityType.Armor:
                Abi_Armor(value1);
                break;
            case AbilityType.DodgeChance:
                Abi_DodgeChance(value1);
                break;
        }
    }
    protected override void Abi_Health(float value, Mode mode)
    {
        base.Abi_Health(value, mode);
        _currentHealth = Status.currentHealth;
    }
    private void Abi_Regen(float value)
    {
        _percentRegen += value;
    }
    private void Abi_CelestialPact(float value1, float value2)
    {
        base.Abi_Health(value1, Mode.Increase);
        base.Abi_Damage(value2, Mode.Subtract);
    }
    private void Abi_Armor(float value)
    {
        float amr = 0;
        if (Status.armor == 0) 
            amr = 10;
        else                    
            amr = Status.armor * value;

        Status.armor += Mathf.FloorToInt(amr);
    }
    private void Abi_DodgeChance(float value)
    {
        _percentDodgeChance += value;
    }


    #endregion



}