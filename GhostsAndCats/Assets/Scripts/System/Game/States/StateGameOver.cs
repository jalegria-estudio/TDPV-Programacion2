using UnityEngine;

namespace System.Game.States
{
    /// <summary>
    /// Game Over Mode
    /// </summary>
    public enum GameOverMode
    {
        NONE,
        GAME_OVER_WIN,
        GAME_OVER_LOSE
    }

    /// <summary>
    /// State Game-Over
    /// </summary>
    class StateGameOver : IGameState
    {
        protected readonly GameManager m_gameManager = null;
        public GameOverMode m_mode = GameOverMode.NONE;

        public GameOverMode Mode { get => this.m_mode; set => this.m_mode = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_gameManager">Main game manager</param>
        public StateGameOver(GameManager p_gameManager)
        {
            this.m_gameManager = p_gameManager;
            this.m_mode = GameOverMode.NONE;
        }

        /// <summary>
        /// Logic that runs when enter to the state
        /// <summary>
        public void Enter()
        {
            GameObject l_player = GameObject.FindGameObjectWithTag("tPlayer");
            l_player.GetComponent<Player>().DisableActions();

            this.m_gameManager.GameEnding();//<(!) Load ending scene
            LevelManager l_lvlMngr = this.m_gameManager.LevelManager;
            l_lvlMngr.UnloadSceneByName(l_lvlMngr.GetLevelScene().name);//<(!) Unload Game level

            Debug.Log("GAME OVER!");
        }

        /// <summary>
        /// Logic that runs when exit from the state
        /// <summary>
        public void Exit()
        {
        }

        /// <summary>
        /// Fixed coded logic runs every frame. Note: include here a condition to transition to a new state
        /// <summary>
        public void Update()
        {
        }
    }
}//namespace System.Game.States
