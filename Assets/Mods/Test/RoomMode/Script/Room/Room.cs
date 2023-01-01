using UnityEngine;

namespace MainMode.Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private bool _fogOnStart = true;
        [SerializeField] private RoomBehaviour _behaviour;
        [SerializeField] private SpriteRenderer _fogSprite;

        private void Start()
        {
            _fogSprite.enabled = _fogOnStart;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
        if (collision.TryGetComponent(out Player player))
            {
                _fogSprite.enabled = false;
                if(_behaviour)
                    _behaviour.Play();
            }    
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _fogSprite.enabled = true;
                if (_behaviour)
                    _behaviour.Stop();
            }
        }
    }
}