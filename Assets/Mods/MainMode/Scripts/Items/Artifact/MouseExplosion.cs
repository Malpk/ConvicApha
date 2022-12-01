using UnityEngine;

namespace MainMode.Items
{
    public class MouseExplosion : Artifact
    {
        [SerializeField] private float _timeDestroy;
        [SerializeField] private MouseExplosionProjectale _projectale;

        public override string Name => "Взрывная мышь";

        protected override void UseArtifact()
        {
            var projectale = Instantiate(_projectale.gameObject, user.transform.position, user.transform.rotation).
                 GetComponent<MouseExplosionProjectale>();
            projectale.Shoot(_timeDestroy);
        }
    }
}