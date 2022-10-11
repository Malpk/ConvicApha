using UnityEngine;

namespace MainMode.Items
{
    public class Balonchik : Item
    {
        [SerializeField] private float _timeActive;
        [SerializeField] private AirJet perfab;

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
            var jet = Instantiate(perfab, user.transform.position, user.transform.rotation).GetComponent<AirJet>();
            user.AddEffects(jet, _timeActive);
            jet.Enter(user.GetComponent<Rigidbody2D>());
        }
    }
}