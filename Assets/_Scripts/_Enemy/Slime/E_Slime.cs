
using System;
using UnityEngine;

public class E_Slime : EnemyController
{
    [Header("----- Private Methods -----")]
    [Space]
    [SerializeField] Transform posSkill;



    protected override void OnDisable()
    {
        base.OnDisable();
        UnSubscribeEvent();
        SubscribeEvent();
    }

    private void SubscribeEvent()
    {
        base.E_AttackSkill += OnSkill;
    }

    private void UnSubscribeEvent()
    {
        base.E_AttackSkill -= OnSkill;
    
    }
    void OnSkill()
    {
        var hit = base.Raycast(posSkill);
        if (hit != null && hit.TryGetComponent<PlayerController>(out var player)) 
        {
            player.TakeDamage(base.Status.damage);
        }

    }


}
