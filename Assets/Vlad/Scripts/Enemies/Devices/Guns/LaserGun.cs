using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    [SerializeField]
    private Laser _laser;
    protected override IEnumerator Rotate()
    {
        _animator.SetTrigger("Activate");
        yield return new WaitForSeconds(_activationTime);
        _laser.gameObject.SetActive(true);

        float startAngle = _rigidbody.rotation;
        for (float f = 0; f < _activeTime; f += Time.deltaTime)
        {
            _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, _rigidbody.rotation + _rotationAngleOnSeconds, Time.deltaTime);
            yield return null;
        }

        _animator.SetTrigger("Deactivate");
        _laser.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //for (float f = 0; f < _activationTime; f += Time.deltaTime)
        //{
        //    _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, startAngle, Time.deltaTime);
        //    yield return null;
        //}
        _isActive = false;
    }
}
