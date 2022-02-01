using System.Collections;
using UnityEngine;
using Trident;
using Zenject;

public class InstateTrident : MonoBehaviour
{
    [SerializeField] private int[] _angel;
    [SerializeField] private float _delay;
    [SerializeField] private InstateTileInRadius _instate;
    [SerializeField] private TridentSetting _trident;
    [SerializeField] private DeadLineHolder _holder;

    private IGameRandom _random;

    private void Awake()
    {
        _random = new NotRepeatsRandom();
    }
    private void Start()
    {
        StartCoroutine(InstateTrap());
    }

    private IEnumerator InstateTrap()
    {
        while (true)
        {
            foreach (var angel in _angel)
            {
                _holder.InstateTrident(angel, _trident);
            }
            yield return new WaitForSeconds(_delay);
        }
    }
}
