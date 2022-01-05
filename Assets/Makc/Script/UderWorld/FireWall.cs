using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class FireWall : MonoBehaviour
    {
        [Header("Game Setting")]
        [SerializeField] private int _count;
        [SerializeField] private float _speedWallMovement;
        [Header("Scene Setting")]
        [SerializeField] private Vector3 _offset;
        [SerializeField] private GameObject _wall;

        private int[] _angels = new int[] { 90, 180 };

        private void Start()
        {
            StartCoroutine(RunMode());
        }
        private IEnumerator RunMode()
        {
            for (int i = 0; i < _count; i++)
            {
                int index = Random.Range(0, _angels.Length);
                var wall1 = InstateObject(_offset, _angels[index]);
                var wall2 = InstateObject(_offset, InversAngel(_angels[index]));
                yield return new WaitWhile(()=>(wall1!=null || wall2!=null));
            }
            Destroy(gameObject);
        }
        private GameObject InstateObject(Vector3 offset, int angel)
        {
            offset =  offset.Rotate(InversAngel(angel));
            var wall = Instantiate(_wall, offset, Quaternion.Euler(Vector3.forward * angel));
            wall.GetComponent<FireWallMovement>().StartMovement(angel,_speedWallMovement);
            return wall;
        }
        private int InversAngel(int angel)
        {
            return angel - 180;
        }
    }
}