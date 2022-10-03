using UnityEngine;

namespace MainMode.Items
{
    public class TransSphere : ConsumablesItem
    {
        [Header("Setting")]
        [SerializeField] private int _unitDistance;
        [SerializeField] private TransSpherePoint _point;

        public override void Use()
        {
            var point = Instantiate(_point.gameObject, user.transform.position, user.transform.rotation).
                GetComponent<TransSpherePoint>();
            point.Run(user.transform, user.transform.position + point.transform.up * _unitDistance);
        }
    }
}