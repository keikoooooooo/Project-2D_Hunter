using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static Delegates;

[System.Serializable]
public struct Player_Status
{
    public event HealthChangedEventHandler OnHealthChanged;

    public int maxHealth;
    public int currentHealth;
    public int damageNormal;
    public int damageSpecial;
    public int armor;
    public float moveSpeed;
    public float rangeAttack;
    public float attackSpeed;

    float specialMultiple;

    public void Subtract(int amount)   // trừ current theo giá trị truyền vào
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
    }
    public void Heal(int amount)  // cộng current theo giá trị truyền vào
    {
        if (currentHealth + amount > maxHealth)
            amount = maxHealth - currentHealth;

        currentHealth += amount;
        OnHealthChanged?.Invoke(currentHealth);
    }   
    public bool isDie() // check current = 0
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthChanged?.Invoke(currentHealth);
            return true;
        }
        return false;
    }
    public bool IsHeal() => currentHealth < maxHealth && currentHealth > 0; // check có được hồi máu không ?

    public void SetStats(PlayerStats_SO statsData) // set các thông số theo level
    {
        maxHealth = statsData.MaxHealth;
        currentHealth = maxHealth;
        damageNormal = statsData.Damage;
        specialMultiple = statsData.SpecialMultiple;
        damageSpecial = Mathf.FloorToInt(damageNormal * specialMultiple);
        armor = statsData.Armor;
        moveSpeed = statsData.MoveSpeed;
        rangeAttack = statsData.RangeAttack;
        attackSpeed = 1;
    }

    public void SetStats(int _health, int _dmg, float _moveSpeed, float _attackSpeed)
    {
        maxHealth += _health;
        currentHealth += _health;
        damageNormal += _dmg;
        damageSpecial = Mathf.FloorToInt(damageNormal * specialMultiple);
        moveSpeed += _moveSpeed;
        attackSpeed += _attackSpeed;
    }

}


