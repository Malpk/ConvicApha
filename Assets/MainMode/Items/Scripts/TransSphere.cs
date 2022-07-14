using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    public class TransSphere : Artifact
    {
        [Header("Setting")]
        [SerializeField] private int _unitDistance;
        [SerializeField] private TransSpherePoint _point;


        public override void Use()
        {
            var point = Instantiate(_point.gameObject, user.transform.position,Quaternion.identity).GetComponent<TransSpherePoint>();
            point.Run(user.transform, user.transform.position +
                    user.transform.up * _unitDistance);
        }
    }
}