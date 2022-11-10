using UnityEngine;

[System.Serializable]
public class PlayerHealth
{
    [Min(1)]
    [SerializeField] private int _health = 5;

    private int curretHealth = 0;

    public int Health => curretHealth;
    public int FullHealth => _health;

    public void Reset()
    {
        curretHealth = _health;
    }

    public void SetDamage(int damage)
    {
        curretHealth = Mathf.Clamp(curretHealth - damage, 0, FullHealth);
    }
    public void Heal(int value)
    {
        var newHealthPoints = curretHealth + value;
        if (newHealthPoints > FullHealth)
            curretHealth = FullHealth;
        else
            curretHealth = newHealthPoints;
    }
}
