using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

namespace PlayerComponent
{
    public interface IMovement
    {
        public void Move(Vector2 direction);
    }
}