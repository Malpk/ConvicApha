using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _bodyEffect;

    public event System.Action OnHit;

    public void Hit()
    {
        _bodyEffect.SetTrigger("Hit");
    }

    private void HitAnimationEvent()
    {
        OnHit?.Invoke();
    }
}
