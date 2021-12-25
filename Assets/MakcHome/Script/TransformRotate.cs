using UnityEngine;

namespace PlayerSpace
{
    public class TransformRotate : IRotate
    {
        private Vector2 _curretDiretion; 

        private readonly float speedRotataion;


        public TransformRotate(float speedRotation)
        {
            speedRotataion = speedRotation;
        }

        public Quaternion Rotate(Vector2 direction)
        {
            return Quaternion.Euler(Vector3.forward * direction.x * speedRotataion);
        }
    }
}