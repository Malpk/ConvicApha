namespace MainMode
{
    public interface IResist
    {
        public void AddResistAttack(DamageInfo effectType,float timeActive);
        public bool IsResist(EffectType type);
        public bool IsResist(AttackType type);
    }
}