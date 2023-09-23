using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotEnough : SingletonManager<NotEnough>
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Animator animator;

    Image panel;

    int codeAnimator = Animator.StringToHash("NotEnough");

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
