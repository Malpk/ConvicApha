using MainMode;
using MainMode.Items;
using UnityEngine;
using UnityEngine.AI;
using PlayerComponent;

public class CrystalModeInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private ConsumablesItem _crystalDust;
    [SerializeField] private int startDustCount;
    private NavMeshSurface2d navMeshSurface2d;

    private void Start()
    {
        navMeshSurface2d = GameObject.FindWithTag("NavMesh").GetComponent<NavMeshSurface2d>();
        navMeshSurface2d.BuildNavMesh();
        AddDustToInventory(startDustCount);
    }
    public void AddDustToInventory(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _inventory.AddConsumablesItem(_crystalDust);
        }
    }
}
