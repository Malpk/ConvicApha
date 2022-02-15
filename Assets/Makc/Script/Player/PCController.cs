using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PCController : MonoBehaviour, IGameController
    {
        public float RotateInput => GetRotateInput();

        public Vector2 MovementInput => GetInputMovement();

        private Vector2 GetInputMovement()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            return new Vector2(x, y);
        }
        private float GetRotateInput()
        {
            return Input.GetAxis("Mouse X");
        }
    }
}