using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    public class IntefaceLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private UserInterfaceType _startInterface;
        [Header("Requred Perfab")]
        [AssetReferenceUILabelRestriction("userInterface")]
        [SerializeField] private AssetReferenceGameObject[] _interfacesPerfab;
        [AssetReferenceUILabelRestriction("reciversUI")]
        [SerializeField] private AssetReferenceGameObject[] _reciversAssets;

        private InterfaceSwitcher _holder = null;
        private List<System.Exception> _exceptions = new List<System.Exception>();
        private List<Receiver> _receiver = new List<Receiver>();

        public IReadOnlyList<Receiver> Receivers => _receiver;

        private async void Awake()
        {
            if (_playOnAwake)
               await LoadIntefaceAsync();
        }
        #region Load Interface
        public async Task<InterfaceSwitcher> LoadIntefaceAsync()
        {
            if (_holder != null)
                return _holder;
            var holder = new GameObject("UserInterface");
            holder.transform.position = Vector3.zero;
            var tasks = new List<Task<GameObject>>();
            foreach (var perfab in _interfacesPerfab)
            {
                tasks.Add(perfab.InstantiateAsync().Task);
            }
            await Task.WhenAll(tasks);
            _holder = holder.AddComponent<InterfaceSwitcher>();
            _holder.Intializate(GetIntefaces(tasks, _holder.transform), _startInterface);
            return _holder;
        }

        private UserInterface[] GetIntefaces(List<Task<GameObject>> tasks, Transform holder)
        {
            _exceptions.Clear();
            var list = new List<UserInterface>();
            foreach (var task in tasks)
            {
                task.Result.transform.parent = holder.transform;
                try
                {
                    if (task.Result.TryGetComponent(out UserInterface UI))
                        list.Add(UI);
                    else
                        throw new System.NullReferenceException("gameobject is not UserInterface component");
                }
                catch (System.Exception ex)
                {
                    _exceptions.Add(ex);
                }

            }
            return list.ToArray();
        }
        #endregion
        #region Load Receiver
        public async Task LoadReceiverAsync()
        {
            var tasks = new List<Task<GameObject>>();
            foreach (var reference in _reciversAssets)
            {
                tasks.Add(reference.LoadAssetAsync().Task);
            }
            await Task.WhenAll(tasks);
            _receiver = GetRecivers(tasks);
        }
        public void UnloadReceiver()
        {
            _receiver.Clear();
            foreach (var asset in _reciversAssets)
            {
                asset.ReleaseAsset();
            }
        }
        private List<Receiver> GetRecivers(List<Task<GameObject>> tasks)
        {
            var list = new List<Receiver>();
            for (int i = 0; i < tasks.Count; i++)
            {
                try
                {
                    if (tasks[i].Result.TryGetComponent(out Receiver reciver))
                    {
                        list.Add(reciver);
                    }
                    else
                    {
                        throw new System.NullReferenceException("GameObjects is not component Receiver");
                    }
                }
                catch (System.Exception ex)
                {
                    _exceptions.Add(ex);
                }
            }
            return list;
        }
        #endregion


    }
}