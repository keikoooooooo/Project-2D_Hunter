using UnityEngine;

public class E_Ghost : EnemyController
{
   
    [Header("----- Private Methods -----")] [Space]
    [SerializeField] Transform posSkill1;
    [SerializeField] Transform posSkill;

    [SerializeField] ParticleSkills skill1Prefab;
    ObjectPool<ParticleSkills> poolSkill1;


    #region Private Methods
    protected override void Start()
    {
        base.Start();
        Initialized();
    }
    protected override  void OnDisable()
    {
        base.OnDisable();
        UnSubscribeEvent();
        SubscribeEvent();
    }
    #endregion


    #region Public Methods
    void Initialized()
    {
        poolSkill1 = new ObjectPool<ParticleSkills>(skill1Prefab, slotsVFX, 0);
    }
    void SubscribeEvent() // đăng kí sự kiện theo level
    {
        int level = base.level;

        base.E_AttackSkill += level switch  // Nhận sự kiện từ Event của animation Attack
        {
            1 => OnSkill1,
            _ => OnSkill,
        };
    }
    void UnSubscribeEvent() // hủy kí sự kiện theo level
    {
        E_AttackSkill -= OnSkill1;
        E_AttackSkill -= OnSkill;
    }
    void OnSkill1() // lv1
    {
        Vector2 direction = base.player.transform.position - posSkill1.position; // lấy hướng của player
        direction.y += 1;
        ParticleSkills skill = poolSkill1.Get();      // lấy skill từ pool
        skill.SetStats(posSkill1.position, Status.damage);// set các thông tin cho skill 
        skill.Shoot(direction, 8);     // và bắn skill ra
    }
    void OnSkill() // lv2, lv3
    {
        var hit = base.Raycast();

        if(hit != null && hit.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(Status.damage);
        }
    }
    #endregion

}
