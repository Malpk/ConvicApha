using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject HealthIconPrefab;
    public List<GameObject> HealthIcons;
    public void Setup(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newIcon = Instantiate(HealthIconPrefab, transform);
            HealthIcons.Add(newIcon);
        }
    }

    public void DisplayHealth(int health)
    {
        for (int i = 0; i < HealthIcons.Count; i++)
        {
            HealthIcons[i].SetActive(i < health);
        }
    }
}
