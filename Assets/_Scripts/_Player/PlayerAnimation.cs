using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    const string A_Idle = "Idle";
    const string A_Walk = "Walk";
    const string A_Dead = "Dead";

    const string A_Attack = "Attack";
    const string A_AttackAdd = "Attack Add";

    const string A_HoldAttack = "Hold Attack";
    const string A_DropAttack = "Drop Attack";

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;

    Vector3 sizeChar;

    void Awake() => animationState = skeletonAnimation.AnimationState;
    void Start()
    {
        Idle();
        sizeChar = skeletonAnimation.transform.localScale;
    }
    public TrackEntry GetTrackEntry() => animationState.GetCurrent(0); // lấy thông tin của animation hiện tại
    public void Flip(float posX) // lật animation theo hướng di chuyển
    {
        if(posX > 0) skeletonAnimation.transform.localScale = new Vector2(sizeChar.x, sizeChar.y);
        else if (posX < 0) skeletonAnimation.transform.localScale = new Vector2(-sizeChar.x, sizeChar.y);
    }  

    public void Idle() => SpineManager.SetAnimation(animationState, A_Idle);
    public void Walk() => SpineManager.SetAnimation(animationState, A_Walk);
    public void Dead() => SpineManager.SetAnimation(animationState, A_Dead);

    public void Attack(float value) => SpineManager.SetAnimation(animationState, A_Attack).TimeScale = value;  // attack normal
    public void AttackAdd(float value) => SpineManager.SetAnimation(animationState, A_AttackAdd).TimeScale = value;

    public void AttackHold() => SpineManager.SetAnimation(animationState, A_HoldAttack); // attack special
    public void AttackDrop() => SpineManager.SetAnimation(animationState, A_DropAttack);

    public void SetSkin(int indexSkin) => SpineManager.SetSkin(skeletonAnimation, $"V{indexSkin}");

}
