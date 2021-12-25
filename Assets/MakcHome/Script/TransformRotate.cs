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
        private Vector2 DefineAxis(Vector2 direction)
        {
            if (direction.x != 0 && direction.y == 0)
                return Vector2.up;
            else if (direction.x == 0 && direction.y != 0)
                return Vector2.right;
            else
                return Vector2.zero;
        }
    }
}