#define GAME_DEBUG
//#undef GAME_DEBUG

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace System.Game
{
    public class GameManager : MonoBehaviour // Equivalente a BikeController
    {
        [Header("Configuration")]
        [SerializeField] protected bool m_autoStart = false;
        [Tooltip("Set on Autostart and insert a level ID. Value 2 the game starts in stage 1-1!")]
        [SerializeField] protected uint m_startLevel = 0;

        internal States.StatePlaying m_playingState;
        internal States.StateGameOver m_gameOverState;
        internal States.StatePause m_pauseState;

        protected GameStateMachine m_stateMachine = null;
        protected LevelManager m_levelManager = null;
        protected GameObject m_level = null;

        public Type State { get => m_stateMachine.m_currentState.GetType(); }


        // Start is called before the first frame update
        void Start()
        {
            ///// Initialize State Machine /////
            m_stateMachine = new GameStateMachine();
            m_playingState = new States.StatePlaying(this);
            m_gameOverState = new States.StateGameOver(this);
            m_pauseState = new States.StatePause(this);
            m_stateMachine.Init(m_pauseState);

            ///// Initialize Level manager /////
            m_levelManager = gameObject.GetComponent<LevelManager>();
            m_levelManager.init();
            m_levelManager.NextLevel();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_stateMachine.Handle();
        }

        //////////////////////////////////////////////
        /// GAMES-ACTIONS - METHODS TO CHANGE LEVELS
        //////////////////////////////////////////////

        /// <summary>
        /// Set Game Start State
        /// </summary>
        public void GameStart()
        {
            if (m_autoStart)
            {
                if (m_startLevel > 1)
                    m_levelManager.GoToLevel(m_startLevel);

                GamePlay();
            }
            else
            {
                GameOpening();
            }
        }

        /// <summary>
        /// Move to next level or end the gameplay(!!!) NOTE: TO REFACTORING
        /// </summary>
        public void MoveNextLevel()
        {
            if (!m_levelManager.NextLevel())
            {
                GameOver(States.GameOverMode.GAME_OVER_WIN);//<(e) Si no hay más niveles el player gano el juego
            }
        }

        //////////////////////////////////////////////
        /// GAMES-STATES - METHODS TO CHANGE STATES
        //////////////////////////////////////////////

        /// <summary>
        /// Pause tha game
        /// </summary>
        public void GamePause()
        {
            m_stateMachine.Init(m_pauseState);
        }

        /// <summary>
        /// Start the game-play
        /// </summary>
        public void GamePlay()
        {
            if (m_levelManager.GetLevelScene().name == "Main")//<(i) When the opening is finished
            {
                MoveNextLevel();
            }

            m_stateMachine.ChangeTo(m_playingState);
        }

        /// <summary>
        /// Set GameOver State
        /// </summary>
        /// <param name="p_mode"></param>
        public void GameOver(States.GameOverMode p_mode = States.GameOverMode.GAME_OVER_LOSE)
        {
            m_gameOverState.Mode = p_mode;
            m_stateMachine.ChangeTo(m_gameOverState);
        }

        public void GameOpening()
        {
            SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
        }
    }
} //namespace System.Game
