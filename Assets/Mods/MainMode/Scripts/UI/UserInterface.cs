using UnityEngine;

namespace MainMode.GameInteface
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UserInterface : MonoBehaviour
    {
        public bool IsShow { get; private set; }

        public event System.Action HideAction;
        public event System.Action ShowAction;

        public void Show()
        {
            IsShow = true;
            ShowAction?.Invoke();
        }
        public void Hide()
        {
            IsShow = false;
            HideAction?.Invoke();
        }
    }
}