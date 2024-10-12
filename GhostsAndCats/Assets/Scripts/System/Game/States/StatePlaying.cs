using UnityEngine;

namespace System.Game.States
{
    class StatePlaying : IGameState
    {
        Player m_player = null;
        readonly GameManager m_gameManager = null;
        //Constructor because it not use MonoBehaviour
        public StatePlaying(GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
        }

        // Logic that runs when enter to the state
        public void Enter()
        {
            Debug.Log("Enter to playing state");
            m_player = GameObject.FindGameObjectWithTag("tPlayer").GetComponent<Player>();
            //<(i) Register evt_goal
            m_player.EVT_GOAL += m_gameManager.GameNextLevel;
        }

        // Logic that runs when exit from the state
        public void Exit()
        {
            Debug.Log("Exit to playing state");
            m_player.EVT_GOAL -= m_gameManager.GameNextLevel;
        }

        // Fixed coded logic runs every frame.
        ///// DESIGN DECISION ALERT /////
        // Unity Pattern indica => Include here a condition to transition to a new state
        // Game Pattern indica => This class initializes the Context object and the states, and it also triggers state changes
        // <(!!) En este caso llamo a un metodo del controller (GameManager) para disparar el cambio de estado.
        public void Update()
        {
            //Debug.Log($"Player => Lifes: {m_player.Lifes}");

            if (m_player.Lifes == 0)
            {
                m_gameManager.GameOver(GameOverMode.GAME_OVER_LOSE);
            }
        }
    }
}//namespace System.Game.States
