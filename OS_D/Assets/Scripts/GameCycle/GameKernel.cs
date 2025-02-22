using Zenject;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class GameKernel: MonoKernel
    {
        [Inject]
        GameCycle gameCycle;

        [Inject(Optional = true, Source = InjectSources.Local)]
        private List<IGameTickable> tickables = new();

        [Inject(Optional = true, Source = InjectSources.Local)]
        private List<IGameFixedTickable> fixedTickables = new();

        [Inject(Optional = true, Source = InjectSources.Local)]
        private List<IGameLateTickable> lateTickables = new();

        public override void Update()
        {
            base.Update();

            if (this.gameCycle.State == GameState.PLAY)
            {
                float deltaTime = Time.deltaTime;
                foreach (var tickable in this.tickables)
                {
                    tickable.Tick(deltaTime);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.gameCycle.State == GameState.PLAY)
            {
                float deltaTime = Time.fixedDeltaTime;
                foreach (var tickable in this.fixedTickables)
                {
                    tickable.FixedTick(deltaTime);
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (this.gameCycle.State == GameState.PLAY)
            {
                float deltaTime = Time.deltaTime;
                foreach (var tickable in this.lateTickables)
                {
                    tickable.LateTick(deltaTime);
                }
            }
        }
    }
}