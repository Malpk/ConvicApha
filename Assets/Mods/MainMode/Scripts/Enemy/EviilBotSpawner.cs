using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMode
{
    public class EviilBotSpawner : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [Header("Reference")]
        [SerializeField] private Player _target;
        [SerializeField] private EvilRobot _evilBot;

        private bool _isPlay;

        public bool IsReady { private set; get; }
        
        [Inject]
        public void Construct(Player target)
        {
            _target = target;
        }

        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            _evilBot.SetMode(true);
            ActivateBot();
        }
        public void Stop()
        {
            _evilBot.Explosion();
        }

        private void ActivateBot()
        {
            _evilBot.SetTarget(_target);
            _evilBot.transform.localPosition = Vector2.zero;
            _evilBot.transform.rotation = Quaternion.Euler(Vector3.zero);
            _evilBot.SetMode(true);
            _evilBot.Activate();
        }
    }
}