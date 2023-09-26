using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutSceneXP : MonoBehaviour
{
    public UnityEvent E_Levelup;

    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textLevel;


    private void Start()
    {
        textLevel.text = "1";

        CutSceneManager.Instance.enemy1.E_EnemyDie += IncreaseXP;
    }

    private void OnDestroy()
    {
        if (CutSceneManager.Instance) 
            CutSceneManager.Instance.enemy1.E_EnemyDie -= IncreaseXP;
    }



    public void IncreaseXP()
    {
        slider.DOValue(slider.maxValue, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            E_Levelup?.Invoke();
            slider.minValue = 0;
            slider.maxValue = 20;
            slider.value = 0;
            textLevel.text = "2";
        });
    }





}
