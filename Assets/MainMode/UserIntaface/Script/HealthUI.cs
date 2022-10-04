using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;

public class HealthUI : Receiver
{
    [SerializeField] private List<GameObject> _healthIcons;

    public GameObject HealthIconPrefab;

    public override TypeDisplay DisplayType => TypeDisplay.HealthUI;

    public void SetupHelth(int maxHealth)
    {
        var count = maxHealth - _healthIcons.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                var newIcon = Instantiate(HealthIconPrefab, transform);
                _healthIcons.Add(newIcon);
            }
        }
        foreach (var icon in _healthIcons)
        {
            icon.SetActive(true);
        }
    }
    public void Display(int health)
    {
        for (int i = 0; i < _healthIcons.Count; i++)
        {
            _healthIcons[i].SetActive(i < health);
        }
    }
}
