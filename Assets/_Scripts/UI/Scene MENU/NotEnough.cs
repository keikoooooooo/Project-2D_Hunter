using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotEnough : SingletonManager<NotEnough>
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private Image panel;

    readonly int codeAnimator = Animator.StringToHash("NotEnough");

    private void Start()
    {
        panel = GetComponent<Image>();
        panel.enabled = false;
        text.gameObject.SetActive(false);
    }

    public void ActiveNotEnough(string textTitle)
    {
        text.text = textTitle;
        animator.Play(codeAnimator);
    }


}
