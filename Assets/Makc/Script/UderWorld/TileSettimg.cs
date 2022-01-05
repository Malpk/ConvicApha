using UnityEngine;

[CreateAssetMenu(menuName ="Tile/TileSetting")]
public class TileSettimg : ScriptableObject
{
    [SerializeField] private float _timeActive;
    [SerializeField] private GameObject _tile;

    public float timeActive => _timeActive;
    public GameObject tile => _tile;
}
