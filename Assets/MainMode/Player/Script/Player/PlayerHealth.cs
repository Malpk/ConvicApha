using UnityEngine;

[System.Serializable]
public class PlayerHealth
{
    [SerializeField] private int _healthPoints = 5;
    [SerializeField] private HealthUI _display;

    private bool _isStart;

    public int Health => _healthPoints;
    public int MaxHealth { get; private set; }
    public bool IsLoadDisplay => _display;


    public void Intializate()
    {
        if (_display && !_isStart)
        {
            _isStart = true;
            _display.SetupHelth(_healthPoints);
            MaxHealth = _healthPoints;
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
        if (newHealthPoints > MaxHealth)
            _healthPoints = MaxHealth;
        else
            _healthPoints = newHealthPoints;
        if(_display)
            _display.Display(_healthPoints);
    }

}
