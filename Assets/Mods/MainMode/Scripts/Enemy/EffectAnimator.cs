using UnityEngine;

public class EffectAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public System.Action OnExplosion;

    public void Effect(int state)
    {
        _animator.SetInteger("State", state);
    }

    private void HideAnimationEvent()
    {
        OnExplosion?.Invoke();
    }
}
