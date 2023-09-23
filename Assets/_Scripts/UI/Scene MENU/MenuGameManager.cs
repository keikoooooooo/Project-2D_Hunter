using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameManager : SingletonManager<MenuGameManager>, IData
{
    #region Variables
    [SerializeField] ProfileManager profileManager;
    [SerializeField] GameObject PanelMenu;
    public SkeletonGraphic skeletonGraphic;

    [Header("Top")]// tất cả thông tin trong tài khoản của user
    public Image avatar;
    public TextMeshProUGUI textUsername;
    public TextMeshProUGUI textTrophyCount;
    [Space]
    [SerializeField] TextMeshProUGUI textPower;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textGem;

    [SerializeField] Animator animatorPower;
    [SerializeField] Animator animatorCoin;
    [SerializeField] Animator animatorGem;

    static readonly int powerCodeAnim = Animator.StringToHash("PowerBar");
    static readonly int coinCodeAnim = Animator.StringToHash("CoinBar");
    static readonly int gemCodeAnim = Animator.StringToHash("GemBar");

    [Space]
    [SerializeField] GameObject panelNotiPower; // panel hồi power
    [SerializeField] TextMeshProUGUI textPowerRecoveryTime;

    [Header("Left")]
    [SerializeField] TextMeshProUGUI textValueChestReward;
    [SerializeField] Slider sliderChestReward;
    [SerializeField] Button bttClaimChestReward;
    [SerializeField] GameObject notiClaimChestReward;
    [SerializeField] Sprite spriteCoin;
    [SerializeField] Sprite spriteGem;

    [Header("Middle")]
    public Image iconNextReward;
    public Image fillNextReward;
    public TextMeshProUGUI textCurrentTrophyValue;
    [SerializeField] GameObject bttTrophy;
    bool isOpenPanelNotiPower;

    UserData _userData;
    CharactersData _charactersData;
    #endregion

    void OnEnable()
    {
        DataReference.Register_IData(this);
    }
    void Start()
    {
        AudioManager.Instance.Play(AudioName.Menu);

        isOpenPanelNotiPower = false;
        textPowerRecoveryTime.text = "00M\n00S";
        panelNotiPower.SetActive(false);
        PanelMenu.SetActive(true);
        GameManager.Instance.SendData();  
        GameManager.Instance.UpdateMultiData();  // yêu cầu gửi data 
        GameManager.Instance.E_PowerRecoveryTime += TextPowerRecoveryTime;
    }
    void OnDisable()
    {
       if (GameManager.Instance) GameManager.Instance.E_PowerRecoveryTime -= TextPowerRecoveryTime;
    }

    #region Public 
    public void GETData(GameManager gameManager)
    {
        _userData = gameManager.UserData;
        _charactersData = gameManager.CharactersData;

        SetTextProfile();
        SetSkeletonGraphic();
        SetChestReward();
    }
    public void UpdateData()
    {
        SetTextProfile();
        SetChestReward();
    }

    void SetTextProfile() // cập nhật data ra UI
    {
        textUsername.text = _userData.Username;
        textUsername.color = _userData.LastUsedColor.HexToColor();
        textTrophyCount.text = $"{_userData.CurrentTrophyCount}";
        textPower.text = $"{_userData.Power}/200";
        textCoin.text = $"{_userData.Coin}";
        textGem.text = $"{_userData.Gem}";
    }
    public void SetSkeletonGraphic() 
    {
        skeletonGraphic.skeletonDataAsset = _charactersData.PlayerController.PlayerAnimation.skeletonAnimation.skeletonDataAsset;
        skeletonGraphic.initialSkinName = $"V{_charactersData.LastUsedCharacterSkin}";
        skeletonGraphic.Initialize(true);
    }
    void SetChestReward()
    {
        int token = _userData.Token;
        textValueChestReward.text = $"{token}/10000";
        sliderChestReward.minValue = 0;
        sliderChestReward.maxValue = 10000;
        sliderChestReward.value = token >= 10000 ? 10000 : token;
        bttClaimChestReward.interactable = token >= 10000;
        notiClaimChestReward.SetActive(token >= 10000);
    }
    public void SetNextReward(Sprite spriteNextReward, float fillAmount, int valueTrophy)
    {
        iconNextReward.sprite = spriteNextReward;
        iconNextReward.SetNativeSize();
        fillNextReward.fillAmount = fillAmount;
        textCurrentTrophyValue.text = $"{valueTrophy}";
    }

    public void OpenMenu(GameObject PanelOther)
    {
        PanelMenu.SetActive(true);
        bttTrophy.SetActive(true);
        PanelOther.SetActive(false);
    }
    public void CloseMenu(GameObject PanelOther)
    {
        PanelOther.SetActive(true);
        bttTrophy.SetActive(false);
        PanelMenu.SetActive(false);
    }



    // Onclick Button
    public void OnClickOpenTrophyRoadButton()
    {
        TrophyRoadManager.Instance.OnClickOpenPanelTrophyButton();
        AudioManager.Instance.Play(AudioName.OnClick);
    }
    public void OnClickBattleButton()
    {
        if(_userData.Power < 30)
        {
            PlayPowerPriceAnim();
            return;
        }
        _userData.Power -= 30;
        GameManager.Instance.SubtractPower();
        LoadingManager.Instance.LoadScene("Gameplay");
        AudioManager.Instance.Play(AudioName.OnClick);
    }
    public void OnClickOpenNotiPanelPowerRecoveryTimeButton()
    {
        isOpenPanelNotiPower = !isOpenPanelNotiPower;

        if (isOpenPanelNotiPower)
        {
            panelNotiPower.SetActive(true);
            Invoke(nameof(InActivePanel), 5f);
        }
        else
        {
            panelNotiPower.SetActive(false);
        }
        AudioManager.Instance.Play(AudioName.OnClick);
    }
    private void InActivePanel()
    {
        panelNotiPower.SetActive(false);
    }
    private void TextPowerRecoveryTime(int time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        textPowerRecoveryTime.text = string.Format("{0:00}M\n{1:00}S", minutes, seconds);

    }
    public void OnClickChestRewardButton()
    {
        _userData.Token -= 10000;

        int coin = Random.Range(100, 500);
        int gem = Random.Range(20, 80);
        int upgradePoint1 = Random.Range(15, 30);
        int upgradePoint2 = Random.Range(25, 50);
        int countPlayerUnlock = _charactersData.PlayerUnlocks.Count;

        int val1 = Random.Range(0, countPlayerUnlock);
        int val2 = Random.Range(0, countPlayerUnlock);

        PlayerController p1, p2;     

        while (true)
        {
            if (countPlayerUnlock <= 1)
            {
                p1 = _charactersData.PlayerUnlocks[val1];
                p2 = null;
                upgradePoint1 *= 2;
                break;
            }
            if (val1 != val2)
            {
                p1 = _charactersData.PlayerUnlocks[val1];
                p2 = _charactersData.PlayerUnlocks[val2];
                break;
            }
            val2 = Random.Range(0, countPlayerUnlock);
        }
        List<Sprite> sprites = new List<Sprite>();
        List<int> value = new List<int>();

        sprites.Add(spriteCoin);
        sprites.Add(spriteGem);
        sprites.Add(p1.stats_SO.Information.Skins[0].Sprite);
        value.Add(coin);
        value.Add(gem);
        value.Add(upgradePoint1);

        _userData.Coin += coin;
        _userData.Gem += gem;
        p1.stats_SO.IncreaseUpgradePoint(upgradePoint1);
        if (p2 != null)
        {
            sprites.Add(p2.stats_SO.Information.Skins[0].Sprite);
            value.Add(upgradePoint2);
            p2.stats_SO.IncreaseUpgradePoint(upgradePoint2);
        }

        SetChestReward();
        RewardManager.Instance.GetRewardBox(sprites, value);
        AudioManager.Instance.Play(AudioName.OnClick);
    }
    public void OnClickSelectPlayerButton()
    {
        bttTrophy.SetActive(false);
        PanelMenu.SetActive(false);

        CharactersManager.Instance.UpdateDataToUI(_charactersData.PlayerController);
        AudioManager.Instance.Play(AudioName.OnClick);
    }

    private void PlayPowerPriceAnim() => animatorPower.Play(powerCodeAnim);
    public void PlayCoinPriceAnim() => animatorCoin.Play(coinCodeAnim);
    public void PlayGemPriceAnim() => animatorGem.Play(gemCodeAnim);
    #endregion



}
