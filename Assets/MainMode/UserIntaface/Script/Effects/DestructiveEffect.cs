using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public abstract class DestructiveEffect : MonoBehaviour
{
    [SerializeField]
    protected int _idEffect;
    [SerializeField]
    protected string _nameEffect;
    [SerializeField]
    private float _timeOfAction = 8;
    [SerializeField]
    private Image[] _activateEffectImages;

    public UnityEvent<DestructiveEffect> OnStartEffect;
    public UnityEvent<DestructiveEffect> OnStopEffect;

    private Coroutine _activateEffectCoroutine;

    public int GetID()
    {
        return _idEffect;
    }
    public virtual void StartEffect(Health health, UnitMove unitMove)
    {
        ActivateTheDisplayEffect(_activateEffectImages);
        OnStartEffect.Invoke(this);
    }
    public virtual void StopEffect()
    {
        if (_activateEffectCoroutine != null)
        {
            StopCoroutine(_activateEffectCoroutine);
        }
        OnStopEffect.Invoke(this);
    }
    public void ActivateTheDisplayEffect(Image[] effectImages)
    {
        _activateEffectCoroutine = StartCoroutine(BlinckEffect(_timeOfAction));

    }
    protected IEnumerator BlinckEffect(float timeOfAction)
    {
        foreach (Image image in _activateEffectImages)
        {
            image.enabled = true;
        }

        for (float t = 1; t > 0; t -= Time.deltaTime / timeOfAction)
        {
            foreach (Image image in _activateEffectImages)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, t);
            }
            yield return null;
        }
        foreach (Image image in _activateEffectImages)
        {
            image.enabled = false;
        }
        StopEffect();
    }
}
