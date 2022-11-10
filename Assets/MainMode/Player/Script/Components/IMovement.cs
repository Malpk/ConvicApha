using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public interface IMovement
    {
        public void Move(Vector2 direction);
    }
}