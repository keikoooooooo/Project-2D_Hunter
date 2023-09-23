using System;
using UnityEngine;

public class AnimationSkills : MonoBehaviour, IPool<AnimationSkills>
{ 
    int damage;

    Animator animator;
    Action<AnimationSkills> action;

    bool isSetAnimator = false; // đánh dấu là đã gán controller cho animator chưa ?

    private readonly static int Code1 = Animator.StringToHash("Skill 1"); // lấy code để chạy animation chung
    private readonly static int Code2 = Animator.StringToHash("Skill 2");
    private readonly static int Code3 = Animator.StringToHash("Skill 3");


    void Awake() => animator = GetComponent<Animator>();
    void OnDisable() => Action();

    public void SetStats(AnimatorOverrideController controller, Vector3 pos, int damage)
    {
        if (!isSetAnimator)
        {
            animator.runtimeAnimatorController = controller;
            isSetAnimator = true;
        }

        transform.position = pos;
        this.damage = damage;
    }

    public void PlaySkill1() => animator.Play(Code1); // chạy animation theo code
    public void PlaySkill2() => animator.Play(Code2);
    public void PlaySkill3() => animator.Play(Code3);


    public void Action()
    {
        if(gameObject.activeSelf) { action(this); }
    }
    public void Init(Action<AnimationSkills> action) => this.action = action; 


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(damage);
        }
    }


}
