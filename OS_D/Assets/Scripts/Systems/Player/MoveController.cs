
using System;
using Zenject;

namespace Game
{
    public sealed class MoveController: IGameStartListener, IGameFinishListener
    {
        private readonly Player player;
        private readonly IMoveInput moveInput;

        public MoveController(Player player, IMoveInput moveInput)
        {
            this.player = player;
            this.moveInput = moveInput;
        }

        void IGameStartListener.OnStartGame()
        {
            this.moveInput.OnMoveInput += this.player.Move;
        }

        void IGameFinishListener.OnFinishGame()
        {
            this.moveInput.OnMoveInput -= this.player.Move;
        }

    }
}