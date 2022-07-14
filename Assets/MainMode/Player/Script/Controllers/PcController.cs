using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR|| UNITY_STANDALONE
namespace PlayerComponent
{
    public class PcController : Controller
    {
        [SerializeField] private KeyCode _itemUse = KeyCode.E;
        [SerializeField] private KeyCode _useAbility = KeyCode.Space;
        [SerializeField] private KeyCode _artifactUse = KeyCode.Q;
        [SerializeField] private KeyCode _intractive = KeyCode.F;

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
            SendDirection();  
        }

        private void SendDirection()
        {
            if (MovementAction != null)
                MovementAction(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }
        private void Interactive()
        {
            if (Input.GetKeyDown(_intractive) && InteractiveAction != null)
                InteractiveAction();
        }
        private void UseItem()
        {
            if (Input.GetKeyDown(_itemUse) && UseItemAction != null)
                UseItemAction();
        }
        private void UseAbillity()
        {
            if (Input.GetKeyDown(_useAbility) && UseAbillityAction != null)
                UseAbillityAction();
        }
        private void UseArtifact()
        {
            if (Input.GetKeyDown(_artifactUse) && UseArtifactAction != null)
                UseArtifactAction();
        }
    }
}
#endif