using System.Collections;
using UnityEngine;
using Zenject;

namespace Trident
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Marker : SpawnPoint
    {
        public bool Constructor(int[] angls, float warningTime, Vector3 offset, GameObject trident)
        {
            if (angls != null)
            {
                StartCoroutine(Indicator(warningTime, angls, offset, trident));
                return true;
            }
            else 
            {
                return false;
            }
        }
        private IEnumerator Indicator(float warningTime,int[] angls, Vector3 offset, GameObject trident)
        {
            yield return StartCoroutine(ColorAnimation(warningTime));
            var index = Random.Range(0, angls.Length);
            var newTrident = InstateObject(trident, angls[index], offset);
            newTrident.transform.parent = transform;
            newTrident.transform.localPosition = Rotate(angls[index], offset);
            yield return new WaitWhile(() => (newTrident != null));
            Destroy(gameObject);
        }
        private IEnumerator ColorAnimation(float warningTime)
        {
            float progress = 0;
            var deadLinaSprite = GetComponent<SpriteRenderer>();
            Color color = deadLinaSprite.color;
            while (progress < 1)
            {
                progress += Time.deltaTime / warningTime;
                deadLinaSprite.color = new Color(color.r, color.g, color.b, 0.4f * progress);
                yield return null;
            }
            deadLinaSprite.color = new Color(color.r, color.g, color.b, 0f);
        }
        public override GameObject InstateObject(GameObject trident,int angle, Vector3 offset)
        {
            var rotate = Quaternion.Inverse(Quaternion.Euler(Vector3.forward * angle));
            return Instantiate(trident, transform.position, rotate);
        }
        private Vector3 Rotate(float angel, Vector3 vector)
        {
            angel *= Mathf.Deg2Rad;
            var x = vector.x * Mathf.Cos(angel) - vector.y * Mathf.Sin(angel);
            var y = vector.x * Mathf.Sin(angel) + vector.y * Mathf.Cos(angel);
            return new Vector3(x, y, vector.z);
        }
    }
}