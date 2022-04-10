using System.Collections;
using UnityEngine;
using UnityEngine.UI;

enum TrapTypes
{
    C14,
    U92,
    N7,
    Ti81
}

public class Trap : Device
{
    [SerializeField]
    private TrapTypes _trapTypes;
    [SerializeField]
    private Image _activateEffectImage;

    private Coroutine _activateEffectCoroutine;

    protected override void ActivateDeviceOnPlayer(PlayerController playerController)
    {
        switch (_trapTypes)
        {
            case TrapTypes.C14:
                break;
            case TrapTypes.U92:
                break;
            case TrapTypes.N7:
                break;
            case TrapTypes.Ti81:
                break;
        }
        _activateEffectCoroutine = StartCoroutine(BlinckEffect());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activateEffectCoroutine != null)
        {
            StopCoroutine(_activateEffectCoroutine);
        }
        base.OnTriggerEnter2D(collision);
    }

    private IEnumerator BlinckEffect()
    {
        _activateEffectImage.enabled = true;
        for (float t = 1; t > 0; t -= Time.deltaTime / _timeOfAction)
        {
            _activateEffectImage.color = new Color(_activateEffectImage.color.r, _activateEffectImage.color.g, _activateEffectImage.color.b, t);
            yield return null;
        }
        _activateEffectImage.enabled = false;
    }
}
