using UnityEngine;

public class Point
{
    private GameObject _item;

    public bool IsBusy => _item? _item.activeSelf : false;
    public Vector2 Position { get; private set; }

    public Point(Vector2 position)
    {
        Position = position;
    }

    public void SetItem<T>(T item) where T : MonoBehaviour
    {
        if (!IsBusy)
        {
            item.transform.position = Position;
            _item = item.gameObject;
        }
    }
    public void Delete(bool destroy = false)
    {
        if(_item && destroy)
            MonoBehaviour.Destroy(_item.gameObject);
        _item = null;
    }
}