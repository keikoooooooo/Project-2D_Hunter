using UnityEngine;

public class CharacterStats_SO : ScriptableObject
{
    [Space]
    public int MaxHealth;
    public int Damage;
    public float MoveSpeed; 
    public float AttackCooldown; // thời gian tấn công (Special)
}