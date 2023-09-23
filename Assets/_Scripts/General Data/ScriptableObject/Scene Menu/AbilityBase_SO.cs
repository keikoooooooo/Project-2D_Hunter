using UnityEngine;


[CreateAssetMenu(menuName = "Ability_SO/Ability Type", fileName = "Type_")]
public class AbilityBase_SO : ScriptableObject
{
    [Header("Ability Information")]
    public Sprite Icon;
    public string AbiName;
    public string Description;

    [Header("Upgrade Stats")][Space(15)]
    public AbilityType abilityType;
    public float[] value;
   
}

public enum AbilityType // khả năng buff khi gameplay
{
// Ability Base
    Health,         // + 50         Hp
    MovementSpeed,  // + 20%        tốc độ di chuyển 
    AttackSpeed,    // + 10%        tốc độ đánh 
    Damage,         // + 5          Dmg 
    MoreXP,         // + 25%        kinh nghiệm

// Ability Other
    HpAndDamage,    // + 30/+ 3     Hp / Dmg
    Regen,          // + 2%Hp/2s    2% Hp mỗi 2 giây
    CelestialPact,  // + 100/- 5    Hp / Dmg
    DemonPact,      // + 15/-50     Dmg / Hp
    Armor,          // + 10%        giáp
    MuiltShot,      // + 1          đòn tấn công
    Distance,       // + 1          phạm vi tấn công
    LuckyXP,        // + 100%       kinh nghiệm
    DeathStrike,    // + 10%        one shot
    LifeSteal,      // + 15%        cơ hội hút 20% máu tối đa của kẻ địch
    DodgeChance,    // + 15%        cơ hội né tránh đòn
    DizzleChance,   // + 5%         cơ hội làm choáng khi tấn công
}