using UnityEngine;

[System.Serializable]
public class PlayerHealth
{
    [SerializeField] private int _healthPoints = 5;
    [SerializeField] private int _maxHealthPoint = 7;
    [SerializeField] private HealthUI _display;

    private bool _isStart;

    public int Health => _healthPoints;
    public int MaxHealth => _maxHealthPoint;
    public bool IsLoadDisplay => _display;


    public void Start()
    {
        if (_display && !_isStart)
        {
            _isStart = true;
            _display.SetupHelth(_healthPoints);
        }
    }
    public bool SetReceiver(HealthUI display)
    {
        if (_display != null)
            return false;
        _display = display;
        return true;
    }

    public void SetDamage(int damage)
    {
        _healthPoints -= damage;
        _display.Display(_healthPoints);
    }
    public void Heal(int value)
    {
        var newHealthPoints = _healthPoints + value;
        if (newHealthPoints > _maxHealthPoint)
            _healthPoints = _maxHealthPoint;
        else
            _healthPoints = newHealthPoints;
        if(_display)
            _display.Display(_healthPoints);
    }

}
