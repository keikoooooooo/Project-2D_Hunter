using System;
using UnityEngine;

[Serializable]
public class ShopData
{
    public string ResetTime; // thời gian reset item -> 0h hàng ngày

    public bool IsFreeCoinClaim;    // đã mua chưa?
    public bool IsBuyUpgradePoint;  // đã mua chưa?
    public int CountBuyPower;       // số lượng mua power / 1ngay

    public string PlayerCurrent;    // player đang bán?
    public bool isBuyPlayer;        // đã mua chưa?
    public Sprite AvatarPlayerCurrent { get; private set; }


    public ShopData() 
    {
        ResetTime = DateTime.MinValue.ToString();
        IsBuyUpgradePoint = true;
        IsFreeCoinClaim = true;
        CountBuyPower = 5;
        PlayerCurrent = "";
        isBuyPlayer = true;
    }


}
