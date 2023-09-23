using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Delegates;

public class CutSceneSelectAbility : MonoBehaviour
{
    public Audio _audio;
    public event AbilitySelectEventHandler E_OnSelectAbility;
  
    [Space]
    [SerializeField] CutSceneIconAnimation cutSceneIconAnimation;
    [SerializeField] CutScenePlayer player;
    [SerializeField] GameObject panelSelectAbility;
    [Space]
    [SerializeField] AbilityBase_SO ability1;
    [SerializeField] AbilityBase_SO ability2;
    [Space]
    [SerializeField] TextMeshProUGUI textNameAbi1;
    [SerializeField] TextMeshProUGUI textNameAbi2;
    [Space]
    [SerializeField] Image iconAbi1;
    [SerializeField] Image iconAbi2;
    [Space]
    [SerializeField] Button bttAbi1;
    [SerializeField] Button bttAbi2;


    bool isOpenPanel = false;


    void Start()
    {
        Initialized();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))  SelectAbility();
    }
    void OnDestroy()
    {
        E_OnSelectAbility -= player.OnSelectAbility;
    }


    public void OpenPanel()
    {
        isOpenPanel = true;
        panelSelectAbility.SetActive(true);
    }
    private void ClosePanel()
    {
        isOpenPanel = false;
        panelSelectAbility.SetActive(false);
    }

    void Initialized()
    {
        panelSelectAbility.SetActive(false);

        textNameAbi1.text = ability1.AbiName;
        textNameAbi2.text = ability2.AbiName;
        iconAbi1.sprite = ability1.Icon;
        iconAbi2.sprite = ability2.Icon;

        E_OnSelectAbility += player.OnSelectAbility;
    }
    void SelectAbility()
    {
        if (!isOpenPanel) return;
        if (ability1 == null) return;
        _audio.Play();
        E_OnSelectAbility?.Invoke(ability1.abilityType);
        ability1 = null;
        panelSelectAbility.SetActive(false);
        cutSceneIconAnimation.panelAll.SetActive(false);
        cutSceneIconAnimation.PlayAnimation(CutSceneAnimationName.IconAW);
        CutSceneManager.Instance.HandlerEnemy1Die();
        ClosePanel();
    }

}
