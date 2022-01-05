using System.Collections;
using UnityEngine;

namespace Underworld
{
    public class FireWallMovement : MonoBehaviour
    {
        private bool _isRun = false;

        public bool StartMovement(int angel,float speedMovement )
        {
            if (!_isRun)
            {
                _isRun = true;
                StartCoroutine(Move(angel, speedMovement));
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator Move(float angel,float speedMovement)
        {
            float progress = 0f;
            var direction = Vector3.right;
            direction.Rotate(angel);
            var distanse = Vector3.Distance(transform.position, transform.position * -1);
            while (progress < distanse)
            {
                var speed = speedMovement * Time.deltaTime;
                transform.Translate(direction * speed);
                progress += speed;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}