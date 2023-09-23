using System.Collections.Generic;

public static class DataReference
{

    private static readonly List<IData> datas = new List<IData>();


    public static void Register_IData(IData data)
    {
        if(!datas.Contains(data)) datas.Add(data);
    }
    public static void Unregister_IData(IData data)
    {
        if(datas.Contains(data))  datas.Remove(data);
    }


    public static List<IData> GetAll_IData() => new List<IData>(datas);

    public static void ClearData()
    {
        datas.Clear();
        if(TrophyRoadManager.Instance) Register_IData(TrophyRoadManager.Instance);
    }
 

}
