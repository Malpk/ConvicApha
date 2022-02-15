using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Timer : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;

    public abstract int TimeValue { get; }

    protected abstract IEnumerator RunTimer();
}
