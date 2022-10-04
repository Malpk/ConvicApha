using UnityEngine;
using PlayerComponent;
using MainMode.GameInteface;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace MainMode.LoadScene
{
    [RequireComponent(typeof(PlayerLoader), typeof(IntefaceLoader))]
    public class BaseLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [SerializeField] protected bool autoRestart;
        [Header("Player Load Setting")]
        [SerializeField] protected MainMenu mainMenu;
        [SerializeField] protected Transform spwanPoint;
        [SerializeField] protected PlayerConfig choosePlayer;
        [SerializeField] protected CameraFollowing cameraFollowing;

        private MarkerUI _marker;
        private IntefaceLoader _intefaceLoader;

        protected Player player;
        protected DeadMenu deadMenu;
        protected PlayerLoader playerLoader;
        protected InterfaceSwitcher holder;

        protected event System.Action LoadAction;
        protected event System.Action UnloadAction;
        protected event System.Action PlayAction;
        protected event System.Action StopGameAction;
        protected event System.Action BackMainMenuAction;

        public bool IsReadyLoad { get; private set; }

        private void Awake()
        {
            playerLoader = GetComponent<PlayerLoader>();
            _intefaceLoader = GetComponent<IntefaceLoader>();
        }

        protected virtual void OnEnable()
        {
            IsReadyLoad = true;
            playerLoader.PlayerLoadAction += Intializate;
        }

        protected virtual void OnDisable()
        {
            IsReadyLoad = false;
            playerLoader.PlayerLoadAction -= Intializate;
        }

        private async void Start()
        {
            if (playOnStart)
            {
                await LoadAsync(choosePlayer);
            }
        }
        #region LaodScene
        public async Task LoadAsync(PlayerConfig config)
        {
            if (IsReadyLoad)
            {
                if(!_intefaceLoader.IsLoad)
                    await _intefaceLoader.LoadAsync();
                _marker = _intefaceLoader.Marker;
                holder = _intefaceLoader.Holder;
                deadMenu = _intefaceLoader.DeadMenu;
                deadMenu.Intializate(this);
                await playerLoader.PlayerLaodAsync(spwanPoint, config);
                if (LoadAction != null)
                    LoadAction();
            }
        }
        public void Unload()
        {
            playerLoader.UnLoadPLayer();
            if (UnloadAction != null)
                UnloadAction();
            player.DeadAction -= StopGame;
        }
        
        #endregion
        #region Intilizate
        private void Intializate(Player player)
        {
            if(this.player)
                this.player.DeadAction -= StopGame;
            this.player = player;
            this.player.DeadAction += StopGame;
            _intefaceLoader.Intializate(player);
            IntilizatePlayer(_marker);
        }

        private void IntilizatePlayer(MarkerUI marker)
        {
            marker.Intilizate(player, cameraFollowing);
            player.SetMarker(marker);
            cameraFollowing.SetTarget(player);
        }
        #endregion
        #region Conrollers
        public void Play()
        {
            if (PlayAction != null)
                PlayAction();
            _marker.Play();
            if (player.IsDead)
                player.Respawn();
            holder.SetShow(_intefaceLoader.Hud);
            if (player.TryGetComponent(out IReset restart))
                restart.Restart();
        }
        public void BackMainMenu()
        {
            mainMenu.Show();
            deadMenu.Hide();
            if (BackMainMenuAction != null)
                BackMainMenuAction();
        }
        #endregion
        private void StopGame()
        {
            _marker.Stop();
            cameraFollowing.transform.position = new Vector3(spwanPoint.position.x, 
                spwanPoint.position.y, cameraFollowing.transform.position.z); 
            holder.SetShow(deadMenu);
            if (StopGameAction != null)
                StopGameAction();
        }
    }
}