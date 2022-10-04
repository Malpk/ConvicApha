using UnityEngine;
using MainMode.LoadScene;

namespace Underworld
{
    public sealed class UnderWorldLoader : BaseLoader
    {
        [SerializeField] private UnderWorldGameBuilder _builder;

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayAction += PlayUnderworld;
            StopGameAction += StopUnderworld;
            playerLoader.PlayerLoadAction += Intializate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayAction -= PlayUnderworld;
            StopGameAction -= StopUnderworld;
            playerLoader.PlayerLoadAction -= Intializate;
        }
        private void Intializate(Player player)
        {
            _builder.Intilizate(player);
        }
        private void PlayUnderworld()
        {
            _builder.Play();
        }
        private void StopUnderworld()
        {
            _builder.Stop();
        }
    }
}