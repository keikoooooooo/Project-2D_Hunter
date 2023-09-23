using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMOD : MonoBehaviour
{
    public int value;

    public TMP_InputField textValue;
    [SerializeField] Button bttSubstract;
    [SerializeField] Button bttIncrease;


    void OnEnable()
    {
        bttSubstract.onClick.AddListener(OnClickSubtractButton);
        bttIncrease.onClick.AddListener(OnClickIncreaseButton);
        textValue.onEndEdit.AddListener(OnEndEditInputFile);
    }
    void Start()
    {
        value = 0;
        textValue.text = value.ToString();
    }
    void OnDisable()
    {
        bttSubstract.onClick.RemoveListener(OnClickSubtractButton);
        bttIncrease.onClick.RemoveListener(OnClickIncreaseButton);
        textValue.onEndEdit.RemoveListener(OnEndEditInputFile);
    }



    public void ResetValue()
    {
        value = 0;
        textValue.text = value.ToString();
    }


    // Onclick 
    private void OnEndEditInputFile(string value)
    {
        if (value.Length == 0) return;
        int data = int.Parse(textValue.text);
        this.value += data;
    }
    private void OnClickSubtractButton()
    {
        value -= 1;
        if(value < 0) value = 0;
        textValue.text = value.ToString();
    }
    private void OnClickIncreaseButton()
    {
        value += 1;
        textValue.text = value.ToString();
    }






}
