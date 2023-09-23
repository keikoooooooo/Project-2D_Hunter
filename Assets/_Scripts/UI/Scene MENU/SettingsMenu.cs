using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, IData
{
    [SerializeField] LeanLocalization leanLocalization;
    SettingsData _settingData;
    [SerializeField] GameObject panelSettings;
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
    [SerializeField] Image iconLanguage;
    [SerializeField] Button bttLanguageLeftButton;
    [SerializeField] Button bttLanguageRightButton;
    [SerializeField] Sprite[] spriteLanguges;
    [Space]
    [SerializeField] Button bttOtherGame;
    [SerializeField] Button bttTermsOfService;
    [SerializeField] Button bttPrivacyPolicy;
    [SerializeField] Button bttSupport;
    [SerializeField] Button bttQuit;

    private bool isOnclickSound, isOnclickMusic;
    int languageOption = 0;


    void OnEnable()
    {
        Initialized();
        SubscribeEvent();
    }
    void OnDisable()
    {
        UnSubscribeEvent();
    }



    public void GETData(GameManager gameManager)
    {
        _settingData = gameManager.SettingsData;
        LoadData();
    }
    public void UpdateData() 
    {
        LoadData();
    }
    private void LoadData()
    {
        string languegeName = _settingData.LanguegeName;
        languageOption = _settingData.LanguageOption;
        iconLanguage.sprite = spriteLanguges[languageOption]; 
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
        leanLocalization.SetCurrentLanguage(languegeName);


        AudioManager.Instance.SFXSource.mute = !isOnclickSound;
        AudioManager.Instance.MusicSource.mute = !isOnclickMusic; 

        AudioManager.Instance.SFXSource.volume = sliderSoundFx.value;
        AudioManager.Instance.MusicSource.volume = sliderMusic.value;
    }
    void Initialized()
    {
        DataReference.Register_IData(this);
    }
    private void SubscribeEvent()
    {
        bttSoundFx.onClick.AddListener(OnClickSoundFxButton);
        bttMusic.onClick.AddListener(OnClickMusicButton);
        bttLanguageLeftButton.onClick.AddListener(OnClickLanguageLeftButton);
        bttLanguageRightButton.onClick.AddListener(OnClickLanguageRightButton);
        bttOtherGame.onClick.AddListener(OnClickOtherGameButton);
        bttTermsOfService.onClick.AddListener(OnClickTermsOfServiceButton);
        bttPrivacyPolicy.onClick.AddListener(OnClickPrivacyPolicyButton);
        bttSupport.onClick.AddListener(OnClickSupportButton);
        bttQuit.onClick.AddListener(OnClickQuitButton);

        sliderSoundFx.onValueChanged.AddListener(ChangedValueOnSoundFxSlider);
        sliderMusic.onValueChanged.AddListener(ChangedValueOnMusicSlider);
    }
    private void UnSubscribeEvent()
    {
        bttSoundFx.onClick.RemoveListener(OnClickSoundFxButton);
        bttMusic.onClick.RemoveListener(OnClickMusicButton);
        bttLanguageLeftButton.onClick.RemoveListener(OnClickLanguageLeftButton);
        bttLanguageRightButton.onClick.RemoveListener(OnClickLanguageRightButton);
        bttOtherGame.onClick.RemoveListener(OnClickOtherGameButton);
        bttTermsOfService.onClick.RemoveListener(OnClickTermsOfServiceButton);
        bttPrivacyPolicy.onClick.RemoveListener(OnClickPrivacyPolicyButton);
        bttSupport.onClick.RemoveListener(OnClickSupportButton);
        bttQuit.onClick.RemoveListener(OnClickQuitButton);

        sliderSoundFx.onValueChanged.RemoveListener(ChangedValueOnSoundFxSlider);
        sliderMusic.onValueChanged.RemoveListener(ChangedValueOnMusicSlider);
    }
    void CloseSetting()
    {
        panelSettings.SetActive(false);

    }

    
    // OnClick Button
    public void OnClickCloseButton()
    {
        CloseSetting();
    }
    private void OnClickSoundFxButton()
    {
        isOnclickSound = !isOnclickSound;
        if (isOnclickSound)
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
    private void OnClickLanguageLeftButton()
    {
        languageOption -= 1;
        if(languageOption < 0)
        {
            languageOption = spriteLanguges.Length - 1;
        }
        iconLanguage.sprite = spriteLanguges[languageOption]; 
        _settingData.LanguageOption = languageOption;

        string languageName = spriteLanguges[languageOption].name;
        leanLocalization.SetCurrentLanguage(languageName);
        _settingData.LanguegeName = languageName;
    }
    private void OnClickLanguageRightButton()
    {
        languageOption += 1;
        if(languageOption > spriteLanguges.Length - 1)
        {
            languageOption = 0;
        }
        iconLanguage.sprite = spriteLanguges[languageOption];
        _settingData.LanguageOption = languageOption;

        string languageName = spriteLanguges[languageOption].name;
        leanLocalization.SetCurrentLanguage(languageName);
        _settingData.LanguegeName = languageName;
    }
    private void OnClickOtherGameButton()
    {
        // todo
        GameManager.Instance.SaveData();

        Application.OpenURL("https://keiko-games.itch.io");
    }
    private void OnClickTermsOfServiceButton()
    {
        // todo
        Debug.Log("OnClickTermsOfServiceButton");
    }
    private void OnClickPrivacyPolicyButton()
    {        
        // todo
        Debug.Log("OnClickPrivacyPolicyButton");
    }
    private void OnClickSupportButton()
    {
        // todo
        Debug.Log("OnClickSupportButton");
    }
    private void OnClickQuitButton()
    {
        Application.Quit();
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


}
