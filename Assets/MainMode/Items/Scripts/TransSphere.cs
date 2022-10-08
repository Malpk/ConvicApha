using UnityEngine;

namespace MainMode.Items
{
    public class TransSphere : ConsumablesItem
    {
        [Header("Setting")]
        [SerializeField] private int _unitDistance;
        [SerializeField] private TransSpherePoint _point;

        private void OnEnable()
        {
            UseAction += Actvate;
        }
        private void OnDisable()
        {
            UseAction -= Actvate;
        }
        private void Actvate()
        {
            var point = Instantiate(_point.gameObject, user.transform.position, user.transform.rotation).
                GetComponent<TransSpherePoint>();
            point.Run(user.transform, user.transform.position + point.transform.up * _unitDistance);
        }
    }
}