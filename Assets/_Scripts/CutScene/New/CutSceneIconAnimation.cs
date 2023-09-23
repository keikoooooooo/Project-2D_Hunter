using UnityEngine;
using UnityEngine.Events;

public class CutSceneIconAnimation : MonoBehaviour
{
    public UnityEvent E_EndManuals;
    public GameObject panelAll;
    public Audio _audio;

    [SerializeField] GameObject panelTextAWSD;
    [SerializeField] GameObject panelTextLeft;
    [SerializeField] GameObject panelTextRight;
    [SerializeField] Animator animatorText;
    readonly int CodeAnimTextMove = Animator.StringToHash("ClickKeyTextMove");
    readonly int CodeAnimTextAttackL = Animator.StringToHash("ClickKeyTextAttackL");
    readonly int CodeAnimTextAttackR = Animator.StringToHash("ClickKeyTextAttackR");
    readonly int CodeAnimTextGreat = Animator.StringToHash("ClickKeyTextGreat");
    readonly int CodeAnimTextSelectAbi = Animator.StringToHash("ClickKeyTextSelectAbi");

    Animator animatorIcon;
    readonly int CodeAnimIconDefault = Animator.StringToHash("Icon_Default");
    readonly int CodeAnimIcon1 = Animator.StringToHash("Icon_1");
    readonly int CodeAnimIconA = Animator.StringToHash("Icon_A");
    readonly int CodeAnimIconD = Animator.StringToHash("Icon_D");
    readonly int CodeAnimIconS = Animator.StringToHash("Icon_S");
    readonly int CodeAnimIconW = Animator.StringToHash("Icon_W");
    readonly int CodeAnimIconAW = Animator.StringToHash("Icon_AW");
    readonly int CodeAnimIconWD = Animator.StringToHash("Icon_WD");
    readonly int CodeAnimIconAS = Animator.StringToHash("Icon_AS");
    readonly int CodeAnimIconSD = Animator.StringToHash("Icon_SD");
    readonly int CodeAnimIconMouseL = Animator.StringToHash("Icon_MouseL");
    readonly int CodeAnimIconMouseR = Animator.StringToHash("Icon_MouseR");

    bool isA, isW, isD, isS;
    bool isClickAttackLeft, isClickAttackRight;
    bool isClickAccpect;

    void Awake()
    {
        animatorIcon = GetComponent<Animator>();
    }
    void Start()
    {
        animatorIcon.Play(CodeAnimIconW);
        animatorText.Play(CodeAnimTextMove);
    }

    void Update()
    {
        if (isW && isA && isD && isS && isClickAccpect && isClickAttackLeft && isClickAttackRight) return;

        if (Input.GetKeyDown(KeyCode.W) && !isW)
        {
            isW = true;
            animatorIcon.Play(CodeAnimIconA);
            _audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.A) && isW && !isA)
        {
            isA = true;
            animatorIcon.Play(CodeAnimIconD);
            _audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.D) && isW && isA && !isD)
        {
            isD = true;
            animatorIcon.Play(CodeAnimIconS);
            _audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.S) && isW && isA && isD && !isS)
        {
            isS = true;
            _audio.Play();
        }

        if(isW && isA && isD && isS && !isClickAccpect)
        {
            isClickAccpect = true;
            animatorText.Play(CodeAnimTextAttackR);
            animatorIcon.Play(CodeAnimIconMouseR);
        }

        if (Input.GetMouseButtonDown(1) && isW && isA && isD && isS && !isClickAttackRight)
        {
            isClickAttackRight = true;
            animatorIcon.Play(CodeAnimIconMouseL);
            animatorText.Play(CodeAnimTextAttackL);
            _audio.Play();
        }
        if (Input.GetMouseButtonDown(0) && isW && isA && isD && isS && !isClickAttackLeft)
        {
            isClickAttackLeft = true;
            animatorIcon.Play(CodeAnimIconDefault);
            animatorText.Play(CodeAnimTextGreat);
            Invoke(nameof(DisablePanelAllText), 1f);
            E_EndManuals?.Invoke();
            _audio.Play();
        }
    }


    void DisablePanelAllText()
    {
        panelAll.SetActive(false);
        animatorIcon.Play(CodeAnimIconW);
    }


    public void EnablePanelTextGreat()
    {
        panelAll.SetActive(true);
        animatorText.Play(CodeAnimTextGreat);
    }
    public void EnablePanelTextSelectAbi()
    {
        panelAll.SetActive(true);
        animatorIcon.Play(CodeAnimIcon1);
        animatorText.Play(CodeAnimTextSelectAbi);
    }

    public void PlayAnimation(CutSceneAnimationName cutSceneAnimationName)
    {
        switch (cutSceneAnimationName)
        {
            case CutSceneAnimationName.IconA:
                animatorIcon.Play(CodeAnimIconA);
                break;
            case CutSceneAnimationName.IconS:
                animatorIcon.Play(CodeAnimIconS);
                break;
            case CutSceneAnimationName.IconD:
                animatorIcon.Play(CodeAnimIconD);
                break;
            case CutSceneAnimationName.IconW:
                animatorIcon.Play(CodeAnimIconW);
                break;
            case CutSceneAnimationName.MouseL:
                animatorIcon.Play(CodeAnimIconMouseL);
                break;
            case CutSceneAnimationName.MouseR:
                animatorIcon.Play(CodeAnimIconMouseR);
                break;
            case CutSceneAnimationName.IconAW:
                animatorIcon.Play(CodeAnimIconAW);
                break;
            case CutSceneAnimationName.IconWD:
                animatorIcon.Play(CodeAnimIconWD);
                break;
            case CutSceneAnimationName.IconAS:
                animatorIcon.Play(CodeAnimIconAS);
                break;
            case CutSceneAnimationName.IconSD:
                animatorIcon.Play(CodeAnimIconSD);
                break;
            case CutSceneAnimationName.Icon1:
                animatorIcon.Play(CodeAnimIcon1);
                break;

            case CutSceneAnimationName.Default:
                animatorIcon.Play(CodeAnimIconDefault);
                break;
        }
    }







}
