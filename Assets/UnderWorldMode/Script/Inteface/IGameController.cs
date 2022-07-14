using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponentLegacy
{
    public interface IGameController
    {
        public float RotateInput { get; }
        public Vector2 MovementInput { get; }
    }
}