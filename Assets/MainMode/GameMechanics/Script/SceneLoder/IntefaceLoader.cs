using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    public class IntefaceLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private UserInterfaceType _startInterface;
        [Header("Requred Perfab")]
        [SerializeField] private UserInterface[] _interfacesPerfab;
        [SerializeField] private List<Receiver> _receivers;

        private InterfaceSwitcher _holder = null;

        public IReadOnlyList<Receiver> Receivers => _receivers;

        private void Awake()
        {
            if (!_playOnAwake)
                return;
            LaodInteface();
        }
        public InterfaceSwitcher LaodInteface()
        {
            if (_holder != null)
                return _holder;
            var holder = new GameObject("UserInterface");
            holder.transform.position = Vector3.zero;
            var list = new List<UserInterface>();
            foreach (var perfab in _interfacesPerfab)
            {
                var instantiateObject = Instantiate(perfab.gameObject);
                instantiateObject.transform.parent = holder.transform;
                list.Add(instantiateObject.GetComponent<UserInterface>());
            }
            _holder = holder.AddComponent<InterfaceSwitcher>();
            _holder.Intializate(list.ToArray(), _startInterface);
#if UNITY_ANDROID
            LoadElement(_startInterface, _joystickPerfab.gameObject);
#endif
            return _holder;
        }
    }
}