using Cinemachine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private StartGame startGame;
    [Space]
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    [Space]
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject Lose;
    [Space]
    [SerializeField] private TextMeshProUGUI textTrophy;
    [SerializeField] private TextMeshProUGUI textKilled;
    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textToken;
    [Space]
    [SerializeField] private SkeletonGraphic skeletonGraphicLeft;
    [SerializeField] private SkeletonGraphic skeletonGraphicRight;
    [Space]
    [SerializeField] private Button bttClaimX2;
    [SerializeField] private Button bttExit;

    #region Private

    private void Start()
    {
        animator = GetComponent<Animator>();
        panelEnd.SetActive(false);

        if(GamePlayManager.Instance) GamePlayManager.Instance.player.E_EndAnimationDie += HandlerPlayerDie;
        bttClaimX2.onClick.AddListener(OnClickClaimButton);
        bttExit.onClick.AddListener(OnClickExitButton);
    }

    private void OnDestroy()
    {
        if (GamePlayManager.Instance) GamePlayManager.Instance.player.E_EndAnimationDie -= HandlerPlayerDie;
        bttClaimX2.onClick.RemoveListener(OnClickClaimButton);
        bttExit.onClick.RemoveListener(OnClickExitButton);
    }
    #endregion


    private void HandlerPlayerDie()
    {
        SetStats(false);
        startGame.StopGame();
    }


    public void SetStats(bool isState)
    {
        panelEnd.SetActive(true);
        animator.SetTrigger("isEnd");
        SetSkeleton();
        AudioManager.Instance.Mute(AudioMode.Music, true);
        if (isState) // true = win
        {
            Win.SetActive(true);
            Lose.SetActive(false);
            skeletonGraphicLeft.AnimationState.SetAnimation(0, "Victory", false);
            skeletonGraphicRight.AnimationState.SetAnimation(0, "Victory", false);

            AudioManager.Instance.Play(AudioName.Victory);
        }
        else // false = lose
        {
            Win.SetActive(false);
            Lose.SetActive(true);
            skeletonGraphicLeft.AnimationState.SetAnimation(0, "Defeat", false);
            skeletonGraphicRight.AnimationState.SetAnimation(0, "Defeat", false);

            AudioManager.Instance.Play(AudioName.Defeat);
        }
        SetText(isState);
    }

    private void SetSkeleton()
    {
        skeletonGraphic.skeletonDataAsset = GamePlayManager.Instance.player.PlayerAnimation.skeletonAnimation.skeletonDataAsset;
        skeletonGraphic.initialSkinName = $"V{GamePlayManager.Instance.LastUsedCharacterSkin}";
        skeletonGraphic.Initialize(true);
    }

    int trophy, coin, token, killed;

    private void SetText(bool isState)
    {
        trophy = 0; coin = 0; token = 0; killed = 0;

        trophy = isState ? 30 : 10;
        token = isState ? 200 : 50;
        killed = KillNotification.Instance.Count;
        coin = killed;

        float TimeRemaining = startGame.CurrentCountdownTime(); // nếu thời gian chơi chưa qua 1 nữa -> chỉ nhận 1 ít phần thưởng
        if(TimeRemaining >= 90)
        {
            trophy = 1;
            token = 10;
        }
        if(killed <= 0) // nếu số lượng kill enemy = 0 -> nhận ít phần thưởng hơn
        {
            trophy = 0;
            token = 5;
        }

        textTrophy.text = $"+{trophy}";
        textKilled.text = $"<size=40>x</size><size=67.6>{killed}</size>";
        textCoin.text = $"+{coin}";
        textToken.text = $"+{token}";      
    }


    // Onclick Button
    private void OnClickClaimButton()
    {
        // todo
    }
    private void OnClickExitButton()
    {
        GameManager.Instance.TrophyRoadData.IncreaseTrophy(trophy);
        GameManager.Instance.UserData.Coin += coin;
        GameManager.Instance.UserData.Token += token;

        LoadingManager.Instance.LoadScene("MenuGame");
    }


}
