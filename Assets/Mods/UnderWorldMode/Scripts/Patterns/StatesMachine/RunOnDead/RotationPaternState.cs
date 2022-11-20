using UnityEngine;

namespace Underworld
{
    public class RotationPaternState : BasePatternState
    {
        private readonly int[] _direction = new int[] { -1, 1 };
        private readonly float duration = 0;
        private readonly float speedRotation;

        private float _progress = 0f;
        private float _curretSpeedRotation;

        public RotationPaternState(float speedRotation, float duration)
        {
            this.duration = duration;
            this.speedRotation = speedRotation;
        }

        public System.Action<Quaternion> OnUpdate;

        public override bool IsComplite => _progress >= 1f;

        public override void Start()
        {
            _progress = 0f;
            _curretSpeedRotation = ChooseDirection() * speedRotation;
        }

        public override bool Update()
        {
            _progress += Time.deltaTime / duration;
            OnUpdate?.Invoke(Quaternion.Euler(Vector3.forward * _curretSpeedRotation * Time.deltaTime));
            return _progress < 1f;
        }
        private int ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            return _direction[index];
        }
    }
}