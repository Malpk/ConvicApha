using UnityEngine;

namespace PlayerComponent
{
    public abstract class Transport : MonoBehaviour
    {
        public abstract bool IsActive { get; }

        public abstract void Enter(Rigidbody2D rigidBody);
        public abstract void Exit();

        public abstract void Move(Vector2 direction);
    }
}