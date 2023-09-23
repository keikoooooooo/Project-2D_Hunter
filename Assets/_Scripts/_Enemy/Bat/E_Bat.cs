using UnityEngine;

public class E_Bat : EnemyController
{
    [Header("----- Private Methods -----")]
    [Space]
    [SerializeField] Transform posSkill;



    //protected override void Start()
    //{
    //    base.Start();
    //}
    protected override void OnDisable()
    {
        base.OnDisable();
        UnSubscribeEvent();
        SubscribeEvent();
    }


    void SubscribeEvent() // đăng kí sự kiện theo level
    {
        base.E_AttackSkill += OnSkill;
    }
    void UnSubscribeEvent() // hủy kí sự kiện theo level
    {
        base.E_AttackSkill -= OnSkill;
    }


    void OnSkill()
    {
        var hit = Raycast(posSkill);
        if(hit != null && hit.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(Status.damage);
        }
    }




}
