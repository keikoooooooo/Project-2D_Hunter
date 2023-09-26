using UnityEngine;

public class E_Bat : EnemyController
{
    [Header("----- Private Methods -----")]
    [Space]
    [SerializeField]
    private Transform posSkill;

    
    protected override void OnDisable()
    {
        base.OnDisable();
        UnSubscribeEvent();
        SubscribeEvent();
    }


    private void SubscribeEvent() // đăng kí sự kiện theo level
    {
        base.E_AttackSkill += OnSkill;
    }

    private void UnSubscribeEvent() // hủy kí sự kiện theo level
    {
        base.E_AttackSkill -= OnSkill;
    }


    private void OnSkill()
    {
        var hit = Raycast(posSkill);
        if(hit != null && hit.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(Status.damage);
        }
    }




}
