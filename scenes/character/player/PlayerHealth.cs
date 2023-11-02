namespace Character;

public partial class Player : CharacterBody2D
{
    int numberCollidingBodies = 0;
    void OnBodyEntered(Node2D body)
    {
        numberCollidingBodies++;
        CheckDealDamage();
    }

    void OnBodyExited(Node2D body)
    {
        numberCollidingBodies--;
    }

    void CheckDealDamage()
    {
        if(numberCollidingBodies == 0 || !damageIntervalTimer.IsStopped()) return;
        healthComponent.TakeDamage(10);
        damageIntervalTimer.Start();
    }

    void UpdateHealthDisplay()
    {
        healthBar.Value = healthComponent.GetHealthPercentage();
    }

    void OnDamageIntervalTimerTimeout()
    {
        CheckDealDamage();
    }

    void OnHealthChanged()
    {
        UpdateHealthDisplay();
    }
}