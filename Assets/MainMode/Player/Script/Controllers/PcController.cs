using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR|| UNITY_STANDALONE
namespace PlayerComponent
{
    public class PcController : Controller
    {
        [SerializeField] private KeyCode _itemUse = KeyCode.E;
        [SerializeField] private KeyCode _useAbility = KeyCode.R;
        [SerializeField] private KeyCode _artifactUse = KeyCode.Q;
        [SerializeField] private KeyCode _intractive = KeyCode.Space;

        private bool _isBlock = false;

        public override event Use UseItemAction;
        public override event Use UseArtifactAction;
        public override event Use UseAbillityAction;
        public override event Use InteractiveAction;
        public override event Movement MovementAction;

        private void Update()
        {
            Interactive();
            UseItem();
            UseArtifact();
            UseAbillity();
        }
        private void FixedUpdate()
        {
            if(!_isBlock)
                SendDirection();  
        }

        private void SendDirection()
        {
            if (MovementAction != null)
                MovementAction(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }
        private void Interactive()
        {
            if (Input.GetKeyDown(_intractive) && InteractiveAction != null && !_isBlock)
                InteractiveAction();
        }
        private void UseItem()
        {
            if (Input.GetKeyDown(_itemUse) && UseItemAction != null && !_isBlock)
                UseItemAction();
        }
        private void UseAbillity()
        {
            if (!_isBlock)
            {
                if (Input.GetKeyDown(_useAbility) && UseAbillityAction != null)
                {
                    UseAbillityAction();
                }
            }
        }
        private void UseArtifact()
        {
            if (Input.GetKeyDown(_artifactUse) && UseArtifactAction != null && !_isBlock)
                UseArtifactAction();
        }

        public override void Block()
        {
            _isBlock = true;
        }

        public override void UnBlock()
        {
            _isBlock = false;
        }
    }
}
#endif