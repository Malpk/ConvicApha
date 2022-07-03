namespace MainMode
{
    public interface IResist
    {
        public void AddResistAttack(AttackInfo effectType,float timeActive);
        public bool IsResist(EffectType type);
        public bool IsResist(AttackType type);
    }
}