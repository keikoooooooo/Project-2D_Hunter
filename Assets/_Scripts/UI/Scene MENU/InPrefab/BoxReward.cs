using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxReward : MonoBehaviour, IPool<BoxReward>
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textValue;
    Action<BoxReward> action;

    public void Init(Action<BoxReward> action) => this.action = action;
    public void Action() => action(this);


    public void SetStats(Sprite sprite, int value)
    {
        icon.sprite = sprite;
        textValue.text = value.ToString();
        icon.SetNativeSize();
    }


}
