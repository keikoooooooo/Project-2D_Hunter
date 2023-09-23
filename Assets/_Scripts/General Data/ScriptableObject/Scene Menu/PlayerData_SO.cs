using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData/PlayerData", fileName = "PlayerData")]
public class PlayerData_SO : ScriptableObject , ICharacterData<PlayerController>
{
    public List<PlayerController> ControllerList;

    private Dictionary<string, PlayerController> m_PlayerControllerDic;

    public void Initialized()
    {
        m_PlayerControllerDic = new Dictionary<string, PlayerController>();
        foreach (var player in ControllerList)
        {
            m_PlayerControllerDic.Add(player.stats_SO.Information.CharacterName, player);
        }
    }

    public PlayerController GetController(string controllerName) => m_PlayerControllerDic[controllerName];
    public List<PlayerController> GetControllers() => ControllerList;

}   
