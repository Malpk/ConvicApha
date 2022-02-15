using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerComponent
{
    public interface IMovement 
    {
        public bool Move(Vector2 direction);
    }
}
