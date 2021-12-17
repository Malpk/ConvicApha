using UnityEngine;

namespace PlayerSpace
{
    public class PlayerRotation : IRotate
    {
        private int[,] _listAngel = new int[3, 3]
        {
            { 0, -90 , 90 },
            { 180 , -135 , 135 },
            { 0, -45, 45}
        };

        private float _curretAngel;
        private float _previousTargetAngel;
        private readonly float speedRotation;


        public PlayerRotation(float speedRotation)
        {
            this.speedRotation = speedRotation;

        }
        public Quaternion Rotate(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                _previousTargetAngel = GetTargetAngel(direction, _previousTargetAngel);
                _curretAngel = Mathf.MoveTowards(_curretAngel, _previousTargetAngel, speedRotation);
            }
            return Quaternion.Euler(Vector3.forward * _curretAngel);
        }
        private float GetTargetAngel(Vector2 direction, float lostAngel)
        {
            if (direction != Vector2.zero)
            {
                int x = GetIndex(direction.x);
                int y = GetIndex(direction.y);
                return _listAngel[y,x];
            }
            else 
            {
                return lostAngel;
            }
        }
        private int GetIndex(float direction)
        {
            if (direction > 0)
                return 2;
            else if (direction < 0)
                return 1;
            return 0;
        }
    }
}
