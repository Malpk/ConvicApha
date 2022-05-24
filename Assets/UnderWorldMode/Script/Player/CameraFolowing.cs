using UnityEngine;

namespace Underworld
{
    public class CameraFolowing : MonoBehaviour
    {
        [Header("Scene Setting")]
        [SerializeField] private Player _target;
        [SerializeField] private Vector2 _offset;

        public Vector3 CameraPosition => transform.position;

        void LateUpdate()
        {
            transform.position = new Vector3(_target.Position.x, _target.Position.y, transform.position.z);
        }
    }
}