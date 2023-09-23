using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvaterFrame_None : MonoBehaviour, IPool<AvaterFrame_None>
{

    [SerializeField] Image Avatar;
    [SerializeField] TextMeshProUGUI textCharacterName;

    Action<AvaterFrame_None> action;
    [HideInInspector] public PlayerController playerController;

    //Value Sort
    public int level;
    public int damage;
    public int rarity;

    public void Init(Action<AvaterFrame_None> action) => this.action = action;
    public void Action() => action(this);

    public void SetStats(PlayerController p)
    {
        playerController = p;
        
        level = playerController.stats_SO.Information.Level;
        damage = playerController.stats_SO.Damage;
        rarity = (int)playerController.stats_SO.Information.Rarity;

        Avatar.sprite = p.stats_SO.Information.Skins[0].Sprite;
        textCharacterName.text = p.stats_SO.Information.CharacterName;

        gameObject.name = textCharacterName.text;
    }
    public void SelectPlayer() => CharactersManager.Instance.UpdateDataToUI(playerController);

}
