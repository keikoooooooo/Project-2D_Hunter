using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClaimReward : MonoBehaviour
{
    public Button bttClaim;
    public Image icon;
    int value;


    public void SetState(Sprite sprite, int val)
    {
        value = val;
        RewardManager.Instance.GetRewardBox(sprite, value);
    }
    public void SetState(int val)
    {
        value = val;
        RewardManager.Instance.GetRewardBox(icon.sprite, value);
    }
    public void SetState(List<Sprite> sprites, List<int> vals)
    {
        RewardManager.Instance.GetRewardBox(sprites, vals);
    }

}