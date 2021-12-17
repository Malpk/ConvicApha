using System.Collections;
using UnityEngine;
using Zenject;
public class InstateTrident : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _trap;

    [SerializeField] private InstatePlit _instate;

    private int []  _direction = new int [] { 1,-1 };
    private DeadLinePoint[] _point;
    private IGameRandom _random;
    private UnderWorldMode _curretMode;

    private void Awake()
    {
        _random = new NotRepeatsRandom();
        _point = GetComponentsInChildren<DeadLinePoint>();
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
            foreach (var direction in _direction)
            {
                ChoosePoint(direction);
            }
            yield return new WaitForSeconds(_delay);
        }
    }
    private void ChoosePoint(int direction)
    {
        int id = _random.GetValue(0, _point.Length);
        if (!_point[id].IsStart)
            _point[id].InstateTrap(_trap, direction);
        else
            ChoosePoint(direction);
    }
}
