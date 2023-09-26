using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyRoadManager : SingletonManager<TrophyRoadManager>, IData
{
    [Space]
    [SerializeField] Sprite spriteCoin;
    [SerializeField] Sprite spriteGem;
    [SerializeField] Sprite spriteNormalChest;
    [SerializeField] Sprite spriteSpecialChest;
    [SerializeField] Sprite spriteShadow;
    [Space]
    [SerializeField] GameObject panelTrophy;
    [Space]
    [SerializeField]
    private ScrollRect scrollRectSpawnBoxtrophy;
    [SerializeField] private Transform contentSpawn;
    [SerializeField] private BoxTrophy boxTrophyPrefab;
    [SerializeField] private GameObject boxEmpty;
    [Space]
    [SerializeField]
    private Button bttClaimAll;

    private List<BoxTrophy> _boxTrophyList;
    private Dictionary<Sprite, int> rewardDataDic;

    private TrophyRoadData _trophyRoadData;
    private bool isSpawnBox = false;

    protected override void Awake()
    {
        base.Awake();
        isSpawnBox = false;
        Initialized();
    }

    public void GETData(GameManager gameManager)
    {
        _trophyRoadData = gameManager.TrophyRoadData;

        SpawnBoxTrophy();
        UpdateData();
    }
    public void UpdateData()
    {
        SetInforNextReward();
        SetStateClaimAllButton();
    }

    private void Initialized()
    {
        DataReference.Register_IData(this);

        _boxTrophyList = new List<BoxTrophy>();
    }

    private void SpawnBoxTrophy()
    {
        if (!isSpawnBox)
        {
            int countSpawn = _trophyRoadData.TrophyRoadList.Count;

            for (int i = 0; i < countSpawn; i++)
            {
                BoxTrophy box = Instantiate(boxTrophyPrefab, contentSpawn);
                box.SetStats(_trophyRoadData.CurrentTrophyCount, _trophyRoadData.TrophyRoadList[i]);

                _boxTrophyList.Add(box);
            }
            boxEmpty.transform.SetParent(contentSpawn);

            isSpawnBox = true;
        }
        SetInforNextReward();
    }
    public void UpdateTrophyData()
    {
        for (int i = 0; i < _boxTrophyList.Count; i++)
        {
            var box = _boxTrophyList[i];
            box.UpdateData(_trophyRoadData.CurrentTrophyCount);
        }
        SetStateClaimAllButton();
    }
    void SetInforNextReward()
    {
        int valueTrophy = 0;
        Sprite spriteNextReward = null;

        float minValue = 0, maxValue = 0;
        foreach (var box in _boxTrophyList)
        {
            minValue = box.slider.minValue;
            maxValue = box.slider.maxValue;

            if (_trophyRoadData.CurrentTrophyCount < maxValue)
            {
                spriteNextReward = box.icon.sprite;
                valueTrophy = (int)box.slider.maxValue;
                break;
            }
        }

        float progressValue = (_trophyRoadData.CurrentTrophyCount - minValue) / (maxValue - minValue);

        if(MenuGameManager.Instance) MenuGameManager.Instance.SetNextReward(spriteNextReward, progressValue, valueTrophy);
    }

    private void SetStateClaimAllButton()
    {
        if (!panelTrophy.activeSelf)
        {
            return;
        }

        bool isEnable = false;
        foreach (var box in _boxTrophyList)
        {
            if (box.isGetReward)
            {
                isEnable = true;        
                break;
            }
        }
        bttClaimAll.interactable = isEnable;
    }

    // Onclick button
    public void OnClickOpenPanelTrophyButton()
    {
        UpdateTrophyData();
        MenuGameManager.Instance.CloseMenu(panelTrophy);
        UpdateTrophyData();
    }
    public void OnClickClosePanelTrophyButton()
    {
        MenuGameManager.Instance.OpenMenu(panelTrophy);
    }
    public void OnClickClaimAllButton()
    {
        rewardDataDic = new Dictionary<Sprite, int>();
        foreach (var data in _boxTrophyList)
        {
            if (_trophyRoadData.CurrentTrophyCount < data.slider.maxValue)  break;

            if (data != null && data.icon.sprite != null && data.isGetReward)
            {
                data.OnClickClaimButton(false);

                if (data.spriteOther.Count != 0)
                {
                    for (int i = 0; i < data.spriteOther.Count; i++)
                    {
                        AddDataDictionary(data.spriteOther[i], data.valueOther[i]);
                    }
                }
                else AddDataDictionary(data.icon.sprite, data.RewardCount);
            }
        }
        RewardManager.Instance.GetRewardBox(rewardDataDic);
        UpdateTrophyData();
        SetStateClaimAllButton();
    }

    private void AddDataDictionary(Sprite sprite , int value) 
    {
        if(sprite == null) return;

        if (rewardDataDic.ContainsKey(sprite))
        {
            rewardDataDic[sprite] += value;
        }
        else
        {
            rewardDataDic.Add(sprite, value);
        }
    }



}
