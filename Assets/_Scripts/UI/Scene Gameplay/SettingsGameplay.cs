using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsGameplay : MonoBehaviour
{
    SettingsData _settingData;
    Animator animator;
    [SerializeField] LeanLocalization _localization;
    [Space]
    [SerializeField] EndGame endGame;
    [SerializeField] GameObject panelSettings;
    [Space]
    [SerializeField] Button bttCameraResizeLeft;
    [SerializeField] Button bttCameraResizeRight;
    [SerializeField] TextMeshProUGUI textValueCameraResize;
    [Space]
    [SerializeField] Button bttSoundFx;
    [SerializeField] Button bttMusic;
    [Space]
    [SerializeField] Slider sliderSoundFx;
    [SerializeField] Slider sliderMusic;
    [Space]
    [SerializeField] Image iconSoundFx;
    [SerializeField] Image iconMusic;
    [SerializeField] Sprite[] spriteSoundIcon;
    [SerializeField] Sprite[] spriteMusicIcon;
    [Space]
    [SerializeField] Button bttOnShowRange;
    [SerializeField] Button bttOffShowRange;
    [Space]
    [SerializeField] Button bttLeaveGame;
    [Space]
    [SerializeField] Button bttOtherGame;
    [SerializeField] Button bttCredits;
    [SerializeField] Button bttSupport;
    [SerializeField] Button bttHelp;

    private readonly int CodeSettingIN  = Animator.StringToHash("Setting_IN");// code chạy animation
    private readonly int CodeSettingOUT = Animator.StringToHash("Setting_OUT");
    private bool isOpenPanel;
    private int currentSizeOption;
    private bool isOnclickSound, isOnclickMusic;


    #region Private Methods
    void Awake()
    {
        Initialized();
    }
    void OnEnable()
    {
        SubscribeEvent();
    }
    void Start()
    {
        LoadData();
        panelSettings.SetActive(false);
        _localization.SetCurrentLanguage(_settingData.LanguegeName);
    }
    void OnDestroy()
    {
        UnSubscribeEvent();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpenPanel)    { CloseSetting(); }
            else                { OpenSetting();  }
        }
    }
    #endregion


    #region Publics Methods
    private void Initialized()
    {
        animator = GetComponent<Animator>();
        _settingData = GameManager.Instance ? GameManager.Instance.SettingsData : new SettingsData();
    }
    private void SubscribeEvent()
    {
        bttCameraResizeLeft.onClick.AddListener(OnClickCameraResizeLeftButton);
        bttCameraResizeRight.onClick.AddListener(OnClickCameraResizeRightButton);
        bttSoundFx.onClick.AddListener(OnClickSoundFxButton);
        bttMusic.onClick.AddListener(OnClickMusicButton);
        bttOnShowRange.onClick.AddListener(OnClickONShowRangeButton);
        bttOffShowRange.onClick.AddListener(OnClickOffShowRangeButton);
        bttLeaveGame.onClick.AddListener(OnClickLeaveGameButton);
        bttOtherGame.onClick.AddListener(OnClickOtherGameButton);
        bttCredits.onClick.AddListener(OnClickCreditsButton);
        bttSupport.onClick.AddListener(OnClickSupportButton);
        bttHelp.onClick.AddListener(OnClickHelpButton);
        
        sliderSoundFx.onValueChanged.AddListener(ChangedValueOnSoundFxSlider);
        sliderMusic.onValueChanged.AddListener(ChangedValueOnMusicSlider);
    }
    private void UnSubscribeEvent()
    {
        bttCameraResizeLeft.onClick.RemoveListener(OnClickCameraResizeLeftButton);
        bttCameraResizeRight.onClick.RemoveListener(OnClickCameraResizeRightButton);
        bttSoundFx.onClick.RemoveListener(OnClickSoundFxButton);
        bttMusic.onClick.RemoveListener(OnClickMusicButton);
        bttOnShowRange.onClick.RemoveListener(OnClickONShowRangeButton);
        bttOffShowRange.onClick.RemoveListener(OnClickOffShowRangeButton);
        bttLeaveGame.onClick.RemoveListener(OnClickLeaveGameButton);
        bttOtherGame.onClick.RemoveListener(OnClickOtherGameButton);
        bttCredits.onClick.RemoveListener(OnClickCreditsButton);
        bttSupport.onClick.RemoveListener(OnClickSupportButton);
        bttHelp.onClick.RemoveListener(OnClickHelpButton);

        sliderSoundFx.onValueChanged.RemoveListener(ChangedValueOnSoundFxSlider);
        sliderMusic.onValueChanged.RemoveListener(ChangedValueOnMusicSlider);
    }

    private void LoadData()
    {
        currentSizeOption = _settingData.valueCameraSize;
        SetCameraStats(currentSizeOption);
        isOnclickSound = _settingData.isOnclickSound;
        if (isOnclickSound)
        {
            iconSoundFx.sprite = spriteSoundIcon[0];
            sliderSoundFx.interactable = true;
        }
        else
        {
            iconSoundFx.sprite = spriteSoundIcon[1];
            sliderSoundFx.interactable = false;
        }
        isOnclickMusic = _settingData.isOnclickMusic;
        if (isOnclickMusic)
        {
            iconMusic.sprite = spriteMusicIcon[0];
            sliderMusic.interactable = true;
        }
        else
        {
            iconMusic.sprite = spriteMusicIcon[1];
            sliderMusic.interactable = false;
        }
        sliderSoundFx.value = _settingData.valueSliderSound;
        sliderMusic.value = _settingData.valueSliderMusic;
        if (_settingData.isOnClickShowRange) OnClickONShowRangeButton();
        else                                 OnClickOffShowRangeButton();


    }
    private void OpenSetting()
    {
        isOpenPanel = true;
        panelSettings.SetActive(true);
        animator.Play(CodeSettingIN);
        GamePlayManager.Instance.player.isPaused = true;
    }
    private void CloseSetting()
    {
        isOpenPanel = false;
        animator.Play(CodeSettingOUT);
        GamePlayManager.Instance.player.isPaused = false;
    }
    public void EndAnimationOut() // gọi bằng event animation
    {
        panelSettings.SetActive(false);
    }

    // OnClick Button
    private void OnClickCameraResizeLeftButton()
    {
        currentSizeOption -= 1;
        if(currentSizeOption < 1)
        {
            currentSizeOption = 3;
        }
        _settingData.valueCameraSize = currentSizeOption;
        SetCameraStats(currentSizeOption);
    }
    private void OnClickCameraResizeRightButton()
    {
        currentSizeOption += 1;
        if (currentSizeOption > 3)
        {
            currentSizeOption = 1;
        }
        _settingData.valueCameraSize = currentSizeOption;
        SetCameraStats(currentSizeOption);
    }
    private void SetCameraStats(int currentSizeOption)
    {
        textValueCameraResize.text = currentSizeOption switch
        {
            1 => "Near",
            2 => "Medium",
            3 => "Far",
            _ => throw new System.NotImplementedException()
        };
        if (GamePlayManager.Instance) GamePlayManager.Instance.SetCameraSize(currentSizeOption);
    }
    public void OnClickCloseButton()
    {
        CloseSetting();
    }
    private void OnClickSoundFxButton()
    {
        isOnclickSound = !isOnclickSound;
        if (isOnclickSound )
        {
            iconSoundFx.sprite = spriteSoundIcon[0];
            sliderSoundFx.interactable = true;
            AudioManager.Instance.Play(AudioName.OnClick);
        }
        else
        {
            iconSoundFx.sprite = spriteSoundIcon[1];
            sliderSoundFx.interactable = false;     
        }
        _settingData.isOnclickSound = isOnclickSound;
        AudioManager.Instance.Mute(AudioMode.SFX, !isOnclickSound);
    }
    private void OnClickMusicButton()
    {
        isOnclickMusic = !isOnclickMusic;

        if (isOnclickMusic)
        {
            iconMusic.sprite = spriteMusicIcon[0];
            sliderMusic.interactable = true;
        }
        else
        {
            iconMusic.sprite = spriteMusicIcon[1];
            sliderMusic.interactable = false;
        }
        _settingData.isOnclickMusic = isOnclickMusic;
        AudioManager.Instance.Mute(AudioMode.Music, !isOnclickMusic);
    }
    private void OnClickONShowRangeButton()
    {
        bttOnShowRange.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        bttOffShowRange.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        _settingData.isOnClickShowRange = true;
        GamePlayManager.Instance.player.SetAttackRangeLine(true);
    }
    private void OnClickOffShowRangeButton()
    {
        bttOnShowRange.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        bttOffShowRange.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        _settingData.isOnClickShowRange = false;
        GamePlayManager.Instance.player.SetAttackRangeLine(false);
    }
    private void OnClickLeaveGameButton()
    {
        endGame.SetStats(false);
        panelSettings.SetActive(false);
    }
    private void OnClickOtherGameButton()
    {
        // todo
    }
    private void OnClickCreditsButton()
    {
        // todo
    }
    private void OnClickSupportButton()
    {
        // todo
    }
    private void OnClickHelpButton()
    {
        // todo
    }
    private void ChangedValueOnSoundFxSlider(float value)
    {
        _settingData.valueSliderSound = value;
        AudioManager.Instance.SFXSource.volume = value;
    }
    private void ChangedValueOnMusicSlider(float value)
    {
        _settingData.valueSliderMusic = value;
        AudioManager.Instance.MusicSource.volume = value;
    }
    #endregion



}
