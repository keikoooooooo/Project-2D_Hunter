using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IData
{
    [Header("Reference")]
    [SerializeField]
    private GameObject panelShop;
    [Space]
    [SerializeField]
    private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textGem;
    [SerializeField] private Animator animatorCoin;
    [SerializeField] private Animator animatorGem;

    private static readonly int coinCodeAnim = Animator.StringToHash("CoinBar");
    private static readonly int gemCodeAnim = Animator.StringToHash("GemBar");

    [Space]
    [SerializeField]
    private RectTransform rectTransformContent;
    [Space]
    [SerializeField]
    private List<ButtonClaimReward> buttonClaimDaily;
    [SerializeField] private List<ButtonClaimReward> buttonClaimCoin;
    [SerializeField] private List<ButtonClaimReward> buttonClaimGem;

    private List<PlayerController> _playersUnlock;
    private List<PlayerController> _playersNonUnlock;

    private PlayerController playerController;

    private ShopData _shopData;
    private UserData _userData;

    private DateTime resetTime;
    private DateTime currentTime;
    private long timeThrough;   // thời gian đã trôi qua (giây)

    private void Awake() => Initialized();
    private void OnEnable() => RegisterEvent();
    private void OnDisable() => UnRegisterEvent();


    private void Initialized()
    {
        DataReference.Register_IData(this);
        _playersUnlock = new List<PlayerController>();
        _playersNonUnlock = new List<PlayerController>();
    }

    private void RegisterEvent()
    {
        buttonClaimDaily[0].bttClaim.onClick.AddListener(OnClickFreeCoinButton);
        buttonClaimDaily[1].bttClaim.onClick.AddListener(OnClickBuyPowerButton);
        buttonClaimDaily[2].bttClaim.onClick.AddListener(OnClickBuyCharacterButton);
        buttonClaimDaily[3].bttClaim.onClick.AddListener(OnClickBuyUpgradePointButton);

        buttonClaimDaily[0].GetComponent<CountdownTime>().E_EndCountdownTime += ResetRewardFreeCoin;
        buttonClaimDaily[1].GetComponent<CountdownTime>().E_EndCountdownTime += ResetBuyPower;
        buttonClaimDaily[3].GetComponent<CountdownTime>().E_EndCountdownTime += ResetBuyUpgradePoint;

        buttonClaimCoin[0].bttClaim.onClick.AddListener(OnClickCoinOption1Button);
        buttonClaimCoin[1].bttClaim.onClick.AddListener(OnClickCoinOption2Button);
        buttonClaimCoin[2].bttClaim.onClick.AddListener(OnClickCoinOption3Button);
    }

    private void UnRegisterEvent()
    {
        buttonClaimDaily[0].bttClaim.onClick.RemoveListener(OnClickFreeCoinButton);
        buttonClaimDaily[1].bttClaim.onClick.RemoveListener(OnClickBuyPowerButton);
        buttonClaimDaily[2].bttClaim.onClick.RemoveListener(OnClickBuyCharacterButton);
        buttonClaimDaily[3].bttClaim.onClick.RemoveListener(OnClickBuyUpgradePointButton);

        buttonClaimDaily[0].GetComponent<CountdownTime>().E_EndCountdownTime -= ResetRewardFreeCoin;
        buttonClaimDaily[1].GetComponent<CountdownTime>().E_EndCountdownTime -= ResetBuyPower;
        buttonClaimDaily[3].GetComponent<CountdownTime>().E_EndCountdownTime -= ResetBuyUpgradePoint;

        buttonClaimCoin[0].bttClaim.onClick.RemoveListener(OnClickCoinOption1Button);
        buttonClaimCoin[1].bttClaim.onClick.RemoveListener(OnClickCoinOption2Button);
        buttonClaimCoin[2].bttClaim.onClick.RemoveListener(OnClickCoinOption3Button);
    }


    public void GETData(GameManager gameManager)
    {
        _userData = gameManager.UserData;
        _shopData = gameManager.ShopData;

        foreach (var player in gameManager.CharactersData.PlayerControllers)
        {
            if (player.stats_SO.Information.isUnlock)
            {
                _playersUnlock.Add(player);
            }
            else
            {
                _playersNonUnlock.Add(player);
            }
        }
    }
    public void UpdateData()
    {
        FindTime();
        SetText();

        if (currentTime.Date > resetTime.Date)
        {
            _shopData.ResetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0).AddDays(1).ToString();

            //Debug.Log("Đủ 24h");
            int valueRand = Random.Range(0, _playersUnlock.Count);         
            playerController = _playersUnlock[valueRand];
            _shopData.PlayerCurrent = playerController.stats_SO.Information.CharacterName;

            if (panelShop.activeSelf)
            {
                // reset daily
                ResetRewardFreeCoin();
                ResetBuyPower();
                ResetBuyUpgradePoint();
            }
        }
        else
        {
            string playerCurrent = _shopData.PlayerCurrent;
            playerController = _playersUnlock.Find(x => x.stats_SO.Information.CharacterName == playerCurrent);

            if (panelShop.activeSelf)
            {
                SetStateClaimFreeCoinButton();
                SetStateBuyPowerButton();
                SetStateBuyUpgradePointButton();
            }

            //Debug.Log("Chưa đủ 24h");Debug.Log($"Cần {Mathf.FloorToInt(timeThrough / 3600)} giờ");Debug.Log($"Cần {Mathf.FloorToInt(timeThrough / 60) % 60} phút"); Debug.Log($"Cần {Mathf.FloorToInt(timeThrough % 60)} giây");
        }

        PlayerInformation inforCurrent = playerController.stats_SO.Information; // tạo vật phẩm upgrade ngẫu nhiên để user mua
        buttonClaimDaily[3].icon.sprite = inforCurrent.Skins[0].Sprite;
    }

    private void FindTime() // tìm thời gian và check đã qua 0h đêm chưa?
    {
        resetTime = DateTime.Parse(_shopData.ResetTime);
        currentTime = DateTime.Now;

        TimeSpan time = resetTime - currentTime;
        timeThrough = (long)time.TotalSeconds;
    }

    private void SetText()
    {
        textCoin.text = $"{_userData.Coin}";
        textGem.text = $"{_userData.Gem}";
    }

    private void SetStateClaimFreeCoinButton() // set trạng thái của button 
    {
        if (_shopData.IsFreeCoinClaim)
        {
            buttonClaimDaily[0].bttClaim.interactable = true;
        }
        else
        {
            buttonClaimDaily[0].bttClaim.interactable = false;
            if (buttonClaimDaily[0].gameObject.activeSelf && buttonClaimDaily[0].TryGetComponent<CountdownTime>(out var countdownTime)) 
                countdownTime.StartCountDown((int)timeThrough);
        }
    }

    private void SetStateBuyPowerButton()
    {
        if (_shopData.CountBuyPower > 0)
        {
            buttonClaimDaily[1].bttClaim.interactable = true;
        }
        else
        {
            buttonClaimDaily[1].bttClaim.interactable = false;
            if (buttonClaimDaily[1].gameObject.activeSelf && buttonClaimDaily[1].TryGetComponent<CountdownTime>(out var countdownTime))    
                countdownTime.StartCountDown((int)timeThrough);
        }
    }

    private void SetStateBuyUpgradePointButton()
    {
        if (_shopData.IsBuyUpgradePoint)
        {
            buttonClaimDaily[3].bttClaim.interactable = true;
        }
        else
        {
            buttonClaimDaily[3].bttClaim.interactable = false;
            if (buttonClaimDaily[3].gameObject.activeSelf && buttonClaimDaily[3].TryGetComponent<CountdownTime>(out var countdownTime))
                countdownTime.StartCountDown((int)timeThrough);
        }
    }

    private void ResetRewardFreeCoin() // reset button để mua tiếp 
    {
        _shopData.IsFreeCoinClaim = true;
        buttonClaimDaily[0].bttClaim.interactable = true;
        if (buttonClaimDaily[0].gameObject.activeSelf && buttonClaimDaily[0].TryGetComponent<CountdownTime>(out var countdownTime)) 
            countdownTime.StopCountDown();
    }
    private void ResetBuyPower()
    {
        _shopData.CountBuyPower = 5;
        buttonClaimDaily[1].gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "5/5";
        if (buttonClaimDaily[1].gameObject.activeSelf && buttonClaimDaily[1].TryGetComponent<CountdownTime>(out var countdownTime))
            countdownTime.StopCountDown();
    }
    private void ResetBuyUpgradePoint()
    {
        _shopData.IsBuyUpgradePoint = true;
        buttonClaimDaily[3].bttClaim.interactable = true;
        if (buttonClaimDaily[3].gameObject.activeSelf && buttonClaimDaily[3].TryGetComponent<CountdownTime>(out var countdownTime))
            countdownTime.StopCountDown();
    }



    #region Onclick Button: Daily
    private void OnClickFreeCoinButton()
    {
        _shopData.ResetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0).AddDays(1).ToString();
        FindTime();
        _shopData.IsFreeCoinClaim = false;
        SetStateClaimFreeCoinButton();

        int randomCoin = Random.Range(5, 50);
        _userData.Coin += randomCoin;
        buttonClaimDaily[0].SetState(randomCoin);
    }
    private void OnClickBuyPowerButton()
    {
        if (_userData.Gem < 20)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        FindTime();
        
        _userData.Gem -= 20;
        _userData.IncreasePower(20);
        _shopData.CountBuyPower -= 1;
        
        SetStateBuyPowerButton();

        buttonClaimDaily[1].SetState(20);
        buttonClaimDaily[1].gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"{_shopData.CountBuyPower}/5";
    }
    private void OnClickBuyCharacterButton()
    {
        if(_userData.Gem < 500)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        _userData.Gem -= 500;
        if(_playersNonUnlock.Count == 0) // đã mua hết character ? -> chỉ nhận mảnh upgrade
        {
            int rand = Random.Range(0, _playersUnlock.Count);
            PlayerController player = _playersUnlock[rand];
            player.stats_SO.IncreaseUpgradePoint(25);
            buttonClaimDaily[2].SetState(player.stats_SO.Information.Skins[0].Sprite, 25);
        }
        else
        {
            int rand = Random.Range(0, _playersNonUnlock.Count);
            PlayerController player = _playersNonUnlock[rand];
            GameManager.Instance.CharactersData.UnLockCharacter(player.stats_SO.Information.CharacterName);
            _userData.CharacterCount += 1;
            _playersUnlock.Add(player);
            _playersNonUnlock.Remove(player);
            buttonClaimDaily[2].SetState(player.stats_SO.Information.Skins[0].Sprite, 1);

        }
    }
    private void OnClickBuyUpgradePointButton()
    {
        if (_userData.Coin < 150)
        {
            NotEnough.Instance.ActiveNotEnough("Not enough coins");
            AudioManager.Instance.Play(AudioName.Error);
            animatorCoin.Play(coinCodeAnim);
            return;
        }
        _shopData.ResetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0).AddDays(1).ToString();
        _userData.Coin -= 150;
        FindTime();
        _shopData.IsBuyUpgradePoint = false;
        SetStateBuyUpgradePointButton();

        playerController.stats_SO.IncreaseUpgradePoint(15);
        buttonClaimDaily[3].SetState(playerController.stats_SO.Information.Skins[0].Sprite, 15);
    }
    #endregion

    #region Onclick Button: Coin
    private void OnClickCoinOption1Button()
    {
        if (_userData.Gem < 100)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        _userData.Gem -= 100;
        _userData.Coin += 500;
        buttonClaimCoin[0].SetState(500);
    }
    private void OnClickCoinOption2Button()
    {
        if (_userData.Gem < 300)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        _userData.Gem -= 300; 
        _userData.Coin += 1700;
        buttonClaimCoin[1].SetState(1700);
    }
    private void OnClickCoinOption3Button()
    {
        if (_userData.Gem < 500)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        _userData.Gem -= 500;
        _userData.Coin += 2800;
        buttonClaimCoin[2].SetState(2800);
    }
    #endregion

    #region  Onclick Button: Gem

    #endregion


    public void OnClickScrollToDaily(float posY) => rectTransformContent.DOAnchorPosY(posY, .7f).SetEase(Ease.Linear);
    public void OnClickScrollToCoinPack(float posY) => rectTransformContent.DOAnchorPosY(posY, .7f).SetEase(Ease.Linear);
    public void OnClickScrollToGemPack(float posY) => rectTransformContent.DOAnchorPosY(posY, .7f).SetEase(Ease.Linear);

    public void OnClickResetBuyUpgradePointButton()
    {
        if(_userData.Gem < 25)
        {
            animatorGem.Play(gemCodeAnim);
            NotEnough.Instance.ActiveNotEnough("Not enough gems");
            AudioManager.Instance.Play(AudioName.Error);
            return;
        }

        _userData.Gem -= 25;
        SetText();
        GameManager.Instance.UpdateSingleData(MenuGameManager.Instance);
        ResetBuyUpgradePoint();
    }
}


