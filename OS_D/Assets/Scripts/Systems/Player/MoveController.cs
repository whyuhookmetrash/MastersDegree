
using System;
using Zenject;

namespace Game
{
    public sealed class MoveController: IInitializable, IDisposable
    {
        private readonly Player player;
        private readonly IMoveInput moveInput;

        public MoveController(Player player, IMoveInput moveInput)
        {
            this.player = player;
            this.moveInput = moveInput;
        }

        void IInitializable.Initialize()
        {
            this.moveInput.OnMoveInput += this.player.Move;
        }

        void IDisposable.Dispose()
        {
            this.moveInput.OnMoveInput -= this.player.Move;
        }

    }
}