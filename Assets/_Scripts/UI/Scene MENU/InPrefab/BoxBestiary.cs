using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxBestiary : MonoBehaviour, IPool<BoxBestiary> 
{
    [SerializeField] Color[] ColorLevel;
    [SerializeField] Sprite gemIcon;
    public Image avatar;
    public Image[] Star;
    int valueStar;
    [Space]
    public TextMeshProUGUI textEnemyName;
    public TextMeshProUGUI textRequest;
    public TextMeshProUGUI textProgress;
    public TextMeshProUGUI textReward;
    public TextMeshProUGUI textDoneRequest;
    [Space]
    public GameObject boxProgress;
    public GameObject boxDoneRequest;
    [Space]
    public Button bttClaim;


    Action<BoxBestiary> action;
    DataKillEnemy dataKill;


    public void Init(Action<BoxBestiary> action) => this.action = action;

    public void Initialized(DataKillEnemy data)
    {
        dataKill = data;
        valueStar = dataKill.KillLevel;

        UpdateData();
    }
    public void UpdateData()
    {
        avatar.sprite = dataKill.Sprite;
        textEnemyName.text = dataKill.EnemyName;
        textRequest.text = $"Hunt monster {dataKill.KillRequest} times.";
        textProgress.text = $"{dataKill.KillCurrent} / {dataKill.KillRequest}";
        textReward.text = $"{dataKill.Reward}";

        textDoneRequest.text = $"{dataKill.KillRequest - 10} monsters hunted !";
        valueStar = dataKill.StarCurrentLevel;

        if (dataKill.KillCurrent >= dataKill.KillRequest) valueStar += 1;
        SetColorStar();
        CheckReward();
    }

    public void OnClickClaimButton()
    {
        int value = dataKill.RewardsList[0];
        dataKill.RewardsList.RemoveAt(0);
        CheckReward();

        RewardManager.Instance.GetRewardBox(gemIcon, value);
        GameManager.Instance.UserData.Gem += value;
    }
    void CheckReward()
    {
        if (dataKill.RewardsList.Count != 0)
        {
            bttClaim.gameObject.SetActive(true);
            boxDoneRequest.SetActive(true);
            boxProgress.SetActive(false);
        }
        else
        {
            bttClaim.gameObject.SetActive(false);
            boxDoneRequest.SetActive(false);
            boxProgress.SetActive(true);
        }
    }
    void SetColorStar()
    {
        int indexColor = dataKill.IndexColorStarCurrentLevel;
        if (indexColor >= ColorLevel.Length)
            indexColor = ColorLevel.Length - 1;

        for (int i = 0; i < valueStar; i++)
        {
            Star[i].color = ColorLevel[indexColor];
        }

        if (valueStar == 6) return;
        for (int i = valueStar; i < 6; i++)
        {
            Star[i].color = ColorLevel[indexColor - 1];
        }
    }


}
