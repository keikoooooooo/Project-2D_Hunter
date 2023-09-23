using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData/EnemyData", fileName = "EnemyData")]
public class EnemyData_SO : ScriptableObject, ICharacterData<EnemyController>
{
    public List<EnemyController> ControllerList;

    private Dictionary<string, EnemyController> m_ControllerDic;

    public void Initialized()
    {
        m_ControllerDic =new Dictionary<string, EnemyController>();

        foreach(EnemyController controller in ControllerList)
        {
            m_ControllerDic.Add(controller.stats_SO.Information.EnemyName, controller);
        }
    }

    public EnemyController GetController(string controllerName) => m_ControllerDic[controllerName];
    public List<EnemyController> GetControllers() => ControllerList;
}
