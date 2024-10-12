namespace System.Game
{
    /**
     * STATE MACHINE PATTERN
     * Source: 
     *      Game Development Patterns with Unity 2021 - Chapter 5
     *      LEVEL UP YOUR CODE WITH GAME PROGRAMMING PATTERNS - Unity Doc 2022. => Ojo! Con las dependencias
     */
    [Serializable]
    public class GameStateMachine//Equivalente BikeContext
    {
        //protected readonly GameObject m_gameManager;
        public IGameState m_currentState = null;

        public IGameState CurrentState { get => m_currentState; protected set => m_currentState = value; }

        public GameStateMachine()
        {
            //m_gameManager = p_gameManager;
        }

        public void Init(IGameState p_startingState)
        {
            m_currentState = p_startingState;
            m_currentState?.Enter();
        }

        public void ChangeTo(IGameState p_nextState)
        {
            m_currentState?.Exit();
            m_currentState = p_nextState;
            m_currentState?.Enter();
        }

        public void Handle()
        {
            m_currentState?.Update(); //<(e) Equivalente a:  if (m_currentState != null) m_currentState.Update();
        }
    }
} //namespace System.Game
