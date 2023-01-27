using UnityEngine;

public class LaserV2 : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _effect;
    [SerializeField] private Collider2D _collider;

    public void Play()
    {
        SetMode(true);
    }

    public void Stop()
    {
        SetMode(false);
    }

    public void SetMode(bool mode)
    {
        _effect.gameObject.SetActive(mode);
        _animator.SetBool("mode", mode);
        _collider.enabled = mode;
    }
}
