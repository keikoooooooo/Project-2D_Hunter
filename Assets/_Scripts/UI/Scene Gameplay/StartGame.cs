using UnityEngine;

public class StartGame : MonoBehaviour
{
    Animator animator;
    CountdownTime countdownTime;
    PlayerController player;

    public int timer;

    [SerializeField] EndGame endGame;
    [Space]
    [SerializeField] GameObject PanelStartGame;


    void Awake()
    {
        animator = GetComponent<Animator>();
        countdownTime = GetComponent<CountdownTime>();
        GamePlayManager.Instance.E_ActivePlayer += GetPlaye;
        countdownTime.E_EndCountdownTime += EndCooldown;
        countdownTime.E_TimeCount += SetTimer;
    }
    void OnDisable()
    {
        if (GamePlayManager.Instance ) 
            GamePlayManager.Instance.E_ActivePlayer -= GetPlaye;
        countdownTime.E_EndCountdownTime -= EndCooldown;
        countdownTime.E_TimeCount -= SetTimer;
    }
    



    public void Begin() => animator.SetTrigger("isStart");
    private void GetPlaye(PlayerController p) => player = p;
    private void SetTimer(int time) => timer = time;
    private void EndCooldown() => animator.SetTrigger("isTimeUp");

    public int CurrentCountdownTime() => countdownTime._countTime;

 


    public void SetStartGame() // gọi bằng event Animation
    {
        if (player != null) player.isPaused = false;

        countdownTime.StartCountDown();
    }
    public void StopGame() => countdownTime.StopCountDown();
    public void SetTimeUp() // gọi bằng event Animation
    {
        if(player != null)
        {
            if (player.isDie) // nếu player die ? 
            {
                endGame.SetStats(false);
            }
            else // ngược lại ?
            {
                player.isDie = true;
                player.PlayerAnimation.Idle();
                endGame.SetStats(true);
            }
        }
    }


}
