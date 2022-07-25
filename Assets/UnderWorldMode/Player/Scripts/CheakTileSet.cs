using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Collider2D))]
    public class CheakTileSet : MonoBehaviour, IPause
    {
        private Collider2D _collider;
        private List<IDetectMode> _iDetecetMode = new List<IDetectMode>();

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void Pause()
        {
            if(_collider.enabled)
                _collider.enabled = false;
        }

        public void UnPause()
        {
            if(!_collider.enabled)
                _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDetectMode>(out IDetectMode tile))
            {
                _iDetecetMode.Add(tile);
                tile.SetTrackingMode(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDetectMode>(out IDetectMode tile))
            {
                tile.SetTrackingMode(false);
                _iDetecetMode.Remove(tile);
            }
        }
    }
}
