namespace PlayerComponent
{
    public class JmanBehaviur : PlayerBaseBehaviour
    {
        public override void AddEffects(MovementEffect effect, float timeActive)
        {
            if (playerAbillitySet is JmanAbilitySet set)
            {
                if (!set.IsActive)
                    base.AddEffects(effect, timeActive);
            }

        }
        public override bool TakeDamage(int damage, DamageInfo damgeInfo)
        {
            if (playerAbillitySet is JmanAbilitySet set)
            {
                if (set.IsActive)
                    return false;
            }
            return base.TakeDamage(damage, damgeInfo);
        }
    }
}