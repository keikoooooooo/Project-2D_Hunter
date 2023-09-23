using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TrophyRoadData
{
    public int CurrentTrophyCount; // cúp user đạt được ?
    public List<TrophyRoad> TrophyRoadList;

    // Constructor?
    public TrophyRoadData() { }
    public TrophyRoadData(TextAsset _requirementData, TextAsset _rewardsData, TextAsset _rewardNames)
    {
        CurrentTrophyCount = 0;
        TrophyRoadList = new List<TrophyRoad>();

        int minRequirementData = 0;
        string[] requirementData = _requirementData.text.Split('\n');
        string[] rewardsData = _rewardsData.text.Split('\n');
        string[] rewardNames = _rewardNames.text.Split('\n');

        for (int i = 0; i < requirementData.Length; i++)
        {
            TrophyRoad trophyRoad = new TrophyRoad( minRequirementData, 
                                                    int.Parse(requirementData[i].Trim()), 
                                                    int.Parse(rewardsData[i].Trim()), 
                                                    rewardNames[i].Trim()
                                                    );
            TrophyRoadList.Add(trophyRoad);
            minRequirementData = int.Parse(requirementData[i].Trim());
        }
    }

    public void IncreaseTrophy(int value) // tăng cúp
    {
        CurrentTrophyCount += value;
        foreach (var trophy in TrophyRoadList)
        {
            if(CurrentTrophyCount >= trophy.MaxTrophyValue && !trophy.HasReward)
            {
                trophy.HasReward = true;
                if(CurrentTrophyCount < trophy.MaxTrophyValue)
                {
                    return;
                }
            }
        }
    }

    public void ReceivedReward(int trophyValue)  // đã nhận thưởng -> đánh dấu = true
        => TrophyRoadList.Find(x => x.MaxTrophyValue == trophyValue).HasReceivedReward = true;
}

[Serializable]
public class TrophyRoad
{
    public int MinTrophyValue;      // cúp tối thiểu cần
    public int MaxTrophyValue;      // cúp tối đa cần
    public int RewardCount;         // số lượng phần thưởng
    public string TextReward;       // tên phần thưởng ?
    public bool HasReward;          // có phần thưởng
    public bool HasReceivedReward;  // đã nhận thưởng?

    public TrophyRoad() { }
    public TrophyRoad(int minValue, int maxValue, int count, string data)
    {
        MinTrophyValue = minValue;
        MaxTrophyValue = maxValue;
        RewardCount = count;
        TextReward = data;
        HasReward = false;
        HasReceivedReward = false;
    }
}
