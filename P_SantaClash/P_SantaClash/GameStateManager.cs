namespace P_SantaClash
{
    public enum GameState
    {
        Menu,
        Playing,
        GameOver
    }

    public class GameStateManager
    {
        public GameState State { get; private set; } = GameState.Menu;

        public void GoToMenu() => State = GameState.Menu;
        public void StartGame() => State = GameState.Playing;
        public void GameOver() => State = GameState.GameOver;
    }
}
