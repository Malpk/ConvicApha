using UnityEngine;

namespace PlayerComponent
{
    public class ContainCell<T>
    {
        public readonly T content;

        private float _progress = 0f;
        private float _timeActive;

        public ContainCell(T effect)
        {
            this.content = effect;
        }

        public void Start(float timeActive)
        {
            _progress = 0;
            if (timeActive > _timeActive)
            {
                _timeActive = timeActive;
            }
        }

        public bool Update()
        {
            _progress += Time.deltaTime / _timeActive;
            return _progress < 1f;
        }
    }
}