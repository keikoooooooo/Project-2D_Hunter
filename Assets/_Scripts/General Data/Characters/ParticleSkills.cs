using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ParticleSkills : MonoBehaviour, IPool<ParticleSkills>
{

    [SerializeField] bool isUsePhysics; // có sử dụng vật lí không ? 
    [SerializeField] bool isActionAuto; // có tự động trả về pool ?
    [SerializeField] float timeReturn;

    int damage;

    Action<ParticleSkills> action;
    Rigidbody2D rb;

    void Awake()
    {
        if (isUsePhysics)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    void OnEnable()
    {
        if (isActionAuto)
        {
            StartCoroutine(ActionCoroutine());
        }
    }

    public IEnumerator ActionCoroutine()
    {
        yield return new WaitForSeconds(timeReturn); // sau (time Return)? giây -> skill này sẽ được trả về lại pool
        Action();
    }


    public void SetStats(Vector3 pos, int damage)
    {
        transform.position = pos;
        this.damage = damage;
    }
    public void Init(Action<ParticleSkills> action) => this.action = action;    
    public void Shoot(Vector2 direction, float force) => rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(damage);
            Action();
        }   
    }

    public void Action() => transform.DOScale(Vector2.zero, .35f).OnComplete(() =>
    {
        action(this);
        transform.localScale = new Vector2(1f, 1f);
    });

}

