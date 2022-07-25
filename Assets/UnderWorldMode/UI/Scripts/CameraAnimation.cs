using System.Collections;
using UnityEngine;
using Underworld;

[RequireComponent(typeof(Animator))]
public class CameraAnimation : MonoBehaviour
{
    [Header("Perfab Setting")]

    private Animator _animator;

    public Vector3 cameraPosition => transform.position;

    public delegate void Complite();
    public Complite CompliteAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Deactive()
    {
        if (CompliteAction != null)
            CompliteAction();
        Move(false);
        enabled = false;
    }
    private void Move(bool value)
    {
        _animator.SetBool("Move", value);
    }
}
