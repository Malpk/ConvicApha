using UnityEngine;

namespace MainMode.Items
{
    public class Balonchik : Artifact
    {
        [SerializeField] private float _timeActive;
        [SerializeField] private AirJet _jetAir;

        public override string Name => "Болончик";

        protected void Awake()
        {
            _jetAir.gameObject.SetActive(false);
            _jetAir.Intializate(_timeActive);
        }

        protected override void UseArtifact()
        {
            _jetAir.gameObject.SetActive(true);
            _jetAir.transform.position = user.transform.position;
            _jetAir.transform.rotation = user.transform.rotation;
            user.transform.parent = _jetAir.transform;
            user.EnterToTransport(_jetAir);
        }
    }
}