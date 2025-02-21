
using UnityEngine;
using System;
using Zenject;

namespace Game
{
    public sealed class MoveInput : IMoveInput, IGameFixedTickable
    {
        public event Action<Vector2> OnMoveInput;

        void IGameFixedTickable.FixedTick(float deltaTime)
        {
            Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (direction.magnitude > 1) { direction.Normalize(); }

            if (direction != Vector2.zero)
            {
                OnMoveInput?.Invoke(direction);
            }
        }
    }
}