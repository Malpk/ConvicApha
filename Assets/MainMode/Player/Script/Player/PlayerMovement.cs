using UnityEngine;
using System.Collections;
using PlayerComponent;
using System.Collections.Generic;
using MainMode.Effects;

namespace PlayerComponent
{
    [System.Serializable]
    public class PlayerMovement : IMovement
    {

        private Character character;
        private Rigidbody2D rigidBody;

        public void Intializate(Character character, Rigidbody2D rigidBody)
        {
            this.character = character;
            this.rigidBody = rigidBody;
        }

        public void Move(Vector2 move)
        {
            if (move != Vector2.zero)
                rigidBody.AddForce(move, ForceMode2D.Force);
        }
        public void Rotate(Vector2 direction, float speedRotatoin)
        {
            Quaternion rotation = Quaternion.Lerp(character.Rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotatoin * Time.deltaTime);
            rigidBody.MoveRotation(rotation);
        }

    }
}