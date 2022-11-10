using UnityEngine;

namespace PlayerComponent
{
    public interface ITransport 
    {

        public void Enter(Player player,Rigidbody2D rigidBody ,Controller controller);
        public void Exit();
    }
}