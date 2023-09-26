using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Delegates;

public class CutSceneSelectAbility : MonoBehaviour
{
    public Audio _audio;
    public event AbilitySelectEventHandler E_OnSelectAbility;
  
    [Space]
    [SerializeField]
    private CutSceneIconAnimation cutSceneIconAnimation;
    [SerializeField] private CutScenePlayer player;
    [SerializeField] private GameObject panelSelectAbility;
    [Space]
    [SerializeField]
    private AbilityBase_SO ability1;
    [SerializeField] private AbilityBase_SO ability2;
    [Space]
    [SerializeField]
    private TextMeshProUGUI textNameAbi1;
    [SerializeField] private TextMeshProUGUI textNameAbi2;
    [Space]
    [SerializeField]
    private Image iconAbi1;
    [SerializeField] private Image iconAbi2;
    [Space]
    [SerializeField]
    private Button bttAbi1;
    [SerializeField] private Button bttAbi2;


    private bool isOpenPanel = false;


    private void Start()
    {
        Initialized();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))  SelectAbility();
    }

    private void OnDestroy()
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

    private void Initialized()
    {
        panelSelectAbility.SetActive(false);

        textNameAbi1.text = ability1.AbiName;
        textNameAbi2.text = ability2.AbiName;
        iconAbi1.sprite = ability1.Icon;
        iconAbi2.sprite = ability2.Icon;

        E_OnSelectAbility += player.OnSelectAbility;
    }

    private void SelectAbility()
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
