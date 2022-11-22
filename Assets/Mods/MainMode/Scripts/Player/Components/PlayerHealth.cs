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
    public bool Heal(int value)
    {
        if (curretHealth == FullHealth)
            return false;
        var newHealthPoints = curretHealth + value;
        curretHealth = newHealthPoints > FullHealth ? FullHealth : newHealthPoints;
        return true;
    }
}
