using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Marker : MonoBehaviour
    {
        [Header("Color Setting")]
        [Range(0f,1f)]
        [SerializeField] private float _maxTransparency;

        private SpriteRenderer _sprite;
        private Coroutine _startCorotine;

        public bool IsActive => _startCorotine != null;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            DeActive();
        }
        public bool ActiveMarker(Vector3 position, float angle, float warningTime)
        {
            if (_startCorotine != null)
                return false;
            _sprite.enabled = true;
            transform.position = position;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            _startCorotine = StartCoroutine(ColorAniamtion(warningTime));
            return true;
        }
        private IEnumerator ColorAniamtion(float duration)
        {
            float progress = 0f;
            Color startColor = _sprite.color;
            while (progress < 1f)
            {
                progress += Time.deltaTime / duration;
                _sprite.color = new Color(startColor.r, startColor.g, startColor.b, _maxTransparency * progress);
                yield return null;
            }
            DeActive();
        }
        private void DeActive()
        {
            _sprite.enabled = false;
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
            _startCorotine = null;
        }
    }
}