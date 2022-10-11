using System;
using UnityEngine;
using MainMode.GameInteface;

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
        private PlayerConfig _config;
        private IntefaceLoader _intefaceLoader;

        protected Player player;
        protected DeadMenu deadMenu;
        protected PlayerLoader playerLoader;
        protected InterfaceSwitcher holder;

        protected event Action LoadAction;
        protected event Action UnloadAction;
        protected event Action PlayAction;
        protected event Action StopGameAction;
        protected event Action BackMainMenuAction;

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

        private void Start()
        {
            if (playOnStart)
                Load(choosePlayer);
        }
        #region LaodScene
        public void Load(PlayerConfig config)
        {
            if (IsReadyLoad)
            {
                IsReadyLoad = true;
                _intefaceLoader.LoadInteface();
                holder = _intefaceLoader.Holder;
                _marker = _intefaceLoader.HUD.GetComponentInChildren<MarkerUI>();
                deadMenu = _intefaceLoader.DeadMenu;
                deadMenu.Intializate(this);
                if (LoadAction != null)
                    LoadAction();
            }
            playerLoader.PlayerLoad(spwanPoint, config);
            _config = config;
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
            cameraFollowing.Play();
        }
        #endregion
        #region Conrollers
        public void Play()
        {
            if (PlayAction != null)
                PlayAction();
            _marker.Play();
            if (!player.IsPlay)
                player.Play();
            cameraFollowing.Play();
            holder.SetShow(_intefaceLoader.HUD);
            if (player.TryGetComponent(out IReset restart))
                restart.Restart(_config);
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
            cameraFollowing.Stop();
            cameraFollowing.transform.position = new Vector3(spwanPoint.position.x, 
                spwanPoint.position.y, cameraFollowing.transform.position.z); 
            holder.SetShow(deadMenu);
            if (StopGameAction != null)
                StopGameAction();
        }
    }
}