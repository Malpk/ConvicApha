public interface IDamage 
{
    public void Explosion(AttackType attack = AttackType.None);
    public void TakeDamage(int damage, DamageInfo type);
}
