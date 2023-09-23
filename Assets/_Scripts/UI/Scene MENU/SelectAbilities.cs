using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAbilities : MonoBehaviour
{
    [SerializeField] GameObject panelSelectAbi;

    [Space]
    [SerializeField] Image iconAbi1;
    [SerializeField] Image iconAbi2;
    [Space]
    [SerializeField] Button bttSelectAbi1;
    [SerializeField] Button bttSelectAbi2;
    [Space]
    [SerializeField] Button bttRemoveAbi1;
    [SerializeField] Button bttRemoveAbi2;
    [Space]
    [SerializeField] GameObject panelBlockAbi1;
    [SerializeField] GameObject panelBlockAbi2;
    [Space]
    [SerializeField] Transform contentSpawnBox;
    [SerializeField] BoxSelectAbility boxSelectPrefab;

    List<BoxSelectAbility> boxSelectAbilities = new List<BoxSelectAbility>();
    ObjectPool<BoxSelectAbility> poolBox;
    PlayerController player;
    int indexClickButton;


    void Awake()
    {
        poolBox = new ObjectPool<BoxSelectAbility>(boxSelectPrefab, contentSpawnBox, 0);
    }
    void Start()
    {
        iconAbi1.enabled = false;
        iconAbi2.enabled = false;
        bttSelectAbi1.interactable = false;
        bttSelectAbi2.interactable = false;
        bttRemoveAbi1.gameObject.SetActive(false);
        bttRemoveAbi2.gameObject.SetActive(false);
        panelBlockAbi1.SetActive(true);
        panelBlockAbi2.SetActive(true);
    }


    public void SetState(PlayerController p)
    {
        player = p;
        
        bttSelectAbi1.interactable = false;
        bttSelectAbi2.interactable = false;
        panelBlockAbi1.SetActive(true);
        panelBlockAbi2.SetActive(true);
        
        if (boxSelectAbilities.Count == 0)
        {
            foreach (var ability in player.stats_SO.ProactiveAbility)
            {
                var box = poolBox.Get();
                if (!box.isEventOnSelect)
                {
                    box.E_OnSelectAbi += UpdateAbilitiesInPlayer;
                    box.isEventOnSelect = true;
                }
                boxSelectAbilities.Add(box);
            }
        }
    
        int levelPlayer = player.stats_SO.Information.Level;
        if(levelPlayer >= 5)
        {
            bttSelectAbi1.interactable = true;
            panelBlockAbi1.SetActive(false);
            string abi1 = player.stats_SO.Information.LastAbilitiesUsed1;
            if (!string.IsNullOrEmpty(abi1))
            {
                iconAbi1.enabled = true;
                iconAbi1.sprite = player.stats_SO.FindAbilities(abi1).Icon;
                bttRemoveAbi1.gameObject.SetActive(true);
            }       
        }
        if(levelPlayer >= 10)
        {
            bttSelectAbi2.interactable = true;
            panelBlockAbi2.SetActive(false);
            string abi2 = player.stats_SO.Information.LastAbilitiesUsed2;
            if (!string.IsNullOrEmpty(abi2))
            {
                iconAbi2.enabled = true;
                iconAbi2.sprite = player.stats_SO.FindAbilities(abi2).Icon;
                bttRemoveAbi2.gameObject.SetActive(true);
            }
        }
    }
    private void SetActiveAbilities()
    {
        foreach (var abi in player.stats_SO.Information.AbilitiesPoint)
        {
            var box = FindBoxSelect(abi.AbiName);
            if (box != null && !abi.IsUnlock)
            {
                box.SetPanelBlock();
            }
        }
    }
    private BoxSelectAbility FindBoxSelect(string _abiName) => boxSelectAbilities.Find(x => x.abiName.text == _abiName);

    private void UpdateAbilitiesInPlayer(AbilityBase_SO ability)
    {
        player.stats_SO.SetAbilities(indexClickButton, ability);
        if(indexClickButton == 1)
        {
            iconAbi1.enabled = true;
            iconAbi1.sprite = ability.Icon;
            bttRemoveAbi1.gameObject.SetActive(true);
        }
        else if(indexClickButton == 2)
        {
            iconAbi2.enabled = true;
            iconAbi2.sprite = ability.Icon;
            bttRemoveAbi2.gameObject.SetActive(true);
        }

        foreach (var box in boxSelectAbilities)
        {
            box.SetUseButton(player.stats_SO.Information.LastAbilitiesUsed1, player.stats_SO.Information.LastAbilitiesUsed2);
        }
        SetActiveAbilities();
    }



    // OnClick Button
    public void OpenPanelSelectAbilitiesButton(int indexButton)
    {
        panelSelectAbi.SetActive(true);

        int count = 0;
        foreach (var box in boxSelectAbilities)
        {
            box.gameObject.SetActive(true);
            box.SetStats(player.stats_SO.ProactiveAbility[count]);
            box.SetUseButton(player.stats_SO.Information.LastAbilitiesUsed1, player.stats_SO.Information.LastAbilitiesUsed2);
            count++;
        }
        SetActiveAbilities();

        indexClickButton = indexButton;
    }
    public void OnClosePanelSelectAbiliesButton()
    {
        boxSelectAbilities.ForEach(x => x.Action());
        panelSelectAbi.SetActive(false);
    }
    public void DestroySelectAbilitiesButton()
    {
        poolBox.ListPool.Clear();
        boxSelectAbilities.Clear();
        for (int i = 0; i < contentSpawnBox.childCount; i++)
        {
            var box = contentSpawnBox.GetChild(i).GetComponent<BoxSelectAbility>();
            box.E_OnSelectAbi -= UpdateAbilitiesInPlayer;
            Destroy(box.gameObject);
        }
    }
    public void OnClickRemoveAbilitiesButton(int indexButton)
    {
        if (indexButton == 1)
        {
            iconAbi1.sprite = null;
            iconAbi1.enabled = false;
            bttRemoveAbi1.gameObject.SetActive(false);
        }
        else if(indexButton == 2)
        {
            iconAbi2.sprite = null;
            iconAbi2.enabled = false;
            bttRemoveAbi2.gameObject.SetActive(false);
        }
        player.stats_SO.SetAbilities(indexButton);
    }


}
