using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillAttackColldown : MonoBehaviour
{
    [SerializeField] Image fill;
    Coroutine coroutineCooldown;

    public void StartColldown(float timer)
    {
        gameObject.SetActive(true);
        fill.fillAmount = 1;
        coroutineCooldown = StartCoroutine(CooldownCoroutine(timer));
    }
    public void StopColldown()
    {
        gameObject.SetActive(false);
        if(coroutineCooldown != null)
        {
            StopCoroutine(coroutineCooldown);
        }
    }

    IEnumerator CooldownCoroutine(float timer)
    {
        float currentTime = timer;
        if (currentTime == 0) currentTime = .1f;
        while (currentTime > 0)
        {
            yield return null;

            currentTime -= Time.deltaTime;
            float fillAmount = currentTime / timer;
            fill.fillAmount = fillAmount;
        }
        fill.fillAmount = 0;
    }
}
