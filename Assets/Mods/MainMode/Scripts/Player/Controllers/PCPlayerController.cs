using UnityEngine;
using MainMode;

namespace PlayerComponent
{
    public class PCPlayerController : MonoBehaviour,IBlock
    {
        [SerializeField] private KeyCode _intractive = KeyCode.Space;
        [SerializeField] private KeyCode _useItem = KeyCode.E;
        [SerializeField] private KeyCode _useAbility = KeyCode.R;
        [SerializeField] private KeyCode _useArtifact = KeyCode.Q;
        [Header("Reference")]
        [SerializeField] private Player _controlledPlayer;
        [SerializeField] private PlayerInventory _inventory;

        private IMovement _movementSet;

        private void Awake()
        {
            _controlledPlayer = GetComponent<Player>();
            enabled = false;
        }
        public void SetMovement(IMovement movementSet)
        {
            _movementSet = movementSet;
            enabled = true;
        }
        private void Update()
        {
            if (Input.GetKeyDown(_intractive))
                _controlledPlayer.InteractiveWhithObject();
            if (Input.GetKeyDown(_useItem) || Input.GetKeyDown(KeyCode.Mouse1))
                _inventory.UseItem();
            if (Input.GetKeyDown(_useAbility)) 
                _controlledPlayer.UseAbillity();
            if (Input.GetKeyDown(_useArtifact) || Input.GetKeyDown(KeyCode.Mouse0))
                _inventory.UseArtifact();
        }
        private void FixedUpdate()
        {
            _movementSet.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
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