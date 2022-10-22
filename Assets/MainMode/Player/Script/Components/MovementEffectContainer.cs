namespace PlayerComponent
{
    public class MovementEffectContainer : PlayerContainer<MovementEffect>
    {
        public MovementEffectContainer()
        {
            changeContainerAction += SetMovementEffect;
        }
        ~MovementEffectContainer()
        {
            changeContainerAction -= SetMovementEffect;
        }

        public float MoveEffect { get; private set; } = 1f;


        private void SetMovementEffect()
        {
            MoveEffect = 1f;
            foreach (var effect in contents)
            {
                MoveEffect *= effect.content.Value;
            }
        }
    }
}