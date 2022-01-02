using System.Collections;
using UnityEngine;
using Trident;
using Zenject;
public class InstateTrident : MonoBehaviour
{
    [SerializeField] private int[] _angel;
    [SerializeField] private float _delay;
    [SerializeField] private InstatePlit _instate;
    [SerializeField] private TridentSetting _trident;
    [SerializeField] private DeadLineHolder _holder;

    private IGameRandom _random;
    private UnderWorldMode _curretMode;

    private void Awake()
    {
        _random = new NotRepeatsRandom();
    }
    private void OnEnable()
    {
        _instate.ModeActiveAction += UpdateMode;
    }
    private void OnDisable()
    {
        _instate.ModeActiveAction -= UpdateMode;
    }
    private void Start()
    {
        StartCoroutine(InstateTrap());
    }
    private void UpdateMode(UnderWorldMode mode)
    {
        switch (mode)
        {
            case UnderWorldMode.OneFire:
                _curretMode = UnderWorldMode.OneFire;
                return;
            case UnderWorldMode.MapFIre:
                _curretMode = UnderWorldMode.MapFIre;
                return;
        }
    }
    private IEnumerator InstateTrap()
    {
        while (true)
        {
            yield return new WaitUntil(() => (_curretMode == UnderWorldMode.OneFire));
            foreach (var angel in _angel)
            {
                _holder.InstateTrident(angel, _trident);
            }
            yield return new WaitForSeconds(_delay);
        }
    }
}
