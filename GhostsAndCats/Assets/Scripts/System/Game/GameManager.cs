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
        protected const int m_QTY_LEVELS = 2;


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

        public void GameOver(States.GameOverMode p_mode = States.GameOverMode.GAME_OVER_LOSE)
        {
            m_gameOverState.Mode = p_mode;
            m_stateMachine.ChangeTo(m_gameOverState);
        }

        public void GameStart()
        {
            m_stateMachine.ChangeTo(m_playingState);
        }

        public void GameOpening()
        {
            GameObject.FindGameObjectWithTag("tStartButton").SetActive(false);
            GameObject.FindObjectOfType<PlayableDirector>()?.Play();
        }

        public void GameNextLevel()
        {
            if (m_levelManager.NextLevel(out m_level))
            {
                GameObject l_bounds = GameObject.FindGameObjectWithTag("tCameraBounds");
                Camera l_camera = Camera.main;
                FollowerCamera l_follower = l_camera.GetComponent<FollowerCamera>();
                l_follower.SetBounds(l_bounds.GetComponent<BoxCollider2D>());
            }
            else
            {
                GameOver(States.GameOverMode.GAME_OVER_WIN);
            }
        }

    }
} //namespace System.Game
