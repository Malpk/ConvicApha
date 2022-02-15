using UnityEngine;

public interface IPoint
{
    public bool IsBusy { get; }
    public ITileAnimation Animation {get;}

    public GameObject CreateObject(GameObject createObject);

    public void SetAtiveObject(bool mode);

    public void DestroyObject();
}
