using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InforAbility : SingletonManager<InforAbility>
{
    SelectAbilities selectAbilities;
    [SerializeField] GameObject panelInfor;

    [Space]
    [SerializeField] TextMeshProUGUI abiName;
    [SerializeField] Image abiIcon;
    [SerializeField] TextMeshProUGUI abiDescription;

    void Start()
    {
        selectAbilities = GetComponent<SelectAbilities>();
        panelInfor.SetActive(false);
    }

    public void SetStats(AbilityBase_SO ability)
    {
        panelInfor.SetActive(true);
        abiName.text = ability.AbiName;
        abiIcon.sprite = ability.Icon;
        abiDescription.text = ability.Description;
    }

}
