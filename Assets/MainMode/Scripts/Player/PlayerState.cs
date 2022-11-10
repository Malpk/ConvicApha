using UnityEngine;

namespace PlayerComponent
{
    [CreateAssetMenu(menuName = "PlayerComponent/State")]
    public class PlayerState : ScriptableObject
    {
        [Header("Movement Setting")]
        [Min(1)]
        [SerializeField] private float _speedMovement;
        [Min(1)]
        [SerializeField] private float _speedRotation;

        public float SpeedMovement => _speedMovement;
        public float SpeedRotation => _speedRotation;
    }
}