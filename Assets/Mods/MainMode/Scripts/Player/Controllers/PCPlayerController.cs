using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    [RequireComponent(typeof(Player))]
    public class PCPlayerController : MonoBehaviour, MainMode.IBlock
    {
        [SerializeField] private KeyCode _intractive = KeyCode.Space;
        [SerializeField] private KeyCode _useItem = KeyCode.E;
        [SerializeField] private KeyCode _useAbility = KeyCode.R;
        [SerializeField] private KeyCode _useArtifact = KeyCode.Q;

        private Player _controlledPlayer;

        private void Awake()
        {
            _controlledPlayer = GetComponent<Player>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(_intractive))
                _controlledPlayer.InteractiveWhithObject();
            if (Input.GetKeyDown(_useItem)) 
                _controlledPlayer.UseItem();
            if (Input.GetKeyDown(_useAbility)) 
                _controlledPlayer.UseAbillity();
            if (Input.GetKeyDown(_useArtifact)) 
                _controlledPlayer.UseArtifact();
        }
        private void FixedUpdate()
        {
            _controlledPlayer.Walk(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
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