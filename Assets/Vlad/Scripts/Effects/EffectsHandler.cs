using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(UnitMove))]
public class EffectsHandler : MonoBehaviour
{

    private UnitMove _unitMove;
    private Health _health;

    private List<DestructiveEffect> _effectsOnUnit = new List<DestructiveEffect>();

    void Start()
    {
        _unitMove = GetComponent<UnitMove>();
        _health = GetComponent<Health>();
    }

    public void AddEffect(DestructiveEffect effect)
    {

        int indexEffect = 0;
        if(IsFindeEffect(effect, out indexEffect))
        {
            _effectsOnUnit[indexEffect].StopEffect();
        }
        _effectsOnUnit.Add(effect);
        effect.OnStopEffect.AddListener(RemoveEffect);
        effect.StartEffect(_health, _unitMove);
    }

    private bool IsFindeEffect(DestructiveEffect effect, out int indexEffect)
    {
        for (int i = 0; i < _effectsOnUnit.Count; i++)
        {
            if (_effectsOnUnit[i].GetID() == effect.GetID())
            {
                indexEffect = i;
                return true;
            }
        }
        indexEffect = 0;
        return false;
    }

    public void RemoveEffect(DestructiveEffect effect)
    {
        int indexEffect = 0;
        if (IsFindeEffect(effect, out indexEffect))
        {
            _effectsOnUnit.RemoveAt(indexEffect);
        }
        effect.OnStopEffect.RemoveListener(RemoveEffect);
        Destroy(effect.gameObject);
    }
}
