using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public class CirculMode : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private int _countPeriod;
        [SerializeField] private float _duration;
        [SerializeField] private float[] _radius;

        [SerializeField]private CircleCollider2D[] _collider;

        public override bool statusWork => true;

        protected override void ModeUpdate()
        {
        }

        private void Start()
        {
            StartCoroutine(Animation());
        }
        private IEnumerator Animation()
        {
            var period = _duration / _countPeriod;
            while (true)
            {
                //var steepScale = 0f;
                //foreach (var radius in _radius)
                //{
                //    var steep = Mathf.Abs(_collider.radius - radius) / period;
                //    while (Mathf.Abs(_collider.radius - radius) > 0.01f)
                //    {
                //        var steepScale = Mathf.MoveTowards(_collider.radius, radius, steep);
                    
                //        yield return null;
                //    }
                //}
                yield return null;
            }
        }
    }
}