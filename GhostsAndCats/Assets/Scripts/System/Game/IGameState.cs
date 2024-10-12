namespace System.Game
{
    public interface IGameState
    {
        // Logic that runs when enter to the state
        public void Enter();

        // Fixed coded logic runs every frame. Note: include here a condition to transition to a new state
        public void Update();

        // Logic that runs when exit from the state
        public void Exit();
    }
} //namespace System.Game

