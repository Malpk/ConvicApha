using UnityEngine;

namespace MainMode.Items
{
    public class MouseExplosion : Item
    {
        [SerializeField] private float _timeDestroy;
        [SerializeField] private MouseExplosionProjectale _projectale;

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
            var projectale = Instantiate(_projectale.gameObject, user.transform.position, user.transform.rotation).
                GetComponent<MouseExplosionProjectale>();
            projectale.Shoot(_timeDestroy);
        }
    }
}