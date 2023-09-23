using UnityEngine;

public class DamageImageFeedback : MonoBehaviour
{
    
    Animator animator;
    readonly int _codeAnim = Animator.StringToHash("DamageImage");
    PlayerController player;


    void Awake()
    {
        animator = GetComponent<Animator>();
        GamePlayManager.Instance.E_ActivePlayer += GetPlayer;
    }

    void OnDestroy()
    {
        if (GamePlayManager.Instance)
            GamePlayManager.Instance.E_ActivePlayer -= GetPlayer;
        if(player != null) 
            player.E_TakeDamage -= SetAnim;
    }

    void GetPlayer(PlayerController p)
    {
        player = p;
        player.E_TakeDamage += SetAnim;
    }

    void SetAnim() => animator.Play(_codeAnim);



}
