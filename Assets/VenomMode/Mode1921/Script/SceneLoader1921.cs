using UnityEngine;
using MainMode.LoadScene;

namespace MainMode.Mode1921
{
    public sealed class SceneLoader1921 : BaseLoader
    {
        [SerializeField] private Mode1921 _mode;

        private ToolSet _toolSet;
        private OxyGenSet _oxySet;

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayAction += PlayVenomMode;
            LoadAction += LoadVenomMode;
            playerLoader.PlayerLoadAction += Intializate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayAction -= PlayVenomMode;
            LoadAction -= LoadVenomMode;
            playerLoader.PlayerLoadAction -= Intializate;
        }

        private void Intializate(Player player)
        {
            if (autoRestart)
            {
                if(this.player)
                    this.player.DeadAction -= Play;
            }
            this.player = player;
            if(autoRestart)
                this.player.DeadAction += Play;
        }
        private void LoadVenomMode()
        {
            _mode.Intializate(holder.GetComponentInChildren<ChangeTest>());
            _oxySet = holder.GetComponentInChildren<OxyGenSet>();
            _toolSet = holder.GetComponentInChildren<ToolSet>();
        }
        private void RestartVenomMode()
        {
            _mode.Restart();
        }
        private void PlayVenomMode()
        {
            _mode.Play();
        }
    }
}