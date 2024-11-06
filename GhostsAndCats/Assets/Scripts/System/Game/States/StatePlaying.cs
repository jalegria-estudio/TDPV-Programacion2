using UnityEngine;

namespace System.Game.States
{
    /// <summary>
    /// Playing State
    /// </summary>
    class StatePlaying : IGameState
    {
        Player m_player = null;
        readonly GameManager m_gameManager = null;

        /// <summary>
        /// Constructor because it not use MonoBehaviour
        /// </summary>
        public StatePlaying(GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
        }
        /// <summary>
        /// Logic that runs when enter to the state
        /// </summary>
        public void Enter()
        {
            Debug.Log("Enter to playing state");
            m_player = GameObject.FindGameObjectWithTag("tPlayer").GetComponent<Player>();
            GameObject.FindFirstObjectByType<AudioSource>().Play();

            //<(i) Register evt_goal for when the player finishes the stage
            m_player.EVT_GOAL += m_gameManager.MoveNextLevel;
        }

        /// <summary>
        /// Logic that runs when exit from the state
        /// </summary>
        public void Exit()
        {
            Debug.Log("Exit from playing state");
            m_player.EVT_GOAL -= m_gameManager.MoveNextLevel;
        }

        // Fixed coded logic runs every frame.
        ///// DESIGN DECISION ALERT /////
        // Unity Pattern indica => Include here a condition to transition to a new state
        // Game Pattern indica => This class initializes the Context object and the states, and it also triggers state changes
        // <(!!) En este caso llamo a un metodo del controller (GameManager) para disparar el cambio de estado.
        public void Update()
        {
            if (m_player.IsDefeated())
            {
                Debug.Log("PLayer is defeated!");
                m_gameManager.GameOver(GameOverMode.GAME_OVER_LOSE);
            }
        }
    }// class StatePlaying
}//namespace System.Game.States
