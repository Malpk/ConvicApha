using System.Collections;
using UnityEngine;

namespace PlayerComponent
{
    public class JmanAbilitySet : PlayerAbillityUseSet
    {
        [Header("Ability")]
        [Min(0.1f)]
        [SerializeField] private float _duration = 1;
        [Min(1)]
        [SerializeField] private float _distance = 1;
        [Min(1)]
        [SerializeField] private float _timeReload = 1;
        [SerializeField] private float _timeActiveDebaf;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private MovementEffect _debaf;

        private bool _isReload;

        public override bool IsReload => _isReload;

        protected override void UseAbility()
        {
            var distance = _distance;
            var hit = Physics2D.Raycast(transform.position, transform.up, distance, _wallLayer);
            if (hit)
            {
                distance = Vector2.Distance(transform.position, hit.point);
            }
            StartCoroutine(Jerk(distance, _duration));
        }
        private IEnumerator Jerk(float distance, float duration)
        {
            _isReload = true;
            yield return JerkUpdate(user.transform.position, user.transform.up * distance, duration);
            user.AddEffects(_debaf, _timeActiveDebaf);
            yield return new WaitForSeconds(_timeReload);
            _isReload = false;
        }

        private IEnumerator JerkUpdate(Vector2 start, Vector2 move, float duration)
        {
            user.Block();
            var progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime / duration;
                user.MoveToPosition(start + move * progress);
                yield return new WaitForFixedUpdate();
            }
            user.UnBlock();
        }
    }
}
