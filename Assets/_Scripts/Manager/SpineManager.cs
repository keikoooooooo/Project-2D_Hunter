using Spine;
using Spine.Unity;

public static class SpineManager
{
    public static TrackEntry SetAnimation(AnimationState animationState, string animationName)
    {
        TrackEntry trackEntry = animationState.SetAnimation(0, animationName, false);
        switch (trackEntry.Animation.Name)
        {
            case "Idle":
            case "Walk":
                trackEntry.Loop = true; 
                break;
        }
        return trackEntry;
    }

    public static void SetSkin(SkeletonAnimation skeletonAnimation, string nameSkin) => skeletonAnimation.skeleton.SetSkin(nameSkin); // set skin theo tên 


}
