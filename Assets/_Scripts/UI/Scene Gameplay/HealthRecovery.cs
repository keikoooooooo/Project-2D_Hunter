using System.Collections;
using UnityEngine;

public class HealthRecovery: MonoBehaviour
{
    [SerializeField] int AmountHeal; // số máu hồi 
    [SerializeField] float delayHeal;   // thời gian hồi máu

    bool isHeal = false;
    PlayerController player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player)) return;

        isHeal = true;
        this.player = player;
        StartCoroutine(HealPlayer(this.player, AmountHeal));
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player)) return;
        isHeal = false;
    }

    IEnumerator HealPlayer(PlayerController p, int health)
    {
        while (isHeal && player != null)
        {
            p.Heal(health);
            yield return new WaitForSeconds(delayHeal);
        }
    }



}
