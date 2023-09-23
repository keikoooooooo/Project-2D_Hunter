using static Delegates;

[System.Serializable]
public class Enemy_Status
{
    public event HealthChangedEventHandler OnHealthChanged;

    public int maxHealth;
    public int currentHealth;
    public int damage;
    public float moveSpeed;
    public int rangeAttack;


    public void Subtract(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
    }
    public bool Is50PercentHealth() => currentHealth <= maxHealth / 2;

    public bool isDie()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }
        return false;
    }
    public void SetStats(EnemyStats_SO statsData, int level)
    {
        maxHealth = statsData.MaxHealth * level;
        currentHealth = maxHealth;
        damage = statsData.Damage * level;
        moveSpeed = statsData.MoveSpeed;
        rangeAttack = statsData.RangeAttack[level - 1];
    }




}
