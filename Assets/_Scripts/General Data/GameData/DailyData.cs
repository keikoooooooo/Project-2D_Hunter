using System;
using System.Collections.Generic;

[Serializable]
public class DailyData
{
    public string ResetTime; // thời gian reset item -> 0h hàng ngày

    public int DayCurrent;
    public List<bool> isRewardDay;

    public DailyData()
    {
        ResetTime = DateTime.MinValue.ToString();
        DayCurrent = 0;
        isRewardDay = new List<bool>
        {
            false, // 0
            false, // 1
            false, // 2
            false, // 3
            false, // 4
            false, // 5
            false  // 6
        };
    }
    public void ResetDaily()
    {
        ResetTime = DateTime.MinValue.ToString();
        DayCurrent = 1;
        isRewardDay = new List<bool>
        {
            false, // 0
            false, // 1
            false, // 2
            false, // 3
            false, // 4
            false, // 5
            false  // 6
        };
    }

    
}
