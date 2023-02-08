using System.Collections.Generic;
using UnityEngine;

public class JetsController : MonoBehaviour
{
    [SerializeField] private List<IzolatorJet> _jets;

    public void Play()
    {
        foreach (var jet in _jets)
        {
            jet.Play();
        }
    }
    public void Stop()
    {
        foreach (var jet in _jets)
        {
            jet.Stop();
        }
    }
}
