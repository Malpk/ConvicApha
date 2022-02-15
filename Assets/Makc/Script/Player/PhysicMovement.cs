using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PhysicMovement : IMovement
    {
        private readonly Rigidbody2D rigidbody;
        private readonly float speedMovement;

        public PhysicMovement(Rigidbody2D rigidbody, float speedMovement)
        {
            this.rigidbody = rigidbody;
            this.speedMovement = speedMovement;
        }

        public bool Move(Vector2 direction)
        {
            var movement = direction * speedMovement;
            float y = rigidbody.velocity.y;
            rigidbody.velocity = movement;
            if (direction == Vector2.zero)
                return false;
            else
                return true;
        }
    }
}