using UnityEngine;

namespace MainMode.Items
{
    public class MouseExplosion : Item
    {
        [SerializeField] private float _timeDestroy;
        [SerializeField] private MouseExplosionProjectale _projectale;

        public override string Name => "Взрывная мышь";

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
            var projectale = Instantiate(_projectale.gameObject, user.transform.position, user.transform.rotation).
                GetComponent<MouseExplosionProjectale>();
            projectale.Shoot(_timeDestroy);
        }
    }
}