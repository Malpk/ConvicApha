using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public class Cleaner : GameMode
    {
        [SerializeField] private float _speedRotation;

        protected override void ModeUpdate()
        {
            transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation * Time.deltaTime);
        }

    }
}