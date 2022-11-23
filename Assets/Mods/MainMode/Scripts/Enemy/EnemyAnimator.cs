using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _effect;
    [SerializeField] private Animator _bodyEffect;

    public event System.Action OnHit;

    public void Hit()
    {
        _bodyEffect.SetTrigger("Hit");
    }

    public void Effect(int state)
    {
        _effect.SetInteger("State", state);
    }

    private void HitAnimationEvent()
    {
        OnHit?.Invoke();
    }
}
