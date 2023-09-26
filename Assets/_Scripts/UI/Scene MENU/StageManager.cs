using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour, IData
{
    [SerializeField] private GameObject panelStage;

    [SerializeField] private Button[] bttMap;
    [SerializeField] private GameObject[] tickUse;


    [Header("Desert Map")][Space]
    [SerializeField]
    private Slider sliderDesert;
    [SerializeField] private TextMeshProUGUI textProgressDesert;
    [SerializeField] private GameObject panelLockDesertMap;


    private int currentTrophy;
    private int mapIndex;


    private void Awake()
    {
        DataReference.Register_IData(this);
    }


    public void GETData(GameManager gameManager)
    {
        currentTrophy = gameManager.TrophyRoadData.CurrentTrophyCount;
        mapIndex = gameManager.UserData.MapIndex;

        UpdateData();
    }
    public void UpdateData()
    {   
        currentTrophy = GameManager.Instance.TrophyRoadData.CurrentTrophyCount;
        SetStateButton();

        if (currentTrophy < 200)
        {
            panelLockDesertMap.SetActive(true);
            tickUse[1].SetActive(false);
        }
        else
        {
            panelLockDesertMap.SetActive(false);
        }

        int val = currentTrophy >= 200 ? 200 : currentTrophy;
        sliderDesert.minValue = 0;
        sliderDesert.maxValue = 200;
        sliderDesert.value = val;
        textProgressDesert.text = $"{sliderDesert.value} / {sliderDesert.maxValue}";
    }

    private void SetStateButton()
    {
        for (int i = 1; i <= bttMap.Length; i++)
        {
            if(i == mapIndex)
            {
                bttMap[i - 1].interactable = false;
                tickUse[i - 1].SetActive(true);
            }
            else
            {
                bttMap[i - 1].interactable = true;
                tickUse[i - 1].SetActive(false);
            }
        }
    }


    public void OnClickSelectMapButton(int mapIndex)
    {
        this.mapIndex = mapIndex;
        GameManager.Instance.UserData.MapIndex = mapIndex;
        SetStateButton();
    }




}
