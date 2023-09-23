using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image fillBack;

    [SerializeField] TextMeshProUGUI textValue;

    [SerializeField] Color green, red;
    [SerializeField] bool enemy, player;


    Animator animator;
    int nameCodeAnimator;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        animator = GetComponent<Animator>();
        nameCodeAnimator = Animator.StringToHash("HealthBar");
    }
    void OnEnable() => slider.onValueChanged.AddListener(SetFillBack);
    void OnDestroy() => slider.onValueChanged.RemoveListener(SetFillBack);
    void Start()
    {
        if (player) // fill -> màu xanh nếu là player
        {
            fill.color = green;
        }
        else if (enemy) // -> màu đỏ là enemy
        {
            fill.color = red;
        }
    }

    public void Init(int value)
    {
        SetMaxValue(value);
        SetValue(value);
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        gameObject.SetActive(true);
    }

    public void SetValue(int value)
    {
        slider.value = value;
        textValue.text = value.ToString();

        fillBack.fillAmount = slider.value / slider.maxValue;

        if(value == 0) 
            gameObject.SetActive(false);
    }
   

    void SetFillBack(float val) => fillBack.DOFillAmount(val / slider.maxValue, 2.5f).SetEase(Ease.Linear);
    public void PlayAnimationHeal() => animator.Play(nameCodeAnimator);

}
