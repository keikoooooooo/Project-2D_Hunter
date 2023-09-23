using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CharacterData/EnemyData", fileName = "EnemyData")]
public class EnemyData_SO : ScriptableObject, ICharacterData<PlayerController>
{
    public List<EnemyController> ControllerList;

    private Dictionary<string , EnemyController> m_ControllerDic;

    public void Initialized()
    {
        ControllerList = new List<EnemyController>();

        foreach (var enemy in ControllerList)
        {
            m_ControllerDic.Add(enemy.stats_SO.Information.EnemyName, enemy);
        }
    }
    public PlayerController Controller(string controllerName)
    {
        throw new System.NotImplementedException();
    }
    public List<PlayerController> Controllers()
    {
        throw new System.NotImplementedException();
    }



}
