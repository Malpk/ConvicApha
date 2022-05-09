using System.Collections;
using UnityEngine;

public class RocketLauncher : Gun
{
    protected override IEnumerator Rotate()
    {
        float startAngle = _rigidbody.rotation;
        for (float f = 0; f < _activationTime; f += Time.deltaTime)
        {
            Vector2 toTarget = transform.position - _effectsHandler.transform.position;
            _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(toTarget, Vector3.forward), Time.deltaTime * _rotationAngleOnSeconds));
            yield return null;
        }
        Shoot();
        yield return new WaitForSeconds(0.5f);
        for (float f = 0; f < _activationTime; f += Time.deltaTime)
        {
            _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, startAngle, Time.deltaTime);
            yield return null;
        }
        _rigidbody.rotation = startAngle;
        _isActive = false;
    }
}
