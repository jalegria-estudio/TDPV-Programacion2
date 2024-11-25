#define GAME_DEBUG
#undef GAME_DEBUG

using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Game
{
    /// <summary>
    /// Main Central Game Manager Class
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("Configuration")]
        [Tooltip("Autostart the game without opening on specific level")]
        [SerializeField] protected bool m_autoStart = false;
        [Tooltip("Set on Autostart and insert a level ID. Value 2 the game starts in stage 1-1!")]
        [SerializeField] protected uint m_startLevel = 0;

        ///////////////////
        /// GAMES STATES
        /////////////////
        internal States.StatePlaying m_playingState;
        internal States.StateGameOver m_gameOverState;
        internal States.StatePause m_pauseState;

        protected GameStateMachine m_stateMachine = null;
        protected LevelManager m_levelManager = null;
        protected Services.SmsService m_smsService = null;
        protected Services.TimerService m_timerService = null;
        protected Services.AudioService m_audioService = null;
        protected GameObject m_level = null;

        public IGameState State { get => this.m_stateMachine.m_currentState; }
        public LevelManager LevelManager { get => this.m_levelManager; }
        public Services.SmsService SmsService { get => this.m_smsService; }
        public Services.TimerService TimerService { get => this.m_timerService; }
        public Services.AudioService AudioService { get => this.m_audioService; }

        public Player Player { get => FindFirstObjectByType<Player>(FindObjectsInactive.Include); } //Source: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Object.FindFirstObjectByType.html

        // Start is called before the first frame update
        void Start()
        {
            ///// Initialize State Machine /////
            this.m_stateMachine = new GameStateMachine();
            this.m_playingState = new States.StatePlaying(this);
            this.m_gameOverState = new States.StateGameOver(this);
            this.m_pauseState = new States.StatePause(this);
            this.m_stateMachine.Init(this.m_pauseState);

            ///// Initialize Level manager /////
            this.m_levelManager = this.gameObject.GetComponent<LevelManager>();
            this.m_levelManager.Init();
            this.m_levelManager.GoToMain();//<(!) Go to Main-Menu Scene
            ///// Initialize Services /////
            this.m_smsService = this.gameObject.GetComponent<Services.SmsService>();
            this.m_timerService = this.gameObject.GetComponent<Services.TimerService>();
            this.m_audioService = this.gameObject.GetComponent<Services.AudioService>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            this.m_stateMachine.Handle();
        }

        //////////////////////////////////////////////
        /// GAMES-ACTIONS - METHODS TO CHANGE LEVELS
        //////////////////////////////////////////////

        /// <summary>
        /// Set Game Start State
        /// </summary>
        public void GameStart()
        {
            this.Player.Data.ResetDefault();
            if (this.m_autoStart)
            {
                if (this.m_startLevel > 1)
                    this.m_levelManager.GoToLevel(this.m_startLevel);

                this.GamePlay();
            }
            else
            {
                this.GameOpening();
            }
        }

        /// <summary>
        /// Move to next level or end the gameplay(!!!) NOTE: TO REFACTORING
        /// </summary>
        public void MoveNextLevel()
        {
            if (this.m_levelManager.FinishCurrentLevel() && !this.m_levelManager.NextLevel())
            {
                this.GameOver(States.GameOverMode.GAME_OVER_WIN);//<(e) Si no hay más niveles el player gano el juego
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
            this.m_stateMachine.Init(this.m_pauseState);
        }

        /// <summary>
        /// Start the game-play
        /// </summary>
        public void GamePlay()
        {
            if (this.m_levelManager != null && this.m_levelManager.GetLevelScene().name == "Main")//<(i) When the opening is finished
            {
                this.MoveNextLevel();
            }

            this.m_stateMachine.ChangeTo(this.m_playingState);
        }

        /// <summary>
        /// Set GameOver State
        /// </summary>
        /// <param name="p_mode"></param>
        public void GameOver(States.GameOverMode p_mode = States.GameOverMode.GAME_OVER_LOSE)
        {
            this.m_gameOverState.Mode = p_mode;
            this.m_stateMachine.ChangeTo(this.m_gameOverState);
        }

        /// <summary>
        /// Load the game opening
        /// </summary>
        public void GameOpening()
        {
            SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
        }

        /// <summary>
        /// Load the game ending
        /// </summary>
        public void GameEnding()
        {
            this.m_levelManager.ActiveEnding();
        }
    }
} //namespace System.Game
