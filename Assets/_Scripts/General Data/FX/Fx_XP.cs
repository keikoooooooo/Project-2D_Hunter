using System;
using UnityEngine;

public class Fx_XP : MonoBehaviour , IPool<Fx_XP>
{
    [SerializeField] ParticleSystem particleColor;

    ParticleSystemRenderer particleRenderer;
    Material paticleMaterial;

    [SerializeField] Color[] colorXp = new Color[4];
    int level = 0;

    Action<Fx_XP> action;
    Audio _audio;

    void Awake()
    {
        _audio = GetComponent<Audio>();
        particleRenderer = particleColor.GetComponent<ParticleSystemRenderer>();
        paticleMaterial = particleRenderer.material;
    }


    public void SetStats(Vector2 pos)
    {
        transform.position = pos;
        RandomXP();
    }
    void RandomXP()
    {
        float val = UnityEngine.Random.value;      
        level = 0;
        if (val <= .5f) 
        {
            paticleMaterial.color = colorXp[0];
            level = 1;
        }
        else if (val <= .75f)
        {
            paticleMaterial.color = colorXp[1];
            level = 2;
        }
        else if (val <= .9f)
        {
            paticleMaterial.color = colorXp[2];
            level = 3;
        }
        else
        {
            paticleMaterial.color = colorXp[3];
            level = 4;
        }
    }

    
    public void Init(Action<Fx_XP> returnAction) => action = returnAction;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            XPManager.Instance.IncreaseXP(level);
            _audio.Play();
            action(this);
        }
    }

}
