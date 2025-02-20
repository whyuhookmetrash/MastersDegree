using Zenject;
using UnityEngine;

namespace Game
{
    public sealed class CameraFollower : ILateTickable
    {
        private readonly Player player;
        private readonly Camera camera;
        private readonly Vector3 offset;
        public CameraFollower(Player player, Camera camera, Vector3 offset)
        {
            this.player = player;
            this.camera = camera;
            this.offset = offset;
        }


        void ILateTickable.LateTick()
        {
            if (this.camera == null)
            {
                return;
            }
            Vector3 position = this.player.transform.position - this.offset;
            this.camera.transform.position = position;
        }
    }
}