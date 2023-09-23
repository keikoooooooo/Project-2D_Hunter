using UnityEngine;
using UnityEngine.Events;


public enum CutSceneAnimationName
{
    Icon1,
    Icon2,
    IconA,
    IconS,
    IconD,
    IconW,
    IconAW,
    IconWD,
    IconAS,
    IconSD,
    MouseL,
    MouseR,
    Default
}

public class CutSceneCheckTrigger : MonoBehaviour
{
    public CutSceneAnimationName AnimationName;

    [Space(10)]
    public UnityEvent<CutSceneAnimationName> E_Trigger;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerTest"))
        {
            E_Trigger?.Invoke(AnimationName);
        }
    }




}
