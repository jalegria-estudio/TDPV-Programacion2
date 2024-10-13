using UnityEngine;
using UnityEngine.Playables;

namespace System.Game
{
    public class GameManager : MonoBehaviour // Equivalente BikeController
    {
        [Header("Configuration")]
        [SerializeField] protected bool m_autoStart = false;

        internal States.StatePlaying m_playingState;
        internal States.StateGameOver m_gameOverState;
        internal States.StatePause m_pauseState;

        protected GameStateMachine m_stateMachine = null;
        protected LevelManager m_levelManager = null;
        protected GameObject m_level = null;

        // Start is called before the first frame update
        void Start()
        {
            m_levelManager = gameObject.GetComponent<LevelManager>();
            m_stateMachine = new GameStateMachine();
            m_playingState = new States.StatePlaying(this);
            m_gameOverState = new States.StateGameOver(this);
            m_pauseState = new States.StatePause(this);

            if (m_autoStart)
            {
                m_stateMachine.Init(m_playingState);
            }
            else
            {
                m_stateMachine.Init(m_pauseState);
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
            m_stateMachine.ChangeTo(m_playingState);
        }

        /// <summary>
        /// Start a opening animation timeline
        /// </summary>
        public void GameOpening()
        {
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
            if (m_levelManager.NextLevel(out m_level))
            {
                GameObject l_bounds = GameObject.FindGameObjectWithTag("tCameraBounds");
                Camera l_camera = Camera.main;
                FollowerCamera l_follower = l_camera.GetComponent<FollowerCamera>();
                l_follower.SetBounds(l_bounds.GetComponent<BoxCollider2D>());
            }
            else //<(e) Si no hay más niveles el player gano el juego
            {
                GameOver(States.GameOverMode.GAME_OVER_WIN);
            }
        }

    }
} //namespace System.Game
