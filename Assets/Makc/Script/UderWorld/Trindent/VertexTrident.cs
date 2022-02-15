using UnityEngine;

namespace Trident
{
    public class VertexTrident : IVertex
    {
        private GameObject _instateObject = null;

        private readonly Vector3 _position;

        public VertexTrident( Vector3 position)
        {
            _position = position;
        }

        public Vector2 position => _position;
        public VertexState state => _instateObject == null ? VertexState.UnBusy : VertexState.Busy;

        public GameObject InstateObject(GameObject instateObject, Transform parent = null)
        {
            if (_instateObject != null)
                return _instateObject;
            _instateObject = MonoBehaviour.Instantiate(instateObject, _position, Quaternion.identity);
            _instateObject.transform.parent = parent;
            return _instateObject;
        }
    }
}