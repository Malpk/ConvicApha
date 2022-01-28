using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Underworld;
using TileSpace;

public class Vise : MonoBehaviour
{
    [Header("Game Setting")]

    [Header("Perfab Setting")]
    [SerializeField] private GameObject _tile;

    private int _length;
    private float _fireTime;
    private float _warnigTime;
    private float _offset;
    private Vector3 _direction;

    private Vector3 StartPostion => _direction * (_length / 2 - 0.5f) * _offset;
    private List<TernForVise> _terms;

    public void Constructor(int length,float offset, Vector3 direction, float fireTime = 1, float warningTime = 1f)
    {
        if (length % 2 != 0)
            length += 1;
        _length = length;
        _offset = offset;
        _fireTime = fireTime;
        _direction = direction;
        _warnigTime = warningTime;
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        _terms = CreateLineTils();
        transform.position = StartPostion;
        for (int i = 0; i < _length-1; i++)
        {
            yield return new WaitForSeconds(_warnigTime);
            yield return StartCoroutine(TurnOnFire());
            yield return new WaitForSeconds(_fireTime);
            yield return StartCoroutine(TurnOffFire());
            transform.position -= _direction * _offset;
        }
        yield return StartCoroutine(TurnOffTile());
        Destroy(gameObject);
    }
    private IEnumerator TurnOffTile()
    {
        TernForVise lostTerm  = null;
        foreach (var term in _terms)
        {
            lostTerm = term;
            term.TurnOffTile();
        }
        yield return new WaitWhile(() => lostTerm != null);
    }
    private List<TernForVise> CreateLineTils()
    {
        var list = new List<TernForVise>();
        var instatePosition = StartPostion;
        for (int i = 0; i < _length; i++)
        {
            var instate = Instantiate(_tile, instatePosition, Quaternion.identity);
            instate.transform.parent = transform;
            if (instate.TryGetComponent<TernForVise>(out TernForVise term))
                list.Add(term);
            instatePosition -= _direction * _offset;
        }
        transform.rotation *= Quaternion.Euler(Vector3.forward * 90);
        return list;
    }
    private IEnumerator TurnOffFire()
    {
        TernForVise lost = null;
        foreach (var term in _terms)
        {
            lost = term;
            term.DeactiveFire();
        }
        yield return new WaitWhile(() => lost.state != TernState.Deactive);
    }
    private IEnumerator TurnOnFire()
    {
        foreach (var term in _terms)
        {
            term.ActiveFire();
        }
        yield return null;
    }
}
