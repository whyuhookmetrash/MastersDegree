namespace Game
{
    public interface IGameListener { }

    public interface IGameStartListener : IGameListener
    {
        void OnStartGame();
    }
    public interface IGameFinishListener : IGameListener
    {
        void OnFinishGame();
    }
    public interface IGamePauseListener : IGameListener
    {
        void OnPauseGame();
    }
    public interface IGameResumeListener : IGameListener
    {
        void OnResumeGame();
    }


}