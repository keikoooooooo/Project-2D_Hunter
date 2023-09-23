using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarFrame : MonoBehaviour, IPool<AvatarFrame>
{

    // thông tin của character
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI textCharacterName;
    [SerializeField] TextMeshProUGUI textCharacterLevel;

    [Header("Box Progress")]// tiến độ thu thập mảnh và text hiển thị số mảnh của character để upgrade
    [SerializeField] Image fillProgress;
    [SerializeField] Gradient gradient;
    [SerializeField] TextMeshProUGUI textProgress;

    [Header("Image Color")]
    [SerializeField] Image frame;
    [SerializeField] Image glow;

    Action<AvatarFrame> action;
    PlayerController playerController;


    //Value Sort
    public int level;
    public int damage;
    public int rarity;



    public void Init(Action<AvatarFrame> action) => this.action = action;


    public void SelectPlayer() => CharactersManager.Instance.UpdateDataToUI(playerController);// cập nhật data của character vừa click vào lên UI

    public void SetStats(PlayerController player) // khi spawn prefab này ra -> sẽ cần truyền player vào để set data ra UI
    {
        playerController = player;

        PlayerStats_SO stats_SO = playerController.stats_SO;

        level = stats_SO.Information.Level;
        damage = stats_SO.Damage;
        rarity = (int)stats_SO.Information.Rarity;

        textCharacterName.text = stats_SO.Information.CharacterName;
        avatar.sprite = stats_SO.Information.Skins[0].Sprite;
        textCharacterLevel.text = $"Lv. {level}";

        int currentPoint = stats_SO.Information.CurrentUpgradePoint;
        int maxPoint = stats_SO.Information.MaxUpgradePoint;

        fillProgress.fillAmount = (float)currentPoint / maxPoint;
        fillProgress.color = gradient.Evaluate(fillProgress.fillAmount);
        textProgress.text = $"{currentPoint}/{maxPoint}";

        SetColorByRarity(stats_SO.Information.Rarity);

        gameObject.name = textCharacterName.text;
    }
    private void SetColorByRarity(CharacterRarity rarity) // set color nền theo độ hiếm của character
    {
        switch (rarity)
        {
            case CharacterRarity.Common:
                frame.color = new Color(.3f, .9f, .2f);
                glow.color = Color.white;
                break;
            case CharacterRarity.Rare:
                frame.color = new Color(.3f, .7f, .9f);
                glow.color = new Color(.8f, .9f, .9f);
                break;
            case CharacterRarity.Epic:
                frame.color = new Color(.6f, .2f, .9f);
                glow.color = new Color(1, .6f, .9f);
                break;
            case CharacterRarity.Legendary:
                frame.color = new Color(.9f, .6f, .2f);
                glow.color = new Color(1, .07f, .8f);
                break;
        }
    }


    public void UpdateStats()
    {
        level = playerController.stats_SO.Information.Level;
        damage = playerController.stats_SO.Damage;

        int currentPoint = playerController.stats_SO.Information.CurrentUpgradePoint;
        int maxPoint = playerController.stats_SO.Information.MaxUpgradePoint;

        fillProgress.fillAmount = (float)currentPoint / maxPoint;
        fillProgress.color = gradient.Evaluate(fillProgress.fillAmount);
        textProgress.text = $"{currentPoint}/{maxPoint}";
    }


}
