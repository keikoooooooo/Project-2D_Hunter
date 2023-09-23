using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderMOD : MonoBehaviour
{
    public float value;
    private float maxValueTemp;

    public Slider slider;
    public TextMeshProUGUI textValue;

    void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
        textValue.text = "0";
        maxValueTemp = slider.maxValue;
    }
    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }



    void OnValueChanged(float value)
    {
        if (Mathf.Approximately(value, Mathf.Round(value)))
            textValue.text = value.ToString();
        else
            textValue.text = value.ToString("F2");
       
        this.value = value - slider.minValue;
    }

    public void UpdateValue()
    {
        if(value < slider.maxValue) 
        {
            slider.minValue = value;
            slider.value = slider.minValue;

            textValue.text = Mathf.Approximately(value, Mathf.Round(value)) ? 
                                textValue.text = value.ToString() : textValue.text = value.ToString("F2");
        }
        else
        {
            slider.interactable = false;
            textValue.text = maxValueTemp.ToString();
        }
        value = 0;
    }
    
}
