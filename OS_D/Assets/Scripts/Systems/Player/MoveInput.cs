
using UnityEngine;
using System;
using Zenject;

namespace Game
{
    public sealed class MoveInput : IMoveInput, IFixedTickable
    {
        public event Action<Vector2> OnMoveInput;

        void IFixedTickable.FixedTick()
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