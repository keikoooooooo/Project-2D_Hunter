using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Stats/Enemy", fileName = "Stats_SO")]
public class EnemyStats_SO : CharacterStats_SO
{
    public int[] RangeAttack;

    [Space]
    public EnemyInformation Information;
}

[Serializable]
public class EnemyInformation
{
    public string EnemyName;
    public Sprite Sprite;
}

