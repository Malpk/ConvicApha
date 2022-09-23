using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace MainMode.Mode1921
{
    public abstract class GeneralSpawner : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnStart;
        [Header("General Reference")]
        [SerializeField] protected MapGrid mapGrid;

        protected event System.Action PlayAction;
        protected event System.Action StopAction;

        private List<string> _failKey = new List<string>();

        public bool IsPaly { private set; get; }
        public abstract bool IsRedy { get; }

        public void Intializate(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
        }

        protected async Task<List<T>> LoadAssetsAsync<T>(string[] loadKeys) where T : SmartItem
        {
            var assets = new List<T>();
            var tasks = new List<Task<GameObject>>();
            for (int i = 0; i < loadKeys.Length; i++)
            {
                tasks.Add(Addressables.LoadAssetAsync<GameObject>(loadKeys[i]).Task);
            }
            await Task.WhenAll(tasks);
            for (int i = 0; i < tasks.Count; i++)
            {
                try
                {
                    if (tasks[i].Result.TryGetComponent(out T item))
                        assets.Add(item);
                    else
                        throw new System.Exception();
                }
                catch
                {
                    _failKey.Add(loadKeys[i]);
                }
            }
            return assets;
        }
        protected async Task<T> LoadAssetAsync<T>(string loadKey) where T : SmartItem
        {
            var task = Addressables.LoadAssetAsync<GameObject>(loadKey).Task;
            await task;
            if (task.Result.TryGetComponent(out T asset))
            {
                return asset;
            }
            else
                throw new System.NullReferenceException($"Gameobject is not contain component SmartItem");

        }

        private void Start()
        {
            if (_playOnStart)
                StartCoroutine(PlayToReady());
        }

        private IEnumerator PlayToReady()
        {
            yield return new WaitWhile(() => !IsRedy);
            Play();
        }

        public void Play()
        {
#if UNITY_EDITOR
            if (IsPaly)
                throw new System.Exception("Spawner is already play");
            if (!IsRedy)
                throw new System.Exception("Spawner is not ready to play");
#endif
            IsPaly = true;
            if (PlayAction != null)
                PlayAction();
        }
        public void Stop()
        {
#if UNITY_EDITOR
            if (!IsPaly)
                throw new System.Exception("Spawner is already stop");
#endif
            IsPaly = false;
            if (StopAction != null)
                StopAction();
        }

        public abstract void Replay();

    }
}