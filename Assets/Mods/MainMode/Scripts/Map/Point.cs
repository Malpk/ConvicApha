using UnityEngine;

public class Point
{
    private GameObject _item;

    public int Size { get; private set; } = 0;
    public bool IsBusy => _item? _item.activeSelf : false;
    public Vector2 Position { get; private set; }
    public Vector2Int ArrayPosition { get; private set; }

    public Point(Vector2 position, Vector2Int arrayPosition)
    {
        Position = position;
        ArrayPosition = arrayPosition;
    }

    public void SetItem<T>(T item, int size = 0) where T : MonoBehaviour
    {
        if (!IsBusy)
        {
            Size = size;
            item.transform.position = Position;
            _item = item.gameObject;
        }
    }
    public void Delete(bool destroy = false)
    {
        if(_item && destroy)
            MonoBehaviour.Destroy(_item.gameObject);
        Size = 0;
        _item = null;
    }
}