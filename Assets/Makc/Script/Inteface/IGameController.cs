using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    public interface IGameController
    {
        public float RotateInput { get; }
        public Vector2 MovementInput { get; }
    }
}