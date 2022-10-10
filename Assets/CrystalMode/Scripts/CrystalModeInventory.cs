using System;
using System.Collections;
using System.Collections.Generic;
using MainMode;
using MainMode.Items;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class CrystalModeInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ConsumablesItem _crystalDust;
    [SerializeField]private InventoryView _inventoryView;
    [SerializeField] private int startDustCount;
    [SerializeField] private Sprite crystalDustSprite;
    private NavMeshSurface2d navMeshSurface2d;

    private void Start()
    {
        navMeshSurface2d = GameObject.FindWithTag("NavMesh").GetComponent<NavMeshSurface2d>();
        navMeshSurface2d.BuildNavMesh();
        for (int i = 0; i < startDustCount; i++)
        {
            _inventory.AddConsumablesItem(_crystalDust);
        }

        _inventoryView.Display(crystalDustSprite, startDustCount);
    }
}
