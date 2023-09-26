using System.Collections;
using UnityEngine;

public class HealthRecovery: MonoBehaviour
{
    [SerializeField] private int AmountHeal; // số máu hồi 
    [SerializeField] private float delayHeal;   // thời gian hồi máu

    private bool isHeal = false;
    private PlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player)) return;

        isHeal = true;
        this.player = player;
        StartCoroutine(HealPlayer(this.player, AmountHeal));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player)) return;
        isHeal = false;
    }

    private IEnumerator HealPlayer(PlayerController p, int health)
    {
        while (isHeal && player != null)
        {
            p.Heal(health);
            yield return new WaitForSeconds(delayHeal);
        }
    }



}
