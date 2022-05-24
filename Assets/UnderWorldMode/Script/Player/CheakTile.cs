using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheakTile : MonoBehaviour
{
    private List<IDetectMode> _iDetecetMode = new List<IDetectMode>();

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
