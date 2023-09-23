using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum TextHandler
{
    Damage,
    Heal,
    XP
}
public class Fx_Text : MonoBehaviour, IPool<Fx_Text>
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI textHandler;
    Action<Fx_Text> action;

    int codeAnimHealDamage = Animator.StringToHash("Damage_HealText");
    int codeAnimXp = Animator.StringToHash("XpText");

    float timerAnimation;

    public void Init(Action<Fx_Text> returnAction) => action = returnAction;
    
    
    public void SetStats(TextHandler textHandler, Vector3 posCurrent, Vector3 posRandom, float value)
    {
        timerAnimation = 0.4f;
        switch (textHandler)
        {
            case TextHandler.Damage:
                SetStatsDamage(posRandom, value);
                break;

            case TextHandler.Heal:
                SetStatsHeal(posRandom, value);
                break;
                
            case TextHandler.XP:
                timerAnimation = 0.8f;
                SetStatsXP(posCurrent, value);      
                break;
        }
        StartCoroutine(Action());
    }
    
    private void SetStatsDamage(Vector3 pos, float damage)
    {
        animator.Play(codeAnimHealDamage);
        pos.y += 4f;
        transform.position = pos;
        textHandler.text = $"{damage}";
    }
    private void SetStatsHeal(Vector3 pos, float heal)
    {
        animator.Play(codeAnimHealDamage);
        pos.y += 4f;
        transform.position = pos;
        textHandler.color = new Color(.18f, 1, 0, 1);
        textHandler.text = $"{heal}";
    }
    private void SetStatsXP(Vector3 pos, float xp)
    {
        animator.Play(codeAnimXp);
        transform.position = pos;
        textHandler.text = $"+{xp} xp";
    }


    IEnumerator Action()
    {
        yield return new WaitForSeconds(timerAnimation);    
        textHandler.color = Color.white;
        action(this);
    }




}
