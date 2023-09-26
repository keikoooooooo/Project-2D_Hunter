using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : SingletonManager<RewardManager>
{
    [SerializeField] GameObject panelReward;
    [SerializeField] private Button bttClaim;
    [Space]
    [SerializeField]
    private BoxReward boxRewardPrefab;
    [SerializeField] private Transform contentSpawn;

    ObjectPool<BoxReward> poolBoxReward;
    private List<BoxReward> rewardList;
    Audio _audio;


    private void OnEnable()
    {
        Initialized();
    }

    private void Initialized()
    {
        _audio = GetComponent<Audio>();
        rewardList = new List<BoxReward>();
        panelReward.SetActive(false);
        poolBoxReward = new ObjectPool<BoxReward>(boxRewardPrefab, contentSpawn, 5);
    }


    public void GetRewardBox(Sprite sprite, int value)
    {
        _audio.Play();
        panelReward.SetActive(true);
        BoxReward box = poolBoxReward.Get();
        box.SetStats(sprite, value);
        rewardList.Add(box);
        GameManager.Instance.UpdateMultiData();
    }
    public void GetRewardBox(List<Sprite> sprites, List<int> value)
    {
        _audio.Play();
        panelReward.SetActive(true);

        for (int i = 0; i < sprites.Count; i++)
        {
            BoxReward box = poolBoxReward.Get();
            box.SetStats(sprites[i], value[i]);
            rewardList.Add(box);
        }
        GameManager.Instance.UpdateMultiData();
    }
    public void GetRewardBox(Dictionary<Sprite, int> keyValuePairs)
    {
        _audio.Play();
        panelReward.SetActive(true);
        foreach (var kvp in keyValuePairs)
        {
            BoxReward box = poolBoxReward.Get();
            box.SetStats(kvp.Key, kvp.Value);
            rewardList.Add(box);
        }
        GameManager.Instance.UpdateMultiData();
    }




    // Onclick Button
    public void OnClickClaimButton()
    {
        panelReward.SetActive(false);
         
        if (rewardList.Count == 0) return;
        foreach (var box in rewardList)
        {
            box.Action();
        }
        rewardList.Clear();
    }


}

