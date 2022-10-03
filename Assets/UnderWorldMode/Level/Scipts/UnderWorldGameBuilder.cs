using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class UnderWorldGameBuilder : MonoBehaviour,IPause
    {
        [Header("General Mode")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _useDefoutSetting;
        [Min(0)]
        [SerializeField] private float _switchDelay = 2f;
        [Header("Reference")]
        [SerializeField] private SwitchPatern _switchPatern;
        [SerializeField] private List<PaternSequnce> _paternsSequnce;

        private List<PaternSequnce> _usedSequnce = new List<PaternSequnce>();

        public bool IsPlay { get; private set; } = false;
        public bool IsPause { get; private set; } = false;

        public void Intilizate(Player player)
        {
            _switchPatern.Intializate(player);
        }

        private void Start()
        {
            if (_playOnStart)
                Play();
        }
        #region Controlers
        public void Play()
        {
#if UNITY_EDITOR
            if (IsPlay)
                throw new System.Exception("UnderorldGameBuilder is already play");
#endif
            if (!IsPlay)
            {
                IsPlay = true;
                StartCoroutine(Build());
            }
        }
        public void Stop()
        {
#if UNITY_EDITOR
            if (!IsPlay)
                throw new System.Exception("UnderorldGameBuilder is already stop");
#endif
            IsPlay = false;
            IsPause = false;
            _switchPatern.Deactivate();
        }

        public void Pause()
        {
            IsPause = true;
            _switchPatern.Pause();
        }

        public void UnPause()
        {
            IsPause = false;
            _switchPatern.UnPause();
        }
        #endregion

        private IEnumerator Build()
        {
            PaternConfig lastConfig = null;
            yield return WaitTime(1f);
            while (IsPlay)
            {
                CheakCountSequence();
                var sequence = GetSequence(lastConfig);
                _usedSequnce.Add(sequence);
                _paternsSequnce.Remove(sequence);
                for (int i = 0; i < sequence.Configs.Length && IsPlay; i++)
                {
                    yield return LaucnhPatern(sequence.Configs[i]);
                    yield return WaitTime(_switchDelay);
                    lastConfig = sequence.Configs[i];
                }
            }
        }
        private IEnumerator LaucnhPatern(PaternConfig config)
        {
            var task = _useDefoutSetting ?
                _switchPatern.ActivateModeAsync(config.TypeMode) : 
                _switchPatern.ActivateModeAsync(config.TypeMode, config);
            yield return new WaitWhile(() => task.IsCompleted && IsPlay);
            yield return new WaitWhile(() => !_switchPatern.IsReady);
        }
        private PaternSequnce GetSequence(PaternConfig previus)
        {
            if (previus)
            {
                var list = new List<PaternSequnce>();
                for (int i = 0; i < _paternsSequnce.Count; i++)
                {
                    if (_paternsSequnce[i].Configs[0].TypeMode != previus.TypeMode)
                        list.Add(_paternsSequnce[i]);
                }
                if(list.Count > 0)
                    return list[Random.Range(0, list.Count)];
            }
            return _paternsSequnce[Random.Range(0, _paternsSequnce.Count)];
        }
        private void CheakCountSequence()
        {
            if (_paternsSequnce.Count == 0)
            {
                _paternsSequnce.AddRange(_usedSequnce);
                _usedSequnce.Clear();
            }
        }

        private IEnumerator WaitTime(float time)
        {
            var progress = 0f;
            while (progress < 1f && IsPlay)
            {
                progress += Time.deltaTime / time;
                yield return new WaitWhile(() => IsPause);
                yield return null;
            }
        }
    }
}