using UnityEngine;

public class DamageImageFeedback : MonoBehaviour
{
    private Animator animator;
    private readonly int _codeAnim = Animator.StringToHash("DamageImage");
    private PlayerController player;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        GamePlayManager.Instance.E_ActivePlayer += GetPlayer;
    }

    private void OnDestroy()
    {
        if (GamePlayManager.Instance)
            GamePlayManager.Instance.E_ActivePlayer -= GetPlayer;
        if(player != null) 
            player.E_TakeDamage -= SetAnim;
    }

    private void GetPlayer(PlayerController p)
    {
        player = p;
        player.E_TakeDamage += SetAnim;
    }

    private void SetAnim() => animator.Play(_codeAnim);



}
