using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerRotate : IRotate
    {

        private float _curretAngel = 0;

        private readonly float _speedRotation;
        private readonly Dictionary<Vector2, int> _angel = new Dictionary<Vector2, int>()
        {
            { Vector2.up, 0 },{ new Vector2(1,1),45},
            { Vector2.right, 90 },{ new Vector2(1,-1),135},
            { Vector2.down, 180 },{ new Vector2(-1,-1),-135},
            { Vector2.left, -90 },{ new Vector2(-1,1),-45}
        };
        public PlayerRotate(float speedRotation)
        {
            _speedRotation = speedRotation;
        }
        public Quaternion Rotate(Vector2 direction)
        {
            direction = RoundDirection(direction);
            if (direction == Vector2.zero)
                return Quaternion.Euler(Vector3.zero);
            var target = GetNearestAngel(_curretAngel, _angel[direction]);
            var steep = Mathf.MoveTowards(_curretAngel, target, _speedRotation);
            steep -= _curretAngel;
            _curretAngel += steep;
            if (Mathf.Abs(_curretAngel) >= 360f)
                _curretAngel = 0;
            return Quaternion.Euler(Vector3.forward * steep);
        }
        public int GetNearestAngel(float curretAngel,int target)
        {
            var inverse = AngelInverse(target);
            var targetLocal = Mathf.Abs(target - curretAngel);
            var iversLocal = Mathf.Abs(inverse - curretAngel);
            if (targetLocal > iversLocal)
                return inverse;
            else if (targetLocal < iversLocal)
                return target;
            else
                return target;
        }
        public int AngelInverse(int angel)
        {
            if (angel > 0)
                return angel - 360;
            else if (angel < 0)
                return angel + 360;
            else
                return 360;
        }
        private Vector2 RoundDirection(Vector2 direction)
        {
            float x = RoundAxis(direction.x);
            float y = RoundAxis(direction.y);
            return new Vector2(x, y);
        }
        private int RoundAxis(float value)
        {
            if (value > 0)
                return 1;
            else if (value < 0)
                return -1;
            return 0;
        }
    }
}