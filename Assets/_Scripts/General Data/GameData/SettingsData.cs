using System;

[Serializable]
public class SettingsData
{
    public int valueCameraSize; // 1 = Near(gần), 2 = Medium(vừa), 3 = Far(xa) => 7, 9, 11

    public int LanguageOption;
    public string LanguegeName;

    public bool isOnclickSound, isOnclickMusic;

    public float valueSliderSound, valueSliderMusic;

    public bool isOnClickShowRange;

    public SettingsData()
    {
        valueCameraSize = 2;

        LanguageOption = 0;
        LanguegeName = "English";

        isOnclickSound = true;
        isOnclickMusic = true;

        valueSliderMusic = .5f;
        valueSliderSound = .5f;

        isOnClickShowRange = true;
    }


}
