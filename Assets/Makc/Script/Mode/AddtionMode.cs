using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddtionMode : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private float _startDelay;
    [Header("Perfab Setting")] 
    [SerializeField]private GameObject _tridentMode;

    void Start()
    {
        StartCoroutine(RunMode());
    }

    private IEnumerator RunMode()
    {
        yield return new WaitForSeconds(_startDelay);
        yield return null;
    }
}
