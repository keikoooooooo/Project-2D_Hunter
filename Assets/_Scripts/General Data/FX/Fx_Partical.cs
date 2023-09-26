using System;
using System.Collections;
using UnityEngine;

public class Fx_Partical : MonoBehaviour, IPool<Fx_Partical> 
{

    [SerializeField] private float delayActive;

    private Action<Fx_Partical> _action;

    private Coroutine activeSkillCoroutine;

    private IEnumerator ActiveSkill()
    {
        yield return new WaitForSeconds(delayActive);

        if(activeSkillCoroutine != null) StopCoroutine(activeSkillCoroutine);
        _action(this);
    }

    public void SetStats(Vector2 pos)
    {
        transform.position = pos;
        activeSkillCoroutine = StartCoroutine(ActiveSkill());
    }

    public void Init(Action<Fx_Partical> returnAction) => _action = returnAction;
}
