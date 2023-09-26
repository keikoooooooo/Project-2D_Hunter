using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    #region Variable
    [SerializeField] GameObject panelCharacterSelect;
    [SerializeField] Audio _audio;

    [Header("------ Middle UI ------")]
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] Image circlePrefab;
    [SerializeField] Transform slotsCircleSpawn;
    [SerializeField] Button bttChangeLeftSkin;
    [SerializeField] Button bttChangeRightSkin;
    [SerializeField] TextMeshProUGUI textUpgradeLevel;
    [SerializeField] TextMeshProUGUI textUpgradePrice;
    [SerializeField] Button bttUpgrade;
    [SerializeField] Fx_Partical fx_UpgradePrefab;
    [SerializeField] Transform SlotsFxUpgrade;
    ObjectPool<Fx_Partical> poolFxUpgrade;
    int indexCurrentSkin; // số skin đang lựa chọn -> 1 = V1, 2 = V2, ...
    int totalSkin;        // tổng số skin 
    List<Image> imagesSpawn; // lưu lại các circle show skin khi spawn ra để set color 

    [Header("------ Left UI ------")]
    [SerializeField] Sprite[] spriteRarity; 
    [SerializeField] Image frameRarity;
    [SerializeField] TextMeshProUGUI textRarity;
    [SerializeField] TextMeshProUGUI textCharacterName;
    [SerializeField] TextMeshProUGUI textCharacterLevel;
    [SerializeField] Image fillProgress;
    [SerializeField] Gradient gradient;
    [SerializeField] TextMeshProUGUI textProgress;
    [SerializeField] TextMeshProUGUI textCharacterInformation;
    [SerializeField] Button bttSelectCharacter;
    [SerializeField] GameObject bttBlockSelect;
    [SerializeField] Button bttBuySkin;

    [Header("------ Right UI ------")]
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textRange;
    [SerializeField] TextMeshProUGUI textSpeed;
    [SerializeField] TextMeshProUGUI textPopUpHealth;
    [SerializeField] TextMeshProUGUI textPopUpDamage;

    SelectAbilities selectAbilities;
    PlayerController playerController;
    PlayerInformation infor;
    #endregion


    private void Awake()
    {
        Initialized();
    }

    private void Start()
    {
        panelCharacterSelect.SetActive(false);
        bttBlockSelect.SetActive(false);
        bttBuySkin.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        RegisterEvent();
    }

    private void OnDisable()
    {
        UnRegisterEvent();
    }

    private void Initialized()
    {
        poolFxUpgrade = new ObjectPool<Fx_Partical>(fx_UpgradePrefab, SlotsFxUpgrade, 1);
        selectAbilities = GetComponent<SelectAbilities>();
    }

    private void RegisterEvent()
    {
        bttUpgrade.onClick.AddListener(OnClickUpgradeButton);
        bttChangeRightSkin.onClick.AddListener(OnClickNextSkinButton);
        bttChangeLeftSkin.onClick.AddListener(OnClickPreviousSkinButton);
        bttSelectCharacter.onClick.AddListener(OnClickSelectCharacterButton);
        bttBuySkin.onClick.AddListener(OnClickBuySkinButton);
    }

    private void UnRegisterEvent()
    {
        bttUpgrade.onClick.RemoveListener(OnClickUpgradeButton);
        bttChangeRightSkin.onClick.RemoveListener(OnClickNextSkinButton);
        bttChangeLeftSkin.onClick.RemoveListener(OnClickPreviousSkinButton);
        bttSelectCharacter.onClick.RemoveListener(OnClickSelectCharacterButton);
        bttBuySkin.onClick.RemoveListener(OnClickBuySkinButton);
    }
    public void UpdateData(PlayerController p) // khi nhấp vào 1 character bất kì -> update data của char đó ra UI
    {
        panelCharacterSelect.SetActive(true);

        //if (p == playerController) return; // nếu đã nhấp vào character này trước đó rồi -> không cần load lại data đó
        
        playerController = p;
        infor = playerController.stats_SO.Information;
        
        skeletonGraphic.skeletonDataAsset = playerController.PlayerAnimation.skeletonAnimation.skeletonDataAsset; // lấy sprite và animation của character
        indexCurrentSkin = 1;
        selectAbilities.SetState(p);
        SetStats();
        SetRarity();
        CheckUpgradeButton();
        SpawnCircleShowSkin();
        SetSkin();
        CheckInforCharacterButton();
    }
    public void UpdateData()
    {
        if(playerController == null) return;

        SetStats();
        CheckUpgradeButton();
        CheckInforCharacterButton();
    }

    private void SetStats()
    {
        var stats_SO = playerController.stats_SO;

        // Left
        textCharacterName.text = infor.CharacterName;
        textCharacterLevel.text = $"{infor.Level}";
        var maxPoint = infor.MaxUpgradePoint;
        var currentPoint = infor.CurrentUpgradePoint;
        fillProgress.fillAmount = (float)currentPoint / maxPoint;
        fillProgress.color = gradient.Evaluate(fillProgress.fillAmount);
        textProgress.text = $"{currentPoint}/{maxPoint}";
        textCharacterInformation.text = infor.CharacterInformation;

        // Middle
        totalSkin = infor.TotalSkin;
        textUpgradeLevel.text = $"Lv{infor.Level + 1} Upgrade";
        textUpgradePrice.text = $"{infor.UpgradePrice}";

        // Right
        textAttack.text = $"{stats_SO.Damage}";
        textHealth.text = $"{stats_SO.MaxHealth}";
        textRange.text = $"{stats_SO.RangeAttack}";
        textSpeed.text = $"{stats_SO.MoveSpeed}";

        if (infor.isUnlock)
        {
            bttBlockSelect.SetActive(false);
            bttSelectCharacter.gameObject.SetActive(true);
        }
        else
        {
            bttBlockSelect.SetActive(true);
            bttSelectCharacter.gameObject.SetActive(false);
        }
    }

    private void SetRarity()
    {
        switch (infor.Rarity)
        {
            case CharacterRarity.Common:
                frameRarity.sprite = spriteRarity[0];
                textRarity.text = "Common";
                break;
            case CharacterRarity.Rare:
                frameRarity.sprite = spriteRarity[1];
                textRarity.text = "Rare";
                break;
            case CharacterRarity.Epic:
                frameRarity.sprite = spriteRarity[2];
                textRarity.text = "Epic";
                break;
            case CharacterRarity.Legendary:
                frameRarity.sprite = spriteRarity[3];
                textRarity.text = "Legendary";
                break;
        }
    }

    private void SpawnCircleShowSkin()
    {
        for (int i = 0; i < slotsCircleSpawn.childCount; i++)
        {
            Destroy(slotsCircleSpawn.GetChild(i).gameObject);
        }
        imagesSpawn = new List<Image>();
        for (int i = 0; i < totalSkin; i++)
        {
            Image image = Instantiate(circlePrefab, slotsCircleSpawn);
            imagesSpawn.Add(image);
        }
    }

    private void CheckUpgradeButton()
    {
        if (infor.UpgradePrice > GameManager.Instance.UserData.Coin || infor.CurrentUpgradePoint < infor.MaxUpgradePoint)
            textUpgradePrice.color = new Color(.8f, .2f, .05f, 1); // đỏ
        else
            textUpgradePrice.color = new Color(0f, .5f, .4f, 1); // xanh
    }

    private void SetSkin()
    {
        skeletonGraphic.initialSkinName = $"V{indexCurrentSkin}";
        skeletonGraphic.Initialize(true);

        SetColorShow();
    }

    private void CheckInforCharacterButton() // check mở khóa nhân vật và mua skin ? 
    {
        if (infor.isUnlock) // nếu đã mở khóa ? 
        {
            bttBlockSelect.SetActive(false);
        
            // check mua skin ?
            if (playerController.stats_SO.Information.Skins[indexCurrentSkin - 1].Price != 0) // nếu chưa mua ??
            {
                bttSelectCharacter.gameObject.SetActive(false);
                bttBuySkin.gameObject.SetActive(true);

                TextMeshProUGUI textPrice = bttBuySkin.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                int price = playerController.stats_SO.Information.Skins[indexCurrentSkin - 1].Price;
                textPrice.text = price.ToString();
                textPrice.color = price > GameManager.Instance.UserData.Gem ? new Color(.8f, .2f, .05f, 1) : new Color(0f, .5f, .4f, 1); ;
            }
            else // đã mua ??
            {
                bttSelectCharacter.gameObject.SetActive(true);
                bttBuySkin.gameObject.SetActive(false);
            }
        }
        else  // nếu chưa mở khóa ?
        {
            bttBlockSelect.SetActive(true);
            bttSelectCharacter.gameObject.SetActive(false);
        }
    }

    private void SetColorShow()
    {
        for (var i = 1; i <= totalSkin; i++)
        {
            imagesSpawn[i - 1].color = i == indexCurrentSkin ? new Color(.67f, .67f, .67f) : new Color(.27f, .27f, .27f);
        }
    }

    private void ActiveTextPopUp()
    {
        textPopUpDamage.gameObject.SetActive(true);
        textPopUpHealth.gameObject.SetActive(true);
        Invoke(nameof(InActiveTextPopUp), .5f);
    }

    private void InActiveTextPopUp()
    {
        textPopUpDamage.gameObject.SetActive(false);
        textPopUpHealth.gameObject.SetActive(false);
    }

    #region OnClick Button
    private void OnClickUpgradeButton()
    {
        if (infor.UpgradePrice > GameManager.Instance.UserData.Coin)
        {
            NotEnough.Instance.ActiveNotEnough("Not enough coins");
            _audio.Play(AudioName.Error);
            return;
        }
        if (infor.CurrentUpgradePoint < infor.MaxUpgradePoint)
        {
            NotEnough.Instance.ActiveNotEnough("Not enough upgrade points");
            _audio.Play(AudioName.Error);
            return;
        }
        _audio.Play(AudioName.Upgrade);

        GameManager.Instance.UserData.Coin -= playerController.stats_SO.Information.UpgradePrice;
        var fx = poolFxUpgrade.Get();
        fx.SetStats(SlotsFxUpgrade.transform.position);
        playerController.stats_SO.UpgradeStats();
        ActiveTextPopUp();
        CheckUpgradeButton();
        SetStats();

        GameManager.Instance.UpdateSingleData(MenuGameManager.Instance);
        CharactersManager.Instance.UpdateStatsCAF();

        if (infor.Level >= 5 || infor.Level >= 10)
            selectAbilities.SetState(playerController);
    }
    private void OnClickSelectCharacterButton()
    {
        playerController.stats_SO.Information.CurrentSkin = indexCurrentSkin;

        GameManager.Instance.CharactersData.SetCurrentPlayer(playerController);
        MenuGameManager.Instance.OpenMenu(panelCharacterSelect);
        MenuGameManager.Instance.SetSkeletonGraphic();
    }
    private void OnClickPreviousSkinButton() // khi nhấn button để đổi skin trước đó
    {
        if (totalSkin == 1) return;

        if(indexCurrentSkin - 1 < 1)
            indexCurrentSkin = totalSkin;     
        else     
            indexCurrentSkin -= 1;

        SetSkin();
        CheckInforCharacterButton();
    }
    private void OnClickNextSkinButton()// khi nhấn button để đổi skin tiếp theo
    {
        if (totalSkin == 1) return;

        if (indexCurrentSkin + 1 > totalSkin)
            indexCurrentSkin = 1;
        else
            indexCurrentSkin += 1;

        SetSkin();
        CheckInforCharacterButton();
    }
    private void OnClickBuySkinButton()
    {
        int price = playerController.stats_SO.Information.Skins[indexCurrentSkin - 1].Price;
        if (GameManager.Instance.UserData.Gem < price)
        {
            NotEnough.Instance.ActiveNotEnough("Not enough gem");
            MenuGameManager.Instance.PlayGemPriceAnim();
            _audio.Play(AudioName.Error);
            return;
        }
        GameManager.Instance.UserData.Gem -= price;
        GameManager.Instance.UpdateSingleData(MenuGameManager.Instance);
        playerController.stats_SO.Information.Skins[indexCurrentSkin - 1].Price = 0;
        RewardManager.Instance.GetRewardBox(playerController.stats_SO.Information.Skins[indexCurrentSkin - 1].Sprite, 1);

        bttBuySkin.gameObject.SetActive(false);
        bttSelectCharacter.gameObject.SetActive(true);
    }
    #endregion


}


