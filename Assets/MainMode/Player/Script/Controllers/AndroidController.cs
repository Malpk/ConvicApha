#if  UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;

namespace PlayerComponent
{
    public class AndroidController : Controller
    {
        [SerializeField] private Joystick _inputMovement;

        private bool _isblock = false;

        public override event Use UseItemAction;
        public override event Use UseArtifactAction;
        public override event Use UseAbillityAction;
        public override event Use InteractiveAction;
        public override event Movement MovementAction;

        private void FixedUpdate()
        {
            if(!_isblock)
            SendDirection();
        }

        public void OnInteractive()
        {
            if (InteractiveAction != null && !_isblock)
                InteractiveAction();
        }
        public void OnUse()
        {
            if (UseItemAction != null && !_isblock)
                UseItemAction();
        }
        public void OnUseAbility()
        {
            if (UseAbillityAction != null && !_isblock)
                UseAbillityAction();
        }
        public void OnUseArtifact()
        {
            if (UseArtifactAction != null && !_isblock)
                UseArtifactAction();
        }
        private void SendDirection()
        {
            if (MovementAction != null && !_isblock)
                MovementAction(_inputMovement.Direction);
        }

        public override void Block()
        {
            _isblock = true;
        }

        public override void UnBlock()
        {
            _isblock = false;
        }
    }
}
#endif