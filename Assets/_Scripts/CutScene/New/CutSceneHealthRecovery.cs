using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CutSceneHealthRecovery : MonoBehaviour
{
    public UnityEvent E_TriggerPlayer;

    public UnityEvent E_HealPlayer;

    [SerializeField] int AmountHeal; // số máu hồi 
    [SerializeField] float delayHeal;   // thời gian hồi máu

    bool isHeal = false;
    CutScenePlayer player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<CutScenePlayer>(out var player)) return;
        E_TriggerPlayer?.Invoke();
   
        isHeal = true;
        this.player = player;
        StartCoroutine(HealPlayer());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<CutScenePlayer>(out var player)) return;
        isHeal = false;
    }

    private IEnumerator HealPlayer()
    {
        while (isHeal && player != null)
        {
            if (player.isOutOfBlood)
            {
                player.Heal(AmountHeal);
                if (player.CheckHealth())
                {
                    player.isOutOfBlood = false;
                    E_HealPlayer?.Invoke();
                }
            }
            yield return new WaitForSeconds(delayHeal);
        }
    }




}
