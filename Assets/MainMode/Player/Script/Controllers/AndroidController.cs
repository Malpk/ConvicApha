#if  UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;

namespace PlayerComponent
{
    public class AndroidController : Controller
    {
        [SerializeField] private Joystick _inputMovement;

        public override event Use UseItemAction;
        public override event Use UseArtifactAction;
        public override event Use UseAbillityAction;
        public override event Use InteractiveAction;
        public override event Movement MovementAction;

        private void FixedUpdate()
        {
            SendDirection();
        }

        public void OnInteractive()
        {
            if (InteractiveAction != null)
                InteractiveAction();
        }
        public void OnUse()
        {
            if (UseItemAction != null)
                UseItemAction();
        }
        public void OnUseAbility()
        {
            if (UseAbillityAction != null)
                UseAbillityAction();
        }
        public void OnUseArtifact()
        {
            if (UseArtifactAction != null)
                UseArtifactAction();
        }
        private void SendDirection()
        {
            if (MovementAction != null)
                MovementAction(_inputMovement.Direction);
        }
    }
}
#endif