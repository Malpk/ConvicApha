using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    [System.Serializable]
    public class PlayerRotate : IRotate
    {
        [SerializeField] private float _speedRotate;



        public Quaternion Rotate(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}