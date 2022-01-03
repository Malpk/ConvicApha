using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TermTile : MonoBehaviour
{
    [SerializeField] private float _warningTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _fire;
    [SerializeField] private Vector3 _firePosition;

    private GameObject _fireInstiate;
    private TernState _state = TernState.Warning;
    private TernState _lostState;

    private void Start()
    {
        StartCoroutine(WarningAnimation());
    }
    private IEnumerator WarningAnimation()
    {
        yield return new WaitForSeconds(_warningTime);
        _animator.SetInteger("State", 1);
        _fireInstiate = Instantiate(_fire, transform);
        _fireInstiate.transform.parent = transform;
        _fireInstiate.transform.localPosition = _firePosition;
        if (_state == TernState.Deactive)
        {
            SetState(false);
            _lostState = TernState.Fire;
        }
        else
        {
            _state = TernState.Fire;
        }
        yield return new WaitUntil(() => (_fireInstiate == null));
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null && _state == TernState.Fire)
            player.Term();
        var cleaner = collision.GetComponent<Cleaner>();
        if (cleaner != null)
        {
            _lostState = _state;
            _state = TernState.Deactive;
            SetState(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var cleaner = collision.GetComponent<Cleaner>();
        if (cleaner != null)
        {
            _state = _lostState;
            SetState(true);
        }
    }
    private void SetState(bool value)
    {
        if (_fireInstiate != null)
            _fireInstiate.GetComponent<SpriteRenderer>().enabled = value;
        GetComponent<SpriteRenderer>().enabled = value;
    }
}
