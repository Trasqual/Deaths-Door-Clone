public interface IDamagable
{
    public void TakeDamage(int amount, DamageDealerType damageDealerType);
    public void Die();
}