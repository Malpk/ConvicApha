using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Timer : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;

    public abstract int TimeValue { get; }

    private void Start()
    {
        StartCoroutine(RunTimer());
    }

    protected abstract IEnumerator RunTimer();
}
