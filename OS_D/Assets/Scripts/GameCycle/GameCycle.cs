using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class GameCycle
    {
        public event Action OnGameStarted;
        public event Action OnGameFinished;
        public event Action OnGamePaused;
        public event Action OnGameResumed;

        public GameState State {  get; private set; }

        private List<IGameListener> listeners = new();

        public void AddListener(IGameListener listener)
        {
            listeners.Add(listener);
        }
        public void RemoveListener(IGameListener listener)
        {
            listeners.Remove(listener);
        }
        public void StartGame()
        {
            if (this.State != GameState.OFF) { return; }
            this.State = GameState.PLAY;
            this.OnGameStarted?.Invoke();

            foreach (var it in listeners)
            {
                if (it is IGameStartListener listener)
                {
                    listener.OnStartGame();
                }
            }
        }

        public void FinishGame()
        {
            if (this.State != GameState.PLAY) { return; }
            this.State = GameState.FINISH;
            this.OnGameFinished?.Invoke();

            foreach (var it in listeners)
            {
                if (it is IGameFinishListener listener)
                {
                    listener.OnFinishGame();
                }
            }
        }

        public void PauseGame()
        {
            if (this.State != GameState.PLAY) { return; }
            this.State = GameState.PAUSE;
            this.OnGamePaused?.Invoke();

            foreach (var it in listeners)
            {
                if (it is IGamePauseListener listener)
                {
                    listener.OnPauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            if (this.State != GameState.PAUSE) { return; }
            this.State = GameState.PLAY;
            this.OnGameResumed?.Invoke();

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