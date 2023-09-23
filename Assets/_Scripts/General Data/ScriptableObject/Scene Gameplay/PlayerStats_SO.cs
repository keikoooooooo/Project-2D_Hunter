using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Stats/Player", fileName = "Stats_SO")]
public class PlayerStats_SO : CharacterStats_SO
{
    [Range(1, 5)] public float RangeAttack;
    public int Armor;   // giáp
    [Range(1, 5)] public float SpecialMultiple; // hệ số nhân của damage special 

    [Space]
    [Tooltip("Khả năng chủ động")]
    public List<AbilityBase_SO> ProactiveAbility;
    public AbilityBase_SO PassiveAbility1 { get; private set; }
    public AbilityBase_SO PassiveAbility2 { get; private set; }

    [Space]
    public PlayerInformation Information;

    public void UpgradeStats()
    {
        MaxHealth += 10;
        Damage += 2;
        Information.Level += 1;
        Information.CurrentUpgradePoint -= Information.MaxUpgradePoint;
        Information.MaxUpgradePoint += 15;
        Information.UpgradePrice += 50;
    }
    public void IncreaseUpgradePoint(int point) => Information.CurrentUpgradePoint += point;

    public void GetAbilities()
    {
        PassiveAbility1 = !string.IsNullOrEmpty(Information.LastAbilitiesUsed1) ?
                    FindAbilities(Information.LastAbilitiesUsed1) : null;
        PassiveAbility2 = !string.IsNullOrEmpty(Information.LastAbilitiesUsed2) ?
                    FindAbilities(Information.LastAbilitiesUsed2) : null;
    }
    public void SetAbilities(int index, AbilityBase_SO abilityBase)
    {
        if(index == 1)
        {
            Information.LastAbilitiesUsed1 = abilityBase.AbiName;
            PassiveAbility1 = abilityBase;
        }
        else if(index == 2)
        {
            Information.LastAbilitiesUsed2 = abilityBase.AbiName;
            PassiveAbility2 = abilityBase;
        }
    }
    public void SetAbilities(int index)
    {
        if (index == 1)
        {
            Information.LastAbilitiesUsed1 = null;
            PassiveAbility1 = null; Debug.Log("Set Null1");
        }
        else if (index == 2)
        {
            Information.LastAbilitiesUsed2 = null;
            PassiveAbility2 = null; Debug.Log("Set Null2");
        }
       
    }
    public void UnlockAbilitiesPoint(string abiName) => Information.AbilitiesPoint.Find(x => x.AbiName == abiName).IsUnlock = true; 

    public AbilityBase_SO FindAbilities(string abiName) => ProactiveAbility.Find(x => x.AbiName == abiName);
}


[Serializable]
public class PlayerInformation
{
    [Serializable]
    public class SkinsEntry
    {
        public Sprite Sprite;
        public int Price;
    }

    public bool isUnlock = false;
    [Space]
    public string CharacterName;
    public string CharacterInformation;
    public string LastAbilitiesUsed1;
    public string LastAbilitiesUsed2;
    public List<AbilitiesEntry> AbilitiesPoint = new List<AbilitiesEntry>();
    [Space]
    public int TotalSkin; 
    public List<SkinsEntry> Skins;
    public int CurrentSkin;
    [Space]
    public int Level;   
    public int CurrentUpgradePoint; // số điểm nâng cấp hiện tại
    public int MaxUpgradePoint;     // số điểm tối đa cần để nâng cấp
    public int UpgradePrice;
    [Space]
    public CharacterRarity Rarity;

    public PlayerInformation(){ }

    public void ResetUpgradePoint()
    {
        isUnlock = false;
        Level = 1;
        CurrentUpgradePoint = 0;
        MaxUpgradePoint = 5;
        UpgradePrice = 50;
        LastAbilitiesUsed1 = null;
        LastAbilitiesUsed2 = null;
    }

}

public enum CharacterRarity // độ hiếm
{
    Common,     // phổ biến
    Rare,       // quý
    Epic,       // sử thi
    Legendary   // huyền thoại
}

