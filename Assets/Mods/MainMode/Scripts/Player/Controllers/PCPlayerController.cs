using UnityEngine;
using MainMode;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class PCPlayerController : MonoBehaviour,IBlock
    {
        [SerializeField] private KeyCode _useItem = KeyCode.E;
        [SerializeField] private KeyCode _useAbility = KeyCode.R;
        [SerializeField] private KeyCode _useArtifact = KeyCode.Q;
        [Header("ControllComponent")]
        [SerializeField] private Player _controlledPlayer;
        [SerializeField] private PlayerInventory _inventory;
        [SerializeField] private PlayerMovementSet _movement;
        [SerializeField] private AbillityActiveSet _abillity;
        [SerializeField] private ShootMarkerView _shootMarker;

        private void Awake()
        {
            enabled = false;
        }
        private void Update()
        {
            if (Input.GetKeyDown(_useItem) || Input.GetKeyDown(KeyCode.Mouse1))
                _inventory.UseItem();
            if (Input.GetKeyDown(_useArtifact) || Input.GetKeyDown(KeyCode.Mouse0))
                _inventory.UseArtifact();
            UseAbillity();
        }
        public void SetAbillity(AbillityActiveSet abillity)
        {
            _abillity = abillity;
        }
        public void UseAbillity()
        {
            if (_abillity)
            {
                if (Input.GetKeyDown(_useAbility))
                {
                    if (_abillity.IsUseRotation)
                        _controlledPlayer.transform.rotation = Quaternion.Euler(Vector3.forward * (_shootMarker.Angel - 90));
                    _abillity.Use();
                }
            }
        }
        private void FixedUpdate()
        {
            _movement.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }

        public void Block()
        {
            enabled = false; 
        }
        public void UnBlock()
        {
            enabled = true;
        }
    }
}