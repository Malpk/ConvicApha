using UnityEngine;

namespace PlayerComponent
{
    [System.Serializable]
    public class PlayerMovement : IMovement
    {
        private Transform _player;
        private Rigidbody2D _rigidBody;

        public void Intializate(Player player, Rigidbody2D rigidBody)
        {
            _player = player.transform;
            _rigidBody = rigidBody;
        }

        public void Move(Vector2 move)
        {
            if (move != Vector2.zero)
                _rigidBody.AddForce(move, ForceMode2D.Force);
        }
        public void Rotate(Vector2 direction, float speedRotatoin)
        {
            Quaternion rotation = Quaternion.Lerp(_player.rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotatoin * Time.deltaTime);
            _rigidBody.MoveRotation(rotation);
        }

    }
}