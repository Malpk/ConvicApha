using System.Collections;
using UnityEngine;
using Zenject;

namespace Trident
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Marker : SpawnPoint
    {
        [SerializeField] private int[] _angels;
        [SerializeField] private float _duration;
        [SerializeField] private TridentSetting _trident;

        private SpriteRenderer _deadLinaSprite;

        private void Awake()
        {
            _deadLinaSprite = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            StartCoroutine(Indicator(_duration));
        }
        private IEnumerator Indicator(float duration)
        {
            float progress = 0;
            Color color = _deadLinaSprite.color;
            while (progress < 1)
            {
                progress += Time.deltaTime / duration;
                _deadLinaSprite.color = new Color(color.r, color.g, color.b, 0.4f * progress);
                yield return null;
            }
            _deadLinaSprite.color = new Color(color.r, color.g, color.b, 0f);
            var trident = InstateObject(_trident);
            trident.transform.parent = transform;
            yield return new WaitUntil(() => (trident == null));
            Destroy(gameObject);
        }

        public override GameObject InstateObject(TridentSetting trident)
        {
            var angel = GetAngel(_angels);
            var offset = RotateVector(trident.OffSet,angel);
            var rotate = Quaternion.Euler(Vector3.forward * angel);
            return Instantiate(trident.InstateObject, transform.position + offset, rotate);
        }
    }
}