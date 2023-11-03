namespace Character;

public partial class Player : CharacterBody2D
{
    void OnAreaEntered(Node2D body)
    {
        if (body.GetParent() is EnemyAttackAbility enemyAttackAbility)
        {
            if (enemyAttackAbility.HitboxComponent.Damage > 0)
            {
                DealDamage((int) enemyAttackAbility.HitboxComponent.Damage);
                enemyAttackAbility.QueueFree();
            }
        }
    }


    public void DealDamage(int amount)
    {
        healthComponent.TakeDamage(amount);
    }

    void UpdateHealthDisplay()
    {
        healthBar.Value = healthComponent.GetHealthPercentage();
    }

    void OnHealthChanged()
    {
        UpdateHealthDisplay();
    }
}