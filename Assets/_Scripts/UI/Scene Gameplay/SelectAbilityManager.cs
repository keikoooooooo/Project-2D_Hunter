using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Delegates;

public class SelectAbilityManager : MonoBehaviour
{

    public event AbilitySelectEventHandler E_OnSelectAbility;

    [SerializeField] private GameObject panelSelectAbility;
    [Space]
    [SerializeField]
    private TextMeshProUGUI textNameAbi1;
    [SerializeField] private TextMeshProUGUI textNameAbi2;
    [Space]
    [SerializeField]
    private Image iconAbi1;
    [SerializeField] Image iconAbi2;


    [Space(10)]
    private List<AbilityBase_SO> _proactiveAbility; // nhận về danh sách các khả năng chủ động của player
    private PlayerController player;

    private AbilityBase_SO ability1;
    private AbilityBase_SO ability2;

    private void Start()
    {
        _proactiveAbility = new List<AbilityBase_SO>();
        panelSelectAbility.SetActive(false);

        if(GamePlayManager.Instance)
        {
            player = GamePlayManager.Instance.player;
            E_OnSelectAbility += player.OnSelectAbility;
            _proactiveAbility = player.stats_SO.ProactiveAbility;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))   SelectAbility(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))   SelectAbility(2);
    }

    private void OnDestroy()
    {
        if(E_OnSelectAbility != null) E_OnSelectAbility -= player.OnSelectAbility;
    }

    #region Spawn Ability
    public void SpawnAbility() // được gọi tự động = event LevelUp của XPManager
    {
        panelSelectAbility.SetActive(true);

        RandomAbility();
    }

    private void RandomAbility()
    {
        var element1 = Random.Range(0, _proactiveAbility.Count);
        var element2 = Random.Range(0, _proactiveAbility.Count);

        while (element1 == element2)
        {
            element2 = Random.Range(0, _proactiveAbility.Count);
        }

        ability1 = _proactiveAbility[element1];  
        ability2 = _proactiveAbility[element2];   

        SetUIAbility(ability1, ability2);
    }

    private void SetUIAbility(AbilityBase_SO abi1, AbilityBase_SO abi2)
    {
        textNameAbi1.text = abi1.AbiName;
        textNameAbi2.text = abi2.AbiName;

        iconAbi1.sprite = abi1.Icon;
        iconAbi2.sprite = abi2.Icon;
    }
    #endregion


    private void SelectAbility(int index)
    {
        AudioManager.Instance.Play(AudioName.OnClick);
        switch (index)
        {
            case 1:
                if (ability1 == null) return;
                E_OnSelectAbility?.Invoke(ability1.abilityType);
                ability1 = null;
                break;
            case 2:
                if (ability2 == null) return;
                E_OnSelectAbility?.Invoke(ability2.abilityType); 
                ability2 = null;
                break;
        }

        XPManager.Instance.SubtractCountLevelUp();

        if (XPManager.Instance.countLevelUp != 0)
        {
            SpawnAbility();
        }
        else
        {
            panelSelectAbility.SetActive(false);
        }
        
    }

}
