using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class XPManager : SingletonManager<XPManager> 
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textLevel;
    public int countLevelUp; // số lượng đã lên cấp

    [Space(10)]
    public UnityEvent E_Levelup;

    int level = 1;
    float valueCurrent = 0;
    public float bonusXp = 1;

    void Start()
    {
        SetVaueDefault();
    }


    void SetVaueDefault()
    {
        bonusXp = 1;
        level = 1;       
        slider.maxValue = level * 10;
        slider.value = 0;
        SetTextLevel();
    }

    public void IncreaseXP(float val)
    {
        DOTween.Kill(slider);
        valueCurrent += val * bonusXp;

        SpawnVFX.Instance.Get_TextHandler(TextHandler.XP, transform.position, (int)Math.Round(val * bonusXp));
        if (valueCurrent >= slider.maxValue)
        {      
            slider.DOValue(slider.maxValue, .25f).OnComplete(() => {
                countLevelUp += 1;
                E_Levelup?.Invoke(); // gọi event tới những class đăng kí
                slider.value = 0;            
                level += 1;
                slider.maxValue = level * 10;
                valueCurrent = 0;
                SetTextLevel();
            });          
        }
        else
        {
            slider.DOValue(valueCurrent, .3f);
        }
    }
    void SetTextLevel() => textLevel.text = $"{level}";

    public void SubtractCountLevelUp()
    {
        countLevelUp -= 1;
        if(countLevelUp <= 0) countLevelUp = 0;
    }
}
