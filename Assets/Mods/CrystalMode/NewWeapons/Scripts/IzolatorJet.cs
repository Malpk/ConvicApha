using UnityEngine;

public class IzolatorJet : MonoBehaviour
{
    [SerializeField] private Animator _amimator;

    public void Play()
    {
        _amimator.SetBool("mode", true);
    }

    public void Stop()
    {
        _amimator.SetBool("mode", false);
    }
}
