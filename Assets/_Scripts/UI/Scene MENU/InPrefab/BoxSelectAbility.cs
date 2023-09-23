using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxSelectAbility : MonoBehaviour, IPool<BoxSelectAbility>
{
    public event Action<AbilityBase_SO> E_OnSelectAbi;
    public bool isEventOnSelect;

    public GameObject panelUseAbi;
    public GameObject panelBlockAbi;
    [Space]
    public TextMeshProUGUI abiName;
    [SerializeField] Image abiIcon;
    [Space]
    public Button bttSelectAbi;
    [SerializeField] Button bttInforAbi;

    AbilityBase_SO ability;
    Action<BoxSelectAbility> action;


    void Start()
    {
        RegisterEvent();
    }
    void OnDestroy()
    {
        UnRegisterEvent();
    }
    public void Init(Action<BoxSelectAbility> action) => this.action = action;
    public void Action() => action(this);


    void RegisterEvent()
    {
        bttSelectAbi.onClick.AddListener(OnClickSelectAbiButton);
        bttInforAbi.onClick.AddListener(OnClickInformationAbiButton);
    }
    void UnRegisterEvent()
    {
        bttSelectAbi.onClick.RemoveListener(OnClickSelectAbiButton);
        bttInforAbi.onClick.RemoveListener(OnClickInformationAbiButton);
    }

    public void SetStats(AbilityBase_SO abilityBase_SO)
    {
        ability = abilityBase_SO;

        abiName.text = abilityBase_SO.AbiName;
        abiIcon.sprite = abilityBase_SO.Icon;
    }
    public void SetUseButton(string lastAbiName1, string lastAbiName2)
    {
        bool isCheck = abiName.text == lastAbiName1 || abiName.text == lastAbiName2;
        panelUseAbi.SetActive(isCheck);
        bttSelectAbi.interactable = !isCheck;
    }

    public void SetPanelBlock()
    {
        panelUseAbi.SetActive(false);
        panelBlockAbi.SetActive(true);
        bttSelectAbi.interactable = false;
        Debug.Log("Block");
    }


    // OnClickButton
    void OnClickSelectAbiButton()
    {  
        E_OnSelectAbi?.Invoke(ability);
    }
    void OnClickInformationAbiButton()
    {
        if (ability == null) return;
        InforAbility.Instance.SetStats(ability);
    }


}
