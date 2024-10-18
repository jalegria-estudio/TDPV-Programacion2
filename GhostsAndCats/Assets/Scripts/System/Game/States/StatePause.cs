using UnityEngine;

namespace System.Game.States
{
    /// <summary>
    /// Inactive or pause state
    /// </summary>
    class StatePause : IGameState
    {
        GameObject m_player = null;
        readonly GameManager m_gameManager = null;

        /// <summary>
        /// Constructor because it not use MonoBehaviour
        /// </summary>
        public StatePause(GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
        }

        /// <summary>
        /// Logic that runs when enter to the state
        /// <summary>
        public void Enter()
        {
#if GAME_DEBUG
            Debug.Log("Enter to inactive state");
#endif
            m_player = GameObject.FindGameObjectWithTag("tPlayer");
            m_player.SetActive(false);
        }

        /// <summary>
        /// Logic that runs when exit from the state
        /// <summary>
        public void Exit()
        {
#if GAME_DEBUG
            Debug.Log("Exit to inactive state");
#endif
            m_player.SetActive(true);
        }

        // Fixed coded logic runs every frame.
        ///// DESIGN DECISION ALERT /////
        // Unity Pattern indica => Include here a condition to transition to a new state
        // Game Pattern indica => This class initializes the Context object and the states, and it also triggers state changes
        // <(!!) En este caso llamo a un metodo del controller (GameManager) para disparar el cambio de estado.
        public void Update()
        {
        }
    }
}//namespace System.Game.States
