using UnityEngine;

namespace PlayerComponent
{
    public abstract class Transport : MonoBehaviour, IMovement
    {

        public abstract void Enter(Player player,Rigidbody2D rigidBody ,PCPlayerController controller);
        public abstract void Exit();

        public abstract void Move(Vector2 direction);
    }
}