using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    const string A_Idle = "Idle";
    const string A_Walk = "Walk";
    const string A_Attack = "Attack";

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    private Vector3 sizeChar;

    void Awake() => animationState = skeletonAnimation.AnimationState;
    void Start()
    {
        Idle();
        sizeChar = skeletonAnimation.transform.localScale;
    }


    public TrackEntry GetTrackEntry() => animationState.GetCurrent(0); // lấy thông tin của animation hiện tại
    public void Flip(float posX)
    {
        if (posX > 0) 
            skeletonAnimation.transform.localScale = new Vector2(sizeChar.x, sizeChar.y);
        else if (posX < 0) 
            skeletonAnimation.transform.localScale = new Vector2(-sizeChar.x, sizeChar.y);
    }

    public void Idle() => SpineManager.SetAnimation(animationState, A_Idle);
    public void Walk() => SpineManager.SetAnimation(animationState, A_Walk);
    public void Attack() => SpineManager.SetAnimation(animationState, A_Attack); 

    public void SetSkin(int indexSkin) => SpineManager.SetSkin(skeletonAnimation, $"V{indexSkin}");
}
