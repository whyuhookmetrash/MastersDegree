using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Game
{
    public sealed class GameListenerComposite: MonoBehaviour,
        IGameStartListener,
        IGameFinishListener,
        IGamePauseListener,
        IGameResumeListener
    {
        [Inject]
        private GameCycle gameCycle;

        [InjectLocal]
        private List<IGameListener> listeners = new();


        private void Start()
        {
            gameCycle.AddListener(this);
        }

        private void OnDestroy()
        {
            gameCycle.RemoveListener(this);
        }

        void IGameStartListener.OnStartGame()
        {
            foreach (var it in listeners)
            {
                if (it is IGameStartListener listener)
                {
                    listener.OnStartGame();
                }
            }
        }

        void IGameFinishListener.OnFinishGame()
        {
            foreach (var it in listeners)
            {
                if (it is IGameFinishListener listener)
                {
                    listener.OnFinishGame();
                }
            }
        }

        void IGamePauseListener.OnPauseGame()
        {
            foreach (var it in listeners)
            {
                if (it is IGamePauseListener listener)
                {
                    listener.OnPauseGame();
                }
            }
        }

        void IGameResumeListener.OnResumeGame()
        {
            foreach (var it in listeners)
            {
                if (it is IGameResumeListener listener)
                {
                    listener.OnResumeGame();
                }
            }
        }


    }
}