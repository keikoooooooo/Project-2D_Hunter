using System.Collections.Generic;
using UnityEngine;

public class MOD_Menu : MonoBehaviour
{

    [SerializeField] GameObject panelMod;
    [Space]
    [SerializeField] ButtonMOD modPower;
    [SerializeField] ButtonMOD modCoin;
    [SerializeField] ButtonMOD modGem;
    [SerializeField] ButtonMOD modToken;
    [SerializeField] ButtonMOD modTrophy;
    [SerializeField] ButtonMOD modUpgradePoint;
    [SerializeField] ButtonMOD modDataKillEnemy;
    [Space]
    [SerializeField] GameObject panelLoadMod;

    List<ButtonMOD> modList;

    private bool isMod = false;
    private bool isOpenMod = false;

    UserData userData;
    CharactersData characterData;
    TrophyRoadData trophyRoadData;
    BestiaryData bestiaryData;


    void Start()
    {
        modList = new List<ButtonMOD>()
        {
            modPower ,
            modCoin ,
            modGem ,
            modToken,
            modTrophy ,
            modUpgradePoint ,
            modDataKillEnemy 
        };

        userData = GameManager.Instance.UserData;
        characterData = GameManager.Instance.CharactersData;
        trophyRoadData = GameManager.Instance.TrophyRoadData;
        bestiaryData = GameManager.Instance.BestiaryData;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isOpenMod)  { ClosePanelMod(); }
            else            { OpenPanelMod(); }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            OnClickApplyModButton();
        }
    }


    private void OpenPanelMod()
    {
        isOpenMod = true;
        panelMod.SetActive(true);
    }
    private void ClosePanelMod()
    {
        isOpenMod = false;
        panelMod.SetActive(false);
    }

    // OnClick Button
    public void OnClickApplyModButton()
    {
        foreach (var mod in modList)
        {
            if(mod.value != 0)
            {
                isMod = true;
                break;
            }
        }

        if (isMod)
        {
            isOpenMod = false;
            isMod = false;
            panelLoadMod.SetActive(true);
            panelMod.SetActive(false);
            Invoke(nameof(InActivePanel), 1);
            ChangedValue();
        }
        else
        {
            panelMod.SetActive(false);
        }
    }
    private void ChangedValue()
    {
        // User Data
        userData.IncreasePower(modPower.value);
        userData.Coin += modCoin.value;
        userData.Gem += modGem.value;
        userData.Token += modToken.value;
        // Character Data
        List<PlayerController> listCharacter = characterData.PlayerControllers;
        foreach (var character in listCharacter)
        {
            if (character.stats_SO.Information.isUnlock)
            {
                character.stats_SO.IncreaseUpgradePoint(modUpgradePoint.value);
            }
        }
        // TrophyRoad Data
        trophyRoadData.IncreaseTrophy(modTrophy.value);
        userData.CurrentTrophyCount = trophyRoadData.CurrentTrophyCount;
        // Bestiary Data
        foreach (var data in bestiaryData.DataKills)
        {
            data.IncreaseKill(modDataKillEnemy.value);
        }

        Debug.Log("MOD SUCCESS");
        GameManager.Instance.UpdateMultiData();
        TrophyRoadManager.Instance.UpdateTrophyData();
    }
    private void InActivePanel()
    {
        panelLoadMod.SetActive(false);
        foreach (var mod in modList)
        {
            mod.ResetValue();
        }
    }


}
