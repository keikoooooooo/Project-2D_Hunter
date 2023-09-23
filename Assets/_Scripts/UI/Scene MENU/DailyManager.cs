using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DailyManager : MonoBehaviour, IData
{
    [SerializeField] ButtonClaimReward bttDay1;
    [SerializeField] ButtonClaimReward bttDay2;
    [SerializeField] ButtonClaimReward bttDay3;
    [SerializeField] ButtonClaimReward bttDay4;
    [SerializeField] ButtonClaimReward bttDay5;
    [SerializeField] ButtonClaimReward bttDay6;
    [SerializeField] ButtonClaimReward bttDay7;
    [SerializeField] GameObject[] notifeBtt;
    [Space]
    [SerializeField] Sprite spriteCoin;
    [SerializeField] Sprite spriteGem;

    List<Sprite> spriteOther = new List<Sprite>();
    List<int> valueOther = new List<int>();

    DailyData _dailyData;

    DateTime resetTime;
    DateTime currentTime;
    TimeSpan timeThrough;   // thời gian đã trôi qua (giây)

    void Awake()
    {
        DataReference.Register_IData(this);
    }
    void Start()
    {
        Initialized();
        RegisterEvent();
    }
    void OnDestroy()
    {
        UnRegisterEvent();
    }


    public void GETData(GameManager gameManager)
    {
        _dailyData = gameManager.DailyData;
        UpdateData();
    }

    public void UpdateData()
    {
        FindTime();

        if (currentTime.Date > resetTime.Date)
        {
            _dailyData.ResetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0).AddDays(1).ToString();
            _dailyData.DayCurrent += 1;

            if (timeThrough.TotalDays > 2) _dailyData.ResetDaily();
        }
        SetStateButton();
    }
    void Initialized()
    {
        bttDay1.bttClaim.interactable = false;
        bttDay2.bttClaim.interactable = false;
        bttDay3.bttClaim.interactable = false;
        bttDay4.bttClaim.interactable = false;
        bttDay5.bttClaim.interactable = false;
        bttDay6.bttClaim.interactable = false;
        bttDay7.bttClaim.interactable = false;

        foreach (var obj in notifeBtt)
        {
            obj.SetActive(false);
        }
    }
    void RegisterEvent()
    {
        bttDay1.bttClaim.onClick.AddListener(OnClickDay1Button);
        bttDay2.bttClaim.onClick.AddListener(OnClickDay2Button);
        bttDay3.bttClaim.onClick.AddListener(OnClickDay3Button);
        bttDay4.bttClaim.onClick.AddListener(OnClickDay4Button);
        bttDay5.bttClaim.onClick.AddListener(OnClickDay5Button);
        bttDay6.bttClaim.onClick.AddListener(OnClickDay6Button);
        bttDay7.bttClaim.onClick.AddListener(OnClickDay7Button);

    }
    void UnRegisterEvent()
    {
        bttDay1.bttClaim.onClick.RemoveListener(OnClickDay1Button);
        bttDay2.bttClaim.onClick.RemoveListener(OnClickDay2Button);
        bttDay3.bttClaim.onClick.RemoveListener(OnClickDay3Button);
        bttDay4.bttClaim.onClick.RemoveListener(OnClickDay4Button);
        bttDay5.bttClaim.onClick.RemoveListener(OnClickDay5Button);
        bttDay6.bttClaim.onClick.RemoveListener(OnClickDay6Button);
        bttDay7.bttClaim.onClick.RemoveListener(OnClickDay7Button);
    }

    void FindTime() // tìm thời gian và check đã qua 0h đêm chưa?
    {
        resetTime = DateTime.Parse(_dailyData.ResetTime);
        currentTime = DateTime.Now;

        timeThrough = currentTime - resetTime;
    }
    void SetStateButton()
    {
        switch (_dailyData.DayCurrent)
        {
            case 1:
                bttDay1.bttClaim.interactable = _dailyData.DayCurrent == 1 && !_dailyData.isRewardDay[0];
                notifeBtt[0].SetActive(bttDay1.bttClaim.interactable);
                break;

            case 2:
                bttDay2.bttClaim.interactable = _dailyData.DayCurrent == 2 && !_dailyData.isRewardDay[1];
                notifeBtt[1].SetActive(bttDay2.bttClaim.interactable);
                break;

            case 3:
                bttDay3.bttClaim.interactable = _dailyData.DayCurrent == 3 && !_dailyData.isRewardDay[2];
                notifeBtt[2].SetActive(bttDay3.bttClaim.interactable);
                break;

            case 4:
                bttDay4.bttClaim.interactable = _dailyData.DayCurrent == 4 && !_dailyData.isRewardDay[3];
                notifeBtt[3].SetActive(bttDay4.bttClaim.interactable);
                break;

            case 5:
                bttDay5.bttClaim.interactable = _dailyData.DayCurrent == 5 && !_dailyData.isRewardDay[4];
                notifeBtt[4].SetActive(bttDay5.bttClaim.interactable);
                break;

            case 6:
                bttDay6.bttClaim.interactable = _dailyData.DayCurrent == 6 && !_dailyData.isRewardDay[5];
                notifeBtt[5].SetActive(bttDay6.bttClaim.interactable);
                break;

            default:
                bttDay7.bttClaim.interactable = _dailyData.DayCurrent == 7 && !_dailyData.isRewardDay[6];
                notifeBtt[6].SetActive(bttDay7.bttClaim.interactable);
                break;
        }
    }

    // OnClick Button
    private void OnClickDay1Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        GameManager.Instance.UserData.Coin += 100;
        notifeBtt[0].SetActive(false);
        _dailyData.isRewardDay[0] = true;
        bttDay1.SetState(100);
        bttDay1.bttClaim.interactable = false;
    }
    private void OnClickDay2Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        GameManager.Instance.UserData.Gem += 20;
        notifeBtt[1].SetActive(false);
        _dailyData.isRewardDay[1] = true;
        bttDay2.SetState(20); 
        bttDay2.bttClaim.interactable = false;
    }
    private void OnClickDay3Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        spriteOther.Clear();
        valueOther.Clear();

        int randCoin = Random.Range(1, 50);
        int randGem = Random.Range(1, 10);

        spriteOther.Add(spriteCoin); valueOther.Add(randCoin);
        spriteOther.Add(spriteGem); valueOther.Add(randGem);

        GameManager.Instance.UserData.Coin += randCoin;
        GameManager.Instance.UserData.Gem += randGem;

        _dailyData.isRewardDay[2] = true;
        notifeBtt[2].SetActive(false); 
        bttDay3.SetState(spriteOther, valueOther);
        bttDay3.bttClaim.interactable = false;
    }
    private void OnClickDay4Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        GameManager.Instance.UserData.Coin += 300;

        _dailyData.isRewardDay[3] = true;
        notifeBtt[3].SetActive(false);
        bttDay4.SetState(300);
        bttDay4.bttClaim.interactable = false;
    }
    private void OnClickDay5Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        GameManager.Instance.UserData.Token += 500;

        _dailyData.isRewardDay[4] = true;
        notifeBtt[4].SetActive(false); 
        bttDay5.SetState(500);
        bttDay5.bttClaim.interactable = false;
    }
    private void OnClickDay6Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        GameManager.Instance.UserData.Gem += 100;

        _dailyData.isRewardDay[5] = true;
        notifeBtt[5].SetActive(false); 
        bttDay6.SetState(100);
        bttDay6.bttClaim.interactable = false;
    }
    private void OnClickDay7Button()
    {
        _dailyData.ResetTime = DateTime.Now.ToString();
        spriteOther.Clear();
        valueOther.Clear();

        int randCoin = Random.Range(30, 100);
        int randGem = Random.Range(10, 20);
        int randPlayer = Random.Range(0, GameManager.Instance.CharactersData.PlayerUnlocks.Count);
        int randUpgradePoint = Random.Range(1, 30);

        PlayerController player = GameManager.Instance.CharactersData.PlayerUnlocks[randPlayer];
        player.stats_SO.IncreaseUpgradePoint(randUpgradePoint);

        spriteOther.Add(spriteCoin); valueOther.Add(randCoin);
        spriteOther.Add(spriteGem); valueOther.Add(randGem);
        spriteOther.Add(player.stats_SO.Information.Skins[0].Sprite); valueOther.Add(randUpgradePoint);

        GameManager.Instance.UserData.Coin += randCoin;
        GameManager.Instance.UserData.Gem += randGem;

        _dailyData.isRewardDay[6] = true;
        notifeBtt[6].SetActive(false);
        bttDay7.SetState(spriteOther, valueOther);
        bttDay7.bttClaim.interactable = false;
    }


}
