using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : SingletonManager<ProfileManager>, IData
{
    public Image avatar;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] private Button bttNameColor;
    [SerializeField] TextMeshProUGUI textNameColor;
    [Space]
    [SerializeField]
    private TextMeshProUGUI textTrophy;
    [SerializeField] private TextMeshProUGUI textCharacter;
    [SerializeField] private TextMeshProUGUI textPlayingTime;
    [SerializeField] private TextMeshProUGUI textTotalPlay;
    [Space]
    [SerializeField]
    private GameObject panelNameColor;
    [SerializeField] private Transform contentSpawnBoxColor;
    [SerializeField] private BoxNameColor boxNameColorPrefab;
    private List<BoxNameColor> boxNameColorList;
    [Space]
    [SerializeField]
    private GameObject panelAvatar;
    [SerializeField] private Sprite[] avatarList;
    [SerializeField] private Transform contentSpawnBoxAvatar;
    [SerializeField] BoxAvatarUser boxAvatarUserPrefab;
    private List<BoxAvatarUser> boxAvatarUserList;


    private UserData _userData;


    #region Private Methods
    void OnEnable()
    {
        Initialized();
    }
    #endregion


    #region Public Methods
    public void GETData(GameManager gameManager)
    {
        _userData = gameManager.UserData;
        SetData();
        UpdateData();
    }
    public void UpdateData()
    {
        SetData();
        textTrophy.text = $"{_userData.CurrentTrophyCount}";
        textCharacter.text = $"{_userData.CharacterCount}";
        textTotalPlay.text = $"{_userData.TotalPlay}";
        textNameColor.text = $"{_userData.LastNameColor}";

        long TotalSeconds = _userData.PlayingTime;
        long minutes = Mathf.FloorToInt(TotalSeconds / 60);
        long seconds = Mathf.FloorToInt(TotalSeconds % 60);
        string time = string.Format("{0:0}M : {1:00}S", minutes, seconds);
        textPlayingTime.text = time;
        UpdateButtonNameColor();
    }
    void Initialized()
    {
        DataReference.Register_IData(this);
        boxNameColorList = new List<BoxNameColor>();
        for (int i = 0; i < 7; i++)
        {
            var box = Instantiate(boxNameColorPrefab, contentSpawnBoxColor);
            box.gameObject.SetActive(true);
            boxNameColorList.Add(box);
        }
        boxAvatarUserList = new List<BoxAvatarUser>();
        for (int i = 0; i < 10; i++)
        {
            var box = Instantiate(boxAvatarUserPrefab, contentSpawnBoxAvatar);
            box.gameObject.SetActive(true);
            box.SetStats(i, avatarList[i]);
            boxAvatarUserList.Add(box);
        }
    }
    public void SetData()
    {
        avatar.sprite = avatarList[_userData.LastAvatarIndex];
        MenuGameManager.Instance.avatar.sprite = avatar.sprite;

        inputField.text = _userData.Username;
        inputField.textComponent.color = _userData.LastUsedColor.HexToColor();
    }

    public void Rename() // khi đổi tên trên inputField
    {
        _userData.Username = inputField.text;
        MenuGameManager.Instance.textUsername.text = inputField.text;
    }


    // Onlick Button
    public void OnClickOpenPanelNameColorButton()
    {
        panelNameColor.SetActive(true);
        UpdateButtonNameColor();
    }
    private void UpdateButtonNameColor()
    {
        if (!panelNameColor.activeSelf) return;

        for (int i = 0; i < boxNameColorList.Count; i++)
        {
            bool isBuy = _userData.PurchasedColors[i].Length > 0;
            boxNameColorList[i].SetStats(i, _userData.Username, _userData.LastUsedColor, _userData.Gem, isBuy);
        }
    }
    public void OnClickOpenPanelAvatar()
    {
        panelAvatar.SetActive(true);
        
    }
    public void OnClickUseBoxNameColor(Color color, string nameColor)
    {
        inputField.textComponent.color = color;
        textNameColor.text = nameColor;
        MenuGameManager.Instance.textUsername.color = color;

        UpdateButtonNameColor();
    }
    #endregion

}
