using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class ShootMarker
    {
        private readonly Camera camera;
        private readonly Transform transform;

        public ShootMarker(CameraFollowing cameraFoolowing, Player player)
        {
            camera = cameraFoolowing.Camera;
            transform = player.transform;
        }

        public float Angel { get; private set; }

        public void Update()
        {
            
        }
    }
}
