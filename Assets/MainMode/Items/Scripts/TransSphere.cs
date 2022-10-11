using UnityEngine;

namespace MainMode.Items
{
    public class TransSphere : ConsumablesItem
    {
        [Header("Setting")]
        [SerializeField] private int _unitDistance;
        [SerializeField] private TransSpherePoint _point;

        protected override void OnEnable()
        {
            base.OnEnable();
            UseAction += Actvate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
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