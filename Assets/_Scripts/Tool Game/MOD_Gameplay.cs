using System.Collections.Generic;
using UnityEngine;

public class MOD_Gameplay : MonoBehaviour
{
    [SerializeField] StartGame startGame;
    [Space]
    [SerializeField] GameObject panelMod;
    [SerializeField] GameObject panelLoadMod;
    [Space]
    [SerializeField] SliderMOD modHealth;
    [SerializeField] SliderMOD modSpeed;
    [SerializeField] SliderMOD modDamage;
    [SerializeField] SliderMOD modAttackSpeed;
    [Space]
    [SerializeField] SliderMOD modTime;
    [SerializeField] SliderMOD modXP;

    private bool isMod = false;
    private bool isOpenMod = false;

    List<SliderMOD> mods;
    PlayerController player;

    void Start()
    {
        mods = new List<SliderMOD>
        {
            modHealth, modSpeed, modDamage, modAttackSpeed, modTime, modXP
        };
        player = GamePlayManager.Instance.player;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (isOpenMod) { ClosePanelMod(); }
            else { OpenPanelMod(); }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickApplyModButton();
        }
        modTime.slider.maxValue = startGame.timer;
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
    void GetPlayer(PlayerController p) => player = p;
    

    public void OnClickApplyModButton()
    {
        foreach (var mod in mods)
        {
            if (mod.value != 0)
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
            Invoke(nameof(InActivePanel), .4f);
            ChangedValue(); // thay đổi giá trị
        }
        else
        {
            panelMod.SetActive(false);
        }
    }

    void ChangedValue()
    {
        player.Status.SetStats((int)modHealth.value, (int)modDamage.value, modSpeed.value, modAttackSpeed.value);

        if(modTime.value != 0)
        {
            int timer = (int)modTime.value;
            startGame.GetComponent<CountdownTime>().StartCountDown(timer);
        }
        XPManager.Instance.bonusXp += modXP.value;

        mods.ForEach(x => x.UpdateValue());
    }
    void InActivePanel()
    {
        panelLoadMod.SetActive(false);
    }

}
