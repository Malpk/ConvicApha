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

    private Collider2D _coliider;

    private void Awake()
    {
        _coliider = GetComponent<Collider2D>();
        //_coliider.enabled = false;
    }
    private void Start()
    {
        //StartCoroutine(WarningAnimation());
    }
    private IEnumerator WarningAnimation()
    {
        yield return new WaitForSeconds(_warningTime);
        _coliider.enabled = true;
        _animator.SetInteger("State", 1);
        var fire = Instantiate(_fire, transform);
        fire.transform.parent = transform;
        fire.transform.localPosition = _firePosition;
        yield return new WaitUntil(() => (fire == null));
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
            player.Term();
        var cleaner = collision.GetComponent<Cleaner>();
        if (cleaner != null)
            GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var cleaner = collision.GetComponent<Cleaner>();
        if (cleaner != null)
            GetComponent<SpriteRenderer>().enabled = true;
    }
}
