using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSetting : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TypeTile _type;
    
    public GameObject Panel => _panel;
    public TypeTile Type => _type;
}
