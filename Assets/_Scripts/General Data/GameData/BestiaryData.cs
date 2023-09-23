using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BestiaryData
{
    public List<DataKillEnemy> DataKills;
    public List<EnemyController> EnemyControllers { get; private set; }

    #region Khởi tạo Constructor & Data
    public BestiaryData() { }
    public BestiaryData(EnemyData_SO enemyData_SO)
    {
        DataKills = new List<DataKillEnemy>();
        EnemyControllers = enemyData_SO.GetControllers();
        foreach (var enemy in EnemyControllers)
        {
            EnemyInformation infor = enemy.stats_SO.Information;
            DataKills.Add(new DataKillEnemy(infor));
        }
    }
    #endregion


    #region LoadData
    public void LoadData(EnemyData_SO enemyData_SO)
    {
        EnemyControllers = enemyData_SO.GetControllers();     
    }
    #endregion


    #region SetData
    public void IncreaseKill(string enemyName)
    {
        DataKills.Find(d => d.EnemyName == enemyName).IncreaseKill(1);
    }
    public void ClaimReward(string enemyName)
    {
        DataKills.Find(d => d.EnemyName == enemyName).ClaimReward();
    }

    #endregion
}


[Serializable]
public class DataKillEnemy
{
    public string EnemyName;
    public Sprite Sprite;

    // Khi lên 1 level -> số lượng tiêu diệt sẽ * thêm LevelkKill
    public int KillLevel;

    // Số lượng tiêu diệt enemy  
    public int KillRequest;     // yêu cầu
    public int KillCurrent;     // hiện tai

    // Khi đạt yêu cầu ở trên sẽ tăng 1 sao.
    public int StarMaxLevel;
    public int StarCurrentLevel;

    public int IndexColorStarCurrentLevel;

    // phần thưởng
    public int Reward;
    public List<int> RewardsList;

    public DataKillEnemy() { }
    public DataKillEnemy(EnemyInformation enemyInformation)
    {
        EnemyName = enemyInformation.EnemyName;
        Sprite = enemyInformation.Sprite;
        KillLevel = 1;
        KillRequest = 10;
        KillCurrent = 0;
        StarMaxLevel = 6;
        StarCurrentLevel = 0;
        IndexColorStarCurrentLevel = 1;
        Reward = 2;

        RewardsList = new List<int>();
    }

    public void IncreaseKill(int value)
    {
        while (value > 0)
        { 
            int valueIncrease = KillRequest - KillCurrent;
            if (value < valueIncrease) // nếu value truyền vào nhỏ hơn mức tăng cấp tiếp theo
            {
                KillCurrent += value;
                value = 0;
            }
            else // ngược lại 
            {
                int surplus = value - valueIncrease; // tìm số dư sau khi cộng
                KillCurrent += valueIncrease;
                value = surplus;
            }
            if (KillCurrent >= KillRequest)
            {
                KillLevel += 1;
                KillCurrent = 0;
                KillRequest += 10;

                StarCurrentLevel += 1;
                if (StarCurrentLevel >= StarMaxLevel)
                {
                    StarCurrentLevel = 0;
                    IndexColorStarCurrentLevel += 1;
                }

                RewardsList.Add(Reward);
                Reward += 3;
            }
        }
    }

    public void ClaimReward()
    {
        RewardsList.RemoveAt(0);
    }

}