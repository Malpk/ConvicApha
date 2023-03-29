using UnityEngine;

namespace PlayerComponent
{
    public class ContainCell<T>
    {
        public readonly EffectType effect;
        public readonly T content;

        private float _progress = 0f;
        private float _timeActive;

        public event System.Action<ContainCell<T>> OnDelete;

        public ContainCell(EffectType effect,T content)
        {
            this.content = content;
            this.effect = effect;
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
            if (_progress >= 1f)
                OnDelete?.Invoke(this);
            return _progress < 1f;
        }
    }
}