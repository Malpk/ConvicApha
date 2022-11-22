namespace PlayerComponent
{
    public class JmanBehaviur : PlayerBaseBehaviour
    {
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