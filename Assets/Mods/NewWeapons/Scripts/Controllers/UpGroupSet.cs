using UnityEngine;

namespace MainMode
{
    public class UpGroupSet : MonoBehaviour
    {
        [SerializeField] private bool _showInStart;
        [Header("Reference")]
        [SerializeField] private Animator _animator;
        [SerializeField] private GroupController _group;

        private void Awake()
        {
            _group.gameObject.SetActive(false);
        }

        private void Start()
        {
            if (_showInStart)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            _animator.SetBool("show", true);
        }

        public void Hide()
        {
            _animator.SetBool("show", false);
        }

        private void ShowAnimationEvent()
        {
            _group.gameObject.SetActive(true);
        }

        private void StartAnimationEvent()
        {
            _group.Play();
        }

        private void HideAnimationEvent()
        {
            _group.gameObject.SetActive(false);
        }

    }
}