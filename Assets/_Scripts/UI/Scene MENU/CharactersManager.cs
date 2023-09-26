using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactersManager : SingletonManager<CharactersManager>, IData
{
    [SerializeField] private CharacterSelectManager CharacterSelectManager;
    [SerializeField] private GameObject PanelCharacters;

    [Space]
    [SerializeField]
    private TextMeshProUGUI textNumberOfCharacter;

    [Space]
    [SerializeField]
    private Button bttSortbyLevel;     // button sắp xếp theo: ?? 
    [SerializeField] private Button bttSortbyDamage;
    [SerializeField] private Button bttSortbyRarity;

    [Space]
    [SerializeField]
    private AvatarFrame cafPrefab; // khung avartar hiển thị thông tn character
    [SerializeField] private AvaterFrame_None caf_NonePrefab;

    [SerializeField] private Transform contentsCaf;     // spawn avartar vào đây
    [SerializeField] private Transform contentsCafNone;

    [Space]
    [SerializeField]
    private TextMeshProUGUI textUnlockedCharacterCount;
    [SerializeField] private TextMeshProUGUI textLockedCharacterCount;

    private ObjectPool<AvatarFrame> avaterFramePool;    // pool
    private ObjectPool<AvaterFrame_None> avaterFrameNonePool;

    List<PlayerController> _ControllersList; // tất cả dữ liệu của người chơi. -> được lấy từ GameManager

    List<AvatarFrame> _AvatarFrameList;
    List<AvaterFrame_None> _AvatarFrameNoneList;

    private bool isSortLevel = false;
    private bool isSortDamage = false;
    private bool isSortRarity = false;

    private float ValueScaleArrowIcon;
    [SerializeField] List<GameObject> ArrowSortButtonList;


    private void OnEnable()
    {
        Initialized();

        DataReference.Register_IData(this);

        bttSortbyLevel.onClick.AddListener(OnClickSortByLevelButton);
        bttSortbyDamage.onClick.AddListener(OnClickSortByDamageButton);
        bttSortbyRarity.onClick.AddListener(OnClickSortByRarityButton);
    }

    private void OnDestroy()
    {
        bttSortbyLevel.onClick.RemoveListener(OnClickSortByLevelButton);
        bttSortbyDamage.onClick.RemoveListener(OnClickSortByDamageButton);
        bttSortbyRarity.onClick.RemoveListener(OnClickSortByRarityButton);
    }


    private void Initialized()
    {
        _AvatarFrameList = new List<AvatarFrame>();
        _AvatarFrameNoneList = new List<AvaterFrame_None>();

        avaterFramePool = new ObjectPool<AvatarFrame>(cafPrefab, contentsCaf, 0);
        avaterFrameNonePool = new ObjectPool<AvaterFrame_None>(caf_NonePrefab, contentsCafNone, 0);

        ValueScaleArrowIcon = ArrowSortButtonList[0].transform.localScale.y;
    }
    
    public void GETData(GameManager gameManager)
    {
        _ControllersList = gameManager.CharactersData.PlayerControllers;

        LoadCharacters();
    }
    public void UpdateData()
    {
        UpdateDataCaf();
        UpdateStatsCAF();
        UpdateTextData();
        CharacterSelectManager.UpdateData();
    }

    private void LoadCharacters() // load toàn bộ data của user ra UI
    {
        if (gameObject == null || !gameObject.activeSelf || _ControllersList == null) return;

        foreach (var data in _ControllersList)
        {
            if (data.stats_SO.Information.isUnlock)
            {
                AvatarFrame caf = avaterFramePool.Get();
                caf.SetStats(data);
                caf.gameObject.SetActive(false);
                _AvatarFrameList.Add(caf);
            }
            else
            {
                AvaterFrame_None caf = avaterFrameNonePool.Get();
                caf.SetStats(data);
                caf.gameObject.SetActive(false);
                _AvatarFrameNoneList.Add(caf);
            }
        }
        UpdateTextData();
        SortByLevel();
    }
    private void UpdateTextData()
    {
        string textNumber = $"<color=#E8893F> {_ControllersList.Count} </color><color=#929EBA> / ?? </color>";
        textNumberOfCharacter.text = textNumber;
        textUnlockedCharacterCount.text = $"{_AvatarFrameList.Count}";
        textLockedCharacterCount.text = $"{_AvatarFrameNoneList.Count}";
    }
    private void UpdateDataCaf()
    {
        var players = _AvatarFrameNoneList.Where(x => x.playerController.stats_SO.Information.isUnlock).ToList();
        if(players.Count == 0) return;

        foreach (var cafNone in players)
        {
            AvatarFrame caf = avaterFramePool.Get(); 
            caf.SetStats(cafNone.playerController);
            _AvatarFrameList.Add(caf);
            _AvatarFrameNoneList.Remove(cafNone);
            cafNone.Action();
        }
        SortByLevel();
    }
    public void UpdateStatsCAF() => _AvatarFrameList.ForEach(caf => caf.UpdateStats());
    public void UpdateDataToUI(PlayerController p) // Load data của character vừa chọn ra UI
    {
        CharacterSelectManager.UpdateData(p);
        PanelCharacters.SetActive(false);
    }

    private void SortByLevel()
    {
        isSortLevel = !isSortLevel;
        
        SetNull_CAF();
        if (isSortLevel) 
        {
            ArrowSortButtonList[0].transform.localScale = new Vector2(ValueScaleArrowIcon, -ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => left.level.CompareTo(right.level));
            _AvatarFrameNoneList.Sort((left, right) => left.level.CompareTo(right.level));
        }
        else
        {
            ArrowSortButtonList[0].transform.localScale = new Vector2(ValueScaleArrowIcon, ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => right.level.CompareTo(left.level));
            _AvatarFrameNoneList.Sort((left, right) => right.level.CompareTo(left.level));
        }
        SetParent_CAF();
    }
    private void SortByDamage()
    {
        isSortDamage = !isSortDamage;

        SetNull_CAF();
        if (isSortDamage)
        {
            ArrowSortButtonList[1].transform.localScale = new Vector2(ValueScaleArrowIcon, -ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => left.damage.CompareTo(right.damage));
            _AvatarFrameNoneList.Sort((left, right) => left.damage.CompareTo(right.damage));
        }
        else
        {
            ArrowSortButtonList[1].transform.localScale = new Vector2(ValueScaleArrowIcon, ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => right.damage.CompareTo(left.damage));
            _AvatarFrameNoneList.Sort((left, right) => right.damage.CompareTo(left.damage));
        }
        SetParent_CAF();
    }
    private void SortByRarity()
    {
        isSortRarity = !isSortRarity;

        SetNull_CAF();
        if (isSortRarity)
        {
            ArrowSortButtonList[2].transform.localScale = new Vector2(ValueScaleArrowIcon, -ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => left.rarity.CompareTo(right.rarity));
            _AvatarFrameNoneList.Sort((left, right) => left.rarity.CompareTo(right.rarity));
        }
        else
        {
            ArrowSortButtonList[2].transform.localScale = new Vector2(ValueScaleArrowIcon, ValueScaleArrowIcon);
            _AvatarFrameList.Sort((left, right) => right.rarity.CompareTo(left.rarity));
            _AvatarFrameNoneList.Sort((left, right) => right.rarity.CompareTo(left.rarity));
        }
        SetParent_CAF();
    }
    private void SetNull_CAF()
    {
        foreach (var caf in _AvatarFrameList)
        {
            caf.gameObject.SetActive(false);
            caf.gameObject.transform.SetParent(null);
        }
        foreach (var caf in _AvatarFrameNoneList)
        {
            caf.gameObject.SetActive(false);
            caf.gameObject.transform.SetParent(null);
        }
    }
    private void SetParent_CAF()
    {
        foreach (var caf in _AvatarFrameList)
        {
            caf.gameObject.transform.SetParent(contentsCaf);
            caf.gameObject.SetActive(true);
        }
        foreach (var caf in _AvatarFrameNoneList)
        {
            caf.gameObject.transform.SetParent(contentsCafNone);
            caf.gameObject.SetActive(true); 
        }
    }


    #region OnClick Button

    private void OnClickSortByLevelButton() => SortByLevel();
    private void OnClickSortByDamageButton() => SortByDamage();
    private void OnClickSortByRarityButton() => SortByRarity();
    #endregion



}
