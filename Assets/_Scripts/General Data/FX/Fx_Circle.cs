using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class Fx_Circle : MonoBehaviour , IPool<Fx_Circle> 
{
    public event Action<Fx_Circle> E_EndCircleEffect; // khi hết animation -> gọi event

    SkeletonAnimation skeleton;
    Spine.AnimationState animationState;

    Action<Fx_Circle> action;
    public void Init(Action<Fx_Circle> action)=> this.action = action;
 
    void Awake()
    {
        skeleton = GetComponent<SkeletonAnimation>();
        animationState = skeleton.AnimationState;
        animationState.Complete += OnCompleteAnimation;
    }

    void OnDestroy() => animationState.Complete -= OnCompleteAnimation;


    void OnCompleteAnimation(TrackEntry trackEntry)
    {
        E_EndCircleEffect?.Invoke(this);
        action(this);
    }


    public void SetStats(Vector3 pos)
    {
        transform.position = pos;
        animationState.SetAnimation(0, "anim_in", false);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
    }

}
