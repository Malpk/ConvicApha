using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerSpace
{
    public interface IMovement 
    {
        public bool Move(Vector2 direction);
    }
}
