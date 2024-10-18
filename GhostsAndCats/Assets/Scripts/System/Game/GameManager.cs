#define GAME_DEBUG
//#undef GAME_DEBUG

using UnityEngine;
using UnityEngine.Playables;

namespace System.Game
{
    public class GameManager : MonoBehaviour // Equivalente a BikeController
    {
        [Header("Configuration")]
        [SerializeField] protected bool m_autoStart = false;
        //[SerializeField] protected int m_levelId = 0;

        internal States.StatePlaying m_playingState;
        internal States.StateGameOver m_gameOverState;
        internal States.StatePause m_pauseState;

        protected GameStateMachine m_stateMachine = null;
        protected LevelManager m_levelManager = null;
        protected GameObject m_level = null;

        public LevelManager LevelManager { get => m_levelManager; }

        // Start is called before the first frame update
        void Start()
        {
            m_stateMachine = new GameStateMachine();
            m_playingState = new States.StatePlaying(this);
            m_gameOverState = new States.StateGameOver(this);
            m_pauseState = new States.StatePause(this);
            m_levelManager = gameObject.GetComponent<LevelManager>();
            m_levelManager.init();

            if (m_autoStart)
            {
                GameObject.Find("Room").SetActive(false);
                GameObject.Find("ButtonStart").SetActive(false);
                GameStart();
            }
            else
            {
                GamePause();
            }

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_stateMachine.Handle();
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

        /// <summary>
        /// Set Game Start State
        /// </summary>
        public void GameStart()
        {
            m_levelManager.NextLevel();
            m_stateMachine.ChangeTo(m_playingState);
        }

        /// <summary>
        /// Start a opening animation timeline
        /// </summary>
        public void GameOpening()
        {
#if GAME_DEBUG
            Debug.Log("<DEBUG>Start Openging!");
#endif

            GameObject.FindGameObjectWithTag("tStartButton").SetActive(false);

            PlayableDirector l_director = GameObject.FindObjectOfType<PlayableDirector>();
            if (l_director != null)
                l_director.Play();
        }

        /// <summary>
        /// Move to next level or end the gameplay(!!!) NOTE: TO REFACTORING
        /// </summary>
        public void GameNextLevel()
        {
            if (!m_levelManager.NextLevel())
            {
                GameOver(States.GameOverMode.GAME_OVER_WIN);//<(e) Si no hay más niveles el player gano el juego
            }
        }

        /// <summary>
        /// Pause tha game
        /// </summary>
        public void GamePause()
        {
            m_stateMachine.Init(m_pauseState);
        }
    }
} //namespace System.Game
