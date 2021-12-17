using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TermTile : MonoBehaviour
{
    [SerializeField] private float _warningTime;
    [SerializeField] private float _timeToDestroy;
    [SerializeField] private Animator _animator;

    private Collider2D _coliider;

    private void Awake()
    {
        _coliider = GetComponent<Collider2D>();
        _coliider.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(WarningAnimation());
    }
    private IEnumerator WarningAnimation()
    {
        yield return new WaitForSeconds(_warningTime);
        _coliider.enabled = true;
        _animator.SetInteger("State", 1);
        Destroy(gameObject, _timeToDestroy);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
            player.Term();
    }
   
}
