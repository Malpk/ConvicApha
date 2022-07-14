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
        for (int i = 0; i < maxHealth; i++)
        {
            var newIcon = Instantiate(HealthIconPrefab, transform);
            newIcon.SetActive(true);
            _healthIcons.Add(newIcon);
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
