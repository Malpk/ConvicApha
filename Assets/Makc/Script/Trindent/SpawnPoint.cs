using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Trident
{
    public abstract class SpawnPoint : MonoBehaviour
    {
        public abstract GameObject InstateObject(GameObject trident, int angle, Vector3 offset);

        protected int GetAngel(int[] angel)
        {
            int index = Random.Range(0, angel.Length);
            return angel[index];
        }

        protected Vector3 RotateVector(Vector3 offset, float angel)
        {
            angel *= Mathf.Deg2Rad;
            float xOffset = offset.x * Mathf.Cos(angel) - offset.y * Mathf.Sin(angel);
            float yOffset = offset.x * Mathf.Sin(angel) + offset.y * Mathf.Cos(angel);
            return new Vector3(xOffset, yOffset);
        }
    }
}