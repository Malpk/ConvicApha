using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

namespace PlayerComponent
{
    public abstract class Controller : MonoBehaviour,IBlock
    {
        public delegate void Use();
        public delegate void Movement(Vector2 direction);

        public abstract event Use UseItemAction;
        public abstract event Use UseArtifactAction;
        public abstract event Use UseAbillityAction;
        public abstract event Use InteractiveAction;
        public abstract event Movement MovementAction;

        public abstract void Block();

        public abstract void UnBlock();
    }
}
