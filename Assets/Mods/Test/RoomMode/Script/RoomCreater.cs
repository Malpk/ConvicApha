using UnityEngine;

namespace MainMode.Room
{
    public class RoomCreater : MonoBehaviour
    {
        [Header("Room Setting")]
        [SerializeField] private bool _playOnAwake = false;
        [SerializeField] private int _exitWidht = 1;
        [SerializeField] private int _countExit = 1;
        [SerializeField] private Vector2 _unitSize = Vector2.one;
        [SerializeField] private Vector2Int _sizeRoom;
        [Header("Reference")]
        [SerializeField] private SpriteRenderer _floorPrefab;

        private Transform _roomHolder;

        private void Start()
        {
            if (_playOnAwake)
                CreateRoom(_exitWidht, _countExit);

        }

        public void CreateRoom(int exitWidht, int countExit)
        {
            
        }
    }
}