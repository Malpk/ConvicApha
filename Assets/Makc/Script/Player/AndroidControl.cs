using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerComponent
{
    public class AndroidControl : MonoBehaviour,IGameController
    {
        [SerializeField] private Joystick _jostick; 

        public float RotateInput => throw new System.NotImplementedException();

        public Vector2 MovementInput => GetInput();

        private Vector2 GetInput()
        {
            return new Vector2(_jostick.Horizontal, _jostick.Vertical);
        }

    }
}