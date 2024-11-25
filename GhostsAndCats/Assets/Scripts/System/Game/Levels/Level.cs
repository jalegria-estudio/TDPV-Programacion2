#define GAME_DEBUG
#undef GAME_DEBUG

using UnityEngine;

namespace System.Game
{
    public class Level : MonoBehaviour
    {
        public event System.Action EVT_START_GAMEPLAY;
        public event System.Action EVT_STOP_GAMEPLAY;

        /// <summary>
        /// A wrapper to allocate level data
        /// </summary>
        [SerializeField] protected LevelData m_data;
        public LevelData Data { get => this.m_data; }

        //protected bool m_updateTimerAuto = true;
        protected Services.TimerService m_timer = null;
        protected Services.AudioService m_jukebox = null;

        protected void Start()
        {
#if GAME_DEBUG
            Debug.Log("Level ON START");
#endif
            this.m_timer = GameManager.Instance.TimerService;
            this.m_jukebox = GameManager.Instance.AudioService;

            if (this.Data.ShowStageRound && !GameManager.Instance.LevelManager.Reloaded)
            {
                this.ShowRound();
            }
            else
            {
                this.StartGameplay();
            }
        }

        protected void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.Player.EVT_DEFEATED -= this.StopGameplay;

            GameObject l_gameObject = GameObject.FindGameObjectWithTag("tBoss");
            if (l_gameObject != null && l_gameObject.TryGetComponent<Boss>(out Boss l_boss))
            {
                l_boss.EVT_KNOCK_OUT -= this.StopGameplay;
                GameManager.Instance.Player.UnregisterBossEvents(l_boss);
            }
        }

        protected void Update()
        {

        }

        /// <summary>
        /// Start the level to player's gameplay. It's called to start after round-stage first time.
        /// </summary>
        public void StartGameplay()
        {
#if GAME_DEBUG
            Debug.Log("Level ON START-GAMEPLAY");
#endif
            Debug.Assert(this.m_timer != null, "<DEBUG ASSERT> Timer Service reference is null!");
            Debug.Assert(this.m_jukebox != null, "<DEBUG ASSERT> Audio Service reference is null!");

            this.m_timer.ResetTimer();
            this.m_timer.PlayTimer((float)this.m_data.m_time);

            if (!GameManager.Instance.LevelManager.Reloaded)//<(!) it helps to not replay music clip all time when restart a level
                this.m_jukebox.PlayMusic(this.m_data.AudioClipGameplay);

            this.SetupPlayer();
            this.SetupBoss();

            EVT_START_GAMEPLAY?.Invoke();
        }

        /// <summary>
        /// Start the level to player's gameplay. It's called to start after round-stage first time.
        /// </summary>
        public void StopGameplay()
        {
#if GAME_DEBUG
            Debug.Log("Level ON STOP-GAMEPLAY");
#endif
            Debug.Assert(this.m_timer != null, "<DEBUG> Timer Service reference is null!");
            Debug.Assert(this.m_jukebox != null, "<DEBUG> Audio Service reference is null!");

            this.m_timer.StopTimer();
            this.m_jukebox.StopMusic();

            EVT_STOP_GAMEPLAY?.Invoke();
        }

        /// <summary>
        /// Show round-stage before start gameplay
        /// </summary>
        protected void ShowRound()
        {
            GameManager.Instance.LevelManager.GoToReport();
            Debug.Log($"STAGE {this.Data.m_level}-{this.Data.m_sublevel}");
        }

        /// <summary>
        /// Config Player character for current level
        /// </summary>
        protected void SetupPlayer()
        {
            GameManager.Instance.Player.EnableActions();
            GameManager.Instance.Player.EVT_DEFEATED += this.StopGameplay;
        }

        /// <summary>
        /// Config the level boss if it exists. Note: This must be inactiva.-
        /// </summary>
        public bool SetupBoss()
        {
            //GameObject l_gameObject = GameObject.FindGameObjectWithTag("tBoss");
            GameObject[] l_objects = (GameObject[])FindObjectsByType(typeof(GameObject), FindObjectsInactive.Include, FindObjectsSortMode.None);
            GameObject l_bossGameObject = null;

            for (int i = 0; i < l_objects.Length; i++)
            {
                if (l_objects[i].activeSelf == false && l_objects[i].name == "Boss") //<(!) Try to find the first boss inactive
                {
                    l_bossGameObject = l_objects[i];
                    l_bossGameObject.TryGetComponent<Boss>(out Boss l_boss);
                    l_bossGameObject.SetActive(true);
                    l_boss.EVT_KNOCK_OUT += this.StopGameplay;
                    GameManager.Instance.Player.RegisterBossEvents(l_boss);

                    return true;
                }
            }

            return false;
        }
#if GAME_DEBUG
        /// <summary>
        /// Simple Render Status Player
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 30), $"< GAME TIME: {this.m_timer.CurrentTime} >");
        }
#endif
    }
}//namespace System.Game