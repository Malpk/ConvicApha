using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerComponentLegacy
{
    public interface IMovement 
    {
        public bool Move(Vector2 direction);
    }
}
