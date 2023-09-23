using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxTrophy : MonoBehaviour
{
    public bool isGetReward;
    public int RewardCount;

    [Space]
    public Image icon;
    [HideInInspector] public List<Sprite> spriteOther = new List<Sprite>();
    [HideInInspector] public List<int> valueOther = new List<int>();

    [SerializeField] Sprite spriteCoin;
    [SerializeField] Sprite spriteGem;
    [SerializeField] Sprite spriteNormalChest;
    [SerializeField] Sprite spriteSpecialChest;
    [SerializeField] Sprite spriteCorruptedKnight;
    [SerializeField] Sprite spriteYeti;
    [Space]
    public Slider slider;
    public GameObject handleRect;
    [SerializeField] Button bttClaim;
    [Space]
    public TextMeshProUGUI textRewardName;
    [SerializeField] TextMeshProUGUI textMaxValue;
    [SerializeField] TextMeshProUGUI textCurrentValue;

    TrophyRoad _trophyRoad;
    int _currentTrophy;

    public void SetStats(int currentTrophy, TrophyRoad trophyRoad)
    {
        _currentTrophy = currentTrophy;
        _trophyRoad = trophyRoad;

        RewardCount = _trophyRoad.RewardCount;
        SetValueSlider();
        SetText();
        SetIcon();
        SetButton();
    }

    private void SetValueSlider()
    {
        slider.maxValue = _trophyRoad.MaxTrophyValue;
        slider.minValue = _trophyRoad.MinTrophyValue;
        slider.value = _currentTrophy >= slider.maxValue ? slider.maxValue : _currentTrophy;
        handleRect.SetActive(_currentTrophy >= slider.minValue && _currentTrophy < slider.maxValue);
    }
    private void SetText()
    {
        textMaxValue.text = $"{slider.maxValue}";
        textCurrentValue.text = $"{_currentTrophy}";
        textRewardName.text = $"{RewardCount} {_trophyRoad.TextReward}";
    }
    private void SetIcon()
    {
        icon.sprite = _trophyRoad.TextReward switch
        {
            "Coin"              => spriteCoin,
            "Gem"               => spriteGem,
            "Normal Chest"      => spriteNormalChest,
            "Special Chest"     => spriteSpecialChest,
            "Corrupted Knight"  => spriteCorruptedKnight,
            "Yeti"              => spriteYeti,
            _                   => null,
        };
        icon.SetNativeSize();
    }
    private void SetButton()
    {
        bttClaim.gameObject.SetActive(false);
        if (_trophyRoad.HasReward && !_trophyRoad.HasReceivedReward) // nếu có phần thưởng và chưa nhận ?
        {
            isGetReward = true;
            bttClaim.gameObject.SetActive(true);
        }
    }


    public void UpdateData(int _currentTrophy)
    {
        textCurrentValue.text = $"{_currentTrophy}";
        slider.value = _currentTrophy >= slider.maxValue ? slider.maxValue : _currentTrophy;
        handleRect.SetActive(_currentTrophy >= slider.minValue && _currentTrophy < slider.maxValue);
        SetButton();
    }


    // OnClick
    public void OnClickClaimButton(bool isGetRewardBox) // gọi trên button
    {
        isGetReward = false;
        SetRewardData();
        _trophyRoad.HasReceivedReward = true; // đánh dấu đã nhận thưởng
        bttClaim.gameObject.SetActive(false);

        if (isGetRewardBox) GetRewardBox();
    }
    public void GetRewardBox()  // gọi trên button
    {
        if (spriteOther.Count != 0 && valueOther.Count != 0)
        {
            RewardManager.Instance.GetRewardBox(spriteOther, valueOther);
        }
        else
        {
            RewardManager.Instance.GetRewardBox(icon.sprite, RewardCount);
        }
    }
    
    private void SetRewardData()
    {
        spriteOther.Clear();
        valueOther.Clear();

        int randCoin = 0, randGem = 0;
        switch (_trophyRoad.TextReward)
        {
            case "Coin":            
                GameManager.Instance.UserData.Coin += RewardCount;    
                break;

            case "Gem":             
                GameManager.Instance.UserData.Gem += RewardCount;      
                break;

            case "Normal Chest":
                randCoin = Random.Range(1, 50);
                randGem = Random.Range(1, 10);

                spriteOther.Add(spriteCoin);    valueOther.Add(randCoin);
                spriteOther.Add(spriteGem);     valueOther.Add(randGem);

                GameManager.Instance.UserData.Coin += randCoin;
                GameManager.Instance.UserData.Gem += randGem;
                break;

            case "Special Chest":   
                randCoin = Random.Range(30, 100);
                randGem = Random.Range(10, 20);
                int randPlayer = Random.Range(0, GameManager.Instance.CharactersData.PlayerUnlocks.Count);
                int randUpgradePoint = Random.Range(1, 30);
                PlayerController player = GameManager.Instance.CharactersData.PlayerUnlocks[randPlayer];
                player.stats_SO.IncreaseUpgradePoint(randUpgradePoint);

                spriteOther.Add(spriteCoin);                                    valueOther.Add(randCoin);
                spriteOther.Add(spriteGem);                                     valueOther.Add(randGem);
                spriteOther.Add(player.stats_SO.Information.Skins[0].Sprite);   valueOther.Add(randUpgradePoint);

                float progressRand = Random.value;
                if(progressRand <= .3f)
                {
                    randPlayer = Random.Range(0, GameManager.Instance.CharactersData.PlayerUnlocks.Count);
                    PlayerController playerTemp = GameManager.Instance.CharactersData.PlayerUnlocks[randPlayer];

                    List<AbilitiesEntry> _abilities = playerTemp.stats_SO.Information.AbilitiesPoint.FindAll(x => x.IsUnlock == false).ToList();
                    if(_abilities.Count > 0)
                    {
                        int _abiRand = Random.Range(0, _abilities.Count);
                        AbilityBase_SO abiliti = playerTemp.stats_SO.FindAbilities(_abilities[_abiRand].AbiName);
                        playerTemp.stats_SO.UnlockAbilitiesPoint(abiliti.AbiName);
                        spriteOther.Add(abiliti.Icon);
                        valueOther.Add(1);
                    }
                }

                GameManager.Instance.UserData.Coin += randCoin;
                GameManager.Instance.UserData.Gem += randGem;
                break;

            case "Corrupted Knight":          
                GameManager.Instance.CharactersData.UnLockCharacter("Corrupted Knight");  
                break;

            case "Yeti":
                GameManager.Instance.CharactersData.UnLockCharacter("Yeti");
                break;

            default: Debug.Log("Not Reward"); break;
        }
    }


}
