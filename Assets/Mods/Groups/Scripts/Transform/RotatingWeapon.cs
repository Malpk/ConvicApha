using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [Header("Reference")]   
    [SerializeField] protected Rigidbody2D _rigidbody;

    private int[] _directons = new int[] { -1, 1 };

    private void FixedUpdate()
    {
        _rigidbody.MoveRotation(_rigidbody.rotation + _rotateSpeed * Time.fixedDeltaTime);
    }

    public void Play()
    {
        enabled = true;
        _rotateSpeed = Mathf.Abs(_rotateSpeed) * _directons[Random.Range(0,_directons.Length)];
    }

    public void Stop()
    {
        enabled = false;
    }
}
