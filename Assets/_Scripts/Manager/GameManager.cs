using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManager : SingletonManager<GameManager>
{
    public event Action<int> E_PowerRecoveryTime;
    Coroutine powerRecoveryTimeCoroutine;

    public bool isEncrypt;

    int timePlay; // thời gian chơi
    private List<IData> _dataReference;
    private bool isLoadData;

    [SerializeField] private PlayerData_SO _playerData_SO;
    [SerializeField] private EnemyData_SO _enemyData_SO;

    [SerializeField] private TextAsset _trophyRequirementsData; // data_ số cúp yêu cầu
    [SerializeField] private TextAsset _trophyRewardsData;      // data_ số phần thưởng
    [SerializeField] private TextAsset _trophyRewardNames;      // data_ tên phần thưởng

    public UserData UserData { get; private set; }
    public CharactersData CharactersData { get; private set; }
    public TrophyRoadData TrophyRoadData { get; private set; }
    public BestiaryData BestiaryData { get; private set; }
    public SettingsData SettingsData { get; private set; }
    public ShopData ShopData { get; private set; }
    public DailyData DailyData { get; private set; }


    #region Private Methods

    private void Start()
    {
        Initialized();
        LoadData();
        UpdateData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
        DataReference.ClearData();
        Debug.Log("Save Data Success");
    }
    #endregion


    #region Data Game
    private void LoadData()
    {
        var _userData = FileHandler.Load<UserData>(FileNameData.UserData, isEncrypt);
        var _characterData = FileHandler.Load<CharactersData>(FileNameData.CharacterData, isEncrypt);
        var _trophyRoadData = FileHandler.Load<TrophyRoadData>(FileNameData.TrophyRoad, isEncrypt);
        var _bestiaryData = FileHandler.Load<BestiaryData>(FileNameData.BestaryData, isEncrypt);
        var _settingData = FileHandler.Load<SettingsData>(FileNameData.SettingsData, isEncrypt);
        var _shopData = FileHandler.Load<ShopData> (FileNameData.ShopData, isEncrypt);
        var _dailyData = FileHandler.Load<DailyData> (FileNameData.DailyData, isEncrypt);

        UserData = _userData ?? new UserData();
        CharactersData = _characterData ?? new CharactersData(_playerData_SO);
        TrophyRoadData = _trophyRoadData ?? new TrophyRoadData(_trophyRequirementsData, _trophyRewardsData, _trophyRewardNames);
        BestiaryData = _bestiaryData ?? new BestiaryData(_enemyData_SO);
        SettingsData = _settingData ?? new SettingsData();
        ShopData = _shopData ?? new ShopData();
        DailyData = _dailyData ?? new DailyData();

        Debug.Log("Load Data Success");
    }
    private void UpdateData()
    {
        UserData.CheckNewAccount();
        UserData.FindRewardOffline();
        CharactersData.LoadData(_playerData_SO);
        BestiaryData.LoadData(_enemyData_SO);
        UserData.CurrentTrophyCount = TrophyRoadData.CurrentTrophyCount;
        UserData.CharacterCount = CharactersData.CharacterUnlock();
        if(UserData.Power < 200)  powerRecoveryTimeCoroutine = StartCoroutine(IncreasePowerCoroutine());
        isLoadData = true;

        LoadingManager.Instance.LoadScene(UserData.isNewAccount ? "CutScene" : "MenuGame");
    }
    public void SaveData()
    {
        UserData.LastQuitTime = DateTime.Now.ToString();
        UserData.PlayingTime += timePlay;
        FileHandler.Save(UserData, FileNameData.UserData, isEncrypt);
        CharactersData.SaveData();
        FileHandler.Save(CharactersData, FileNameData.CharacterData, isEncrypt);
        FileHandler.Save(TrophyRoadData, FileNameData.TrophyRoad, isEncrypt);
        FileHandler.Save(BestiaryData, FileNameData.BestaryData, isEncrypt);
        FileHandler.Save(SettingsData, FileNameData.SettingsData, isEncrypt);
        FileHandler.Save(ShopData, FileNameData.ShopData, isEncrypt);
        FileHandler.Save(DailyData, FileNameData.DailyData, isEncrypt);
    }
    public void SendData()
    {
        if (!isLoadData) return;

        _dataReference = DataReference.GetAll_IData();
        _dataReference.ForEach(data => data.GETData(this)); // gửi data tới các class có đăng kí nhận data?
    }
    public void UpdateMultiData()
    {
        if (!isLoadData) return;

        UserData.CurrentTrophyCount = TrophyRoadData.CurrentTrophyCount;
        UserData.CharacterCount = CharactersData.CharacterUnlock();

        _dataReference = DataReference.GetAll_IData();
        _dataReference.ForEach(data => data.UpdateData());
    }
    public void UpdateSingleData(IData IdataType)
    {
        if (!isLoadData || IdataType == null) return;

        UserData.CurrentTrophyCount = TrophyRoadData.CurrentTrophyCount;
        UserData.CharacterCount = CharactersData.CharacterUnlock();

        IdataType.UpdateData();
    }
    #endregion


    #region Public Methods
    private void Initialized()
    {
        _playerData_SO.Initialized();
        _enemyData_SO.Initialized();
        _dataReference = DataReference.GetAll_IData();

        StartCoroutine(PlayTimeCoroutine());
    }

    private IEnumerator PlayTimeCoroutine()
    {
        while (true)
        {
            timePlay += 1;
            if(timePlay >= 30) // mỗi 10 giây -> sẽ lưu tất cả dữ liệu vào file 1 lần
            {
                UserData.PlayingTime += timePlay;
                timePlay= 0;
                SaveData();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void SubtractPower()
    {
        if (powerRecoveryTimeCoroutine != null) StopCoroutine(powerRecoveryTimeCoroutine);
        StartCoroutine(IncreasePowerCoroutine());
    }

    private IEnumerator IncreasePowerCoroutine()
    {
        var time = 100;
        while (time > 0)
        {
            time -= 1;
            E_PowerRecoveryTime?.Invoke(time);
            yield return new WaitForSeconds(1f);
        }

        if (UserData.IsIncreasePower())
        {
            UserData.Power += 1;
            UpdateSingleData(MenuGameManager.Instance);
            StartCoroutine(IncreasePowerCoroutine());
        }
        else
        {
            if(powerRecoveryTimeCoroutine != null)
            {
                StopCoroutine(powerRecoveryTimeCoroutine);
            }
        }
    }

    public void UpdateDataInBestiary(EnemyController enemyController) => 
        BestiaryData.IncreaseKill(enemyController.stats_SO.Information.EnemyName);
    #endregion





}
