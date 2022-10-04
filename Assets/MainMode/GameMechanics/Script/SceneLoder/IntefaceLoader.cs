using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    public class IntefaceLoader : MonoBehaviour, ILoader
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private UserInterfaceType _startInterface;
        [Header("Requred Perfab")]
        [AssetReferenceUILabelRestriction("userInterface")]
        [SerializeField] private AssetReferenceGameObject[] _interfacesPerfab;

        public bool IsLoad { get; private set; }
        public HUDInteface Hud { get; private set; }
        public DeadMenu DeadMenu { get; private set; }
        public MarkerUI Marker { get; private set; }
        public InterfaceSwitcher Holder { get; private set; }

        private async void Awake()
        {
            if (_playOnAwake)
               await LoadAsync();
        }
        #region Load Interface
        public async Task LoadAsync()
        {
            if (!IsLoad)
            {
                IsLoad = true;
                var tasks = new List<Task<GameObject>>();
                Holder = new GameObject("UserInterface").AddComponent<InterfaceSwitcher>();
                Holder.transform.position = Vector3.zero;
                foreach (var perfab in _interfacesPerfab)
                {
                    tasks.Add(perfab.InstantiateAsync().Task);
                }
                await Task.WhenAll(tasks);
                Holder.Intializate(GetIntefaces(tasks, Holder.transform), _startInterface);
                Hud = Holder.GetComponentInChildren<HUDInteface>();
                DeadMenu = Holder.GetComponentInChildren<DeadMenu>();
                Marker = Hud.GetComponentInChildren<MarkerUI>();
            }
        }

        public void Unload()
        {
            if (IsLoad)
            {
                IsLoad = false;
                var interfaces = Holder.GetComponentsInChildren<UserInterface>();
                foreach (var unload in interfaces)
                {
                    Addressables.ReleaseInstance(unload.gameObject);
                }
                Destroy(Holder.gameObject);
            }
        }
        public void Intializate(Player player)
        {
            var senders = player.GetComponents<ISender>();
            for (int i = 0; i < senders.Length; i++)
            {
                Hud.GetReceiver(senders[i]);
            }
        }
        private UserInterface[] GetIntefaces(List<Task<GameObject>> tasks, Transform holder)
        {
            var list = new List<UserInterface>();
            foreach (var task in tasks)
            {
                if (task.Result.TryGetComponent(out UserInterface ui))
                {
                    list.Add(ui);
                    ui.transform.parent = holder;
                }
            }
            return list.ToArray();
        }
        #endregion
    }
}