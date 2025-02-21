using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameCycleController: ITickable
    {
        private readonly GameCycle gameCycle;
        public GameCycleController(GameCycle gameCycle)
        {
            this.gameCycle = gameCycle;
        }

        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.S)) this.gameCycle.StartGame();
            if (Input.GetKeyDown(KeyCode.F)) this.gameCycle.FinishGame();
            if (Input.GetKeyDown(KeyCode.P)) this.gameCycle.PauseGame();
            if (Input.GetKeyDown(KeyCode.R)) this.gameCycle.ResumeGame();
        }

    }
}