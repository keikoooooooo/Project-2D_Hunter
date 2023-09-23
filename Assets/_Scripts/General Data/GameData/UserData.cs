using System;
using System.Collections.Generic;


[Serializable]
public class UserData
{
    public bool isNewAccount;               // check có phải tk mới ?
    public string LastQuitTime;             // thời gian cuối cùng của lần chơi trước
    public long OfflineMinutes;             // thời gian offline (phút)
    public string Username;                 // tên
    public string LastUsedColor;            // màu ở lần sử dụng trước (Hex: FFFFFF, F28222, .....)
    public string LastNameColor;            // màu ở lần sử dụng trước (tên màu: white, red, ... )
    public List<string> PurchasedColors;    // danh sách màu để tổi màu tên
    public int LastAvatarIndex;             // avatar lần cuối dùng

    public int CurrentTrophyCount;          // số cúp đạt được
    public int MapIndex;                    // map đang chơi
    public int CharacterCount;              // tổng số character có
    public int TotalPlay;                   // tổng số trận chơi
    public long PlayingTime;                // tổng thời gian chơi

    public int Power;
    public int Coin;
    public int Gem;
    public int Token;

    public UserData()
    {
        isNewAccount = true;
        LastQuitTime = "";
        Username = "";
        LastUsedColor = "FFFFFF";
        LastNameColor = "White";
        PurchasedColors = new List<string> { LastUsedColor, "", "", "", "", "", ""};
        LastAvatarIndex = 0;
        CurrentTrophyCount = 0;
        MapIndex = 1;
        TotalPlay = 0;
        PlayingTime = 0;
        Power = 200;
        Coin = 10000;
        Gem = 500;
        Token = 0;
    } 

    public void FindRewardOffline() // tìm số power khi ngoại tuyến
    {
        if (!string.IsNullOrEmpty(LastQuitTime))
        {
            DateTime lastQuitTime = DateTime.Parse(LastQuitTime);
            DateTime currentTime = DateTime.Now;

            TimeSpan offlineDaration = currentTime - lastQuitTime;
            OfflineMinutes = (long)offlineDaration.TotalMinutes;
        }

        int rewardOffline = (int)OfflineMinutes / 5;

        Power += rewardOffline;
        if(Power >= 200) Power = 200;    
    }
    public void CheckNewAccount()
    {
        if (string.IsNullOrEmpty(LastQuitTime)) // nếu tài khoản chưa có username -> tài khoản mới
        {
            LastQuitTime = DateTime.Now.ToString();
            isNewAccount = true;
            Username = "GUEST" + UnityEngine.Random.Range(10000, 99999);
        }
        else
        {
            isNewAccount = false;
        }
    }
    public void CheckUserName()
    {
        if (string.IsNullOrEmpty(Username)) // nếu tài khoản chưa có username -> tài khoản mới
        {
            Username = "GUEST" + UnityEngine.Random.Range(10000, 99999); 
        }
    }
    public bool IsIncreasePower() => Power < 200;
    public void IncreasePower(int val)
    {
        int temp = Power + val;
        Power = temp >= 200 ? 200 : temp;
    }

}


 