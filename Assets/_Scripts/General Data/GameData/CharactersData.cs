using System;
using System.Collections.Generic;


[Serializable]
public class AbilitiesEntry
{
    public string AbiName;
    public bool IsUnlock;
    public AbilitiesEntry() { }
    public AbilitiesEntry(string name, bool isUnlock)
    {
        AbiName = name;
        this.IsUnlock = isUnlock;
    }
}

[Serializable]
public class CharactersData
{
    [Serializable]
    public class Stats
    {
        public int Health;
        public int Damage;
        
        public string LastAbilitiesUsed1;
        public string LastAbilitiesUsed2;

        public PlayerInformation PlayerInformation;
        public List<AbilitiesEntry> AbilitiesPoint = new List<AbilitiesEntry>();

        // Constructor ?
        public Stats() { }
        public Stats(PlayerController p)
        {
            Health = p.stats_SO.MaxHealth;
            Damage = p.stats_SO.Damage;
            PlayerInformation = p.stats_SO.Information;
            p.stats_SO.ProactiveAbility.ForEach(x => AbilitiesPoint.Add(new AbilitiesEntry(x.AbiName, false)));
        }
    }

    public List<Stats> CharacterStats;
    public string LastUsedCharacterName;   // ??
    public int LastUsedCharacterSkin;



    public PlayerController PlayerController { get; private set; }
    public List<PlayerController> PlayerControllers { get; private set; }
    public List<PlayerController> PlayerUnlocks { get; private set; }

    #region Khởi tạo Constructor
    public CharactersData() { }
    public CharactersData(PlayerData_SO playerData_SO)
    {
        CharacterStats = new List<Stats> (); 

        PlayerControllers = playerData_SO.GetControllers();
        foreach (var controller in PlayerControllers)
        {
            controller.stats_SO.Information.ResetUpgradePoint();
            CharacterStats.Add(new Stats(controller));
        }

        CharacterStats[0].PlayerInformation.isUnlock = true;
        PlayerControllers[0].stats_SO.Information.isUnlock = true;

        PlayerController = PlayerControllers[0];
        LastUsedCharacterName = PlayerController.stats_SO.Information.CharacterName;
        LastUsedCharacterSkin = 1;
    }
    #endregion


    #region Load Data
    public void LoadData(PlayerData_SO playerData_SO)
    {
        PlayerUnlocks = new List<PlayerController>();
        PlayerControllers = playerData_SO.GetControllers();
        foreach (var controller in PlayerControllers)
        {
            Stats stats = FindStats(controller.stats_SO.Information.CharacterName);
            controller.stats_SO.MaxHealth   = stats.Health;
            controller.stats_SO.Damage      = stats.Damage;
            controller.stats_SO.Information = stats.PlayerInformation;
            controller.stats_SO.Information.AbilitiesPoint = stats.AbilitiesPoint;
            controller.stats_SO.Information.LastAbilitiesUsed1 = stats.LastAbilitiesUsed1;
            controller.stats_SO.Information.LastAbilitiesUsed2 = stats.LastAbilitiesUsed2;

            controller.stats_SO.GetAbilities();
            if (controller.stats_SO.Information.isUnlock) PlayerUnlocks.Add(controller);
        }
        PlayerController = FindPlayer(LastUsedCharacterName);
    }
    private Stats FindStats(string namePlayer) => CharacterStats.Find(x => x.PlayerInformation.CharacterName == namePlayer);
    private PlayerController FindPlayer(string namePlayer) => PlayerControllers.Find(x => x.stats_SO.Information.CharacterName == namePlayer);
    #endregion


    #region Save Data
    public void SaveData()
    {
        foreach (var stats in CharacterStats)
        {
            PlayerStats_SO playerStats_SO = FindPlayer(stats.PlayerInformation.CharacterName).stats_SO;

            stats.Health = playerStats_SO.MaxHealth;
            stats.Damage = playerStats_SO.Damage;
            stats.LastAbilitiesUsed1 = playerStats_SO.Information.LastAbilitiesUsed1;
            stats.LastAbilitiesUsed2 = playerStats_SO.Information.LastAbilitiesUsed2;
            stats.PlayerInformation = playerStats_SO.Information;
            stats.AbilitiesPoint = playerStats_SO.Information.AbilitiesPoint;
        }
    }
    public void SetCurrentPlayer(PlayerController player)
    {
        PlayerController = player;
        LastUsedCharacterName = player.stats_SO.Information.CharacterName;
        LastUsedCharacterSkin = player.stats_SO.Information.CurrentSkin;
    }
    public void UnLockCharacter(string characterName)
    {
        FindStats(characterName).PlayerInformation.isUnlock = true;
        PlayerUnlocks.Add(FindPlayer(characterName));
    }
    #endregion

    public int CharacterUnlock()
    {
        int count = 0;
        foreach (var player in PlayerControllers)
        {
            if (player.stats_SO.Information.isUnlock)
            {
                count++;
            }
        }
        return count;
    }

}


