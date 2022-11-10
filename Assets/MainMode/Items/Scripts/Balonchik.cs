using UnityEngine;

namespace MainMode.Items
{
    public class Balonchik : Item
    {
        [SerializeField] private float _timeActive;
        [SerializeField] private AirJet _jetAir;

        public override string Name => "Болончик";

        protected override void Awake()
        {
            base.Awake();
            _jetAir.gameObject.SetActive(false);
            _jetAir.Intializate(_timeActive);
        }

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
            _jetAir.gameObject.SetActive(true);
        }
    }
}