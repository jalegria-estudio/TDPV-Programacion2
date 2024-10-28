using Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Game
{
    /// <summary>
    /// Levels Manager for the game
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Configuration")]
        protected int m_currentLevel = 0;
        protected List<string> m_levels = new List<string>();//<(i) Source: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1
        //protected const int m_QTY_LEVELS = 2;

        /// <summary>
        /// Get and set current level index
        /// </summary>
        public int CurrentLevelID
        {
            get => m_currentLevel;
            set => m_currentLevel = value; //<(!) Danger code
        }

        /// <summary>
        /// Get current level scene
        /// </summary>
        /// <returns></returns>
        public Scene GetLevelScene()
        {
            return SceneManager.GetSceneByName(m_levels[m_currentLevel]);
        }

        /// <summary>
        /// Get current level data 
        /// </summary>
        /// <returns></returns>
        public LevelData GetLevelData()
        {
            Level l_level = GameObject.FindFirstObjectByType<Level>();

            return l_level.Data;
        }

        /// <summary>
        /// Initialize the level manager
        /// </summary>
        public void init()
        {
            m_levels.Add("Game");//<(e) Game scene =>id0
            m_levels.Add("Main");
            m_levels.Add("Level1-1");
            m_levels.Add("Level1-2");

            /// Suscription to SceneManager Events => https://docs.unity3d.com/2022.3/Documentation/ScriptReference/SceneManagement.SceneManager.html
            SceneManager.sceneLoaded += OnLoadedLevel;
        }

        /// <summary>
        /// Enable a game object level
        /// </summary>
        /// <param name="p_levelID">Level Index</param>
        public void Active(int p_levelID)
        {
            if (p_levelID < 0 || p_levelID > m_levels.Count)
            {
                Debug.LogWarning($"(!) Scene Level didn't load by out-rangue index. > {p_levelID}");
                return;
            }

            //m_levels[p_levelID].SetActive(true);
            SceneManager.LoadSceneAsync(m_levels[p_levelID], LoadSceneMode.Additive);
            m_currentLevel = p_levelID;
        }

        /// <summary>
        /// Disable a game object level
        /// </summary>
        /// <param name="p_levelID">Level Index</param>
        public void Inactive(int p_levelID)
        {
            if (p_levelID <= 0 || p_levelID > m_levels.Count)
            {
                Debug.LogWarning($"(!) Scene Level didn't unload by out-rangue index. > {p_levelID}");
                return;
            }

            //m_levels[p_levelID].SetActive(false);
            SceneManager.UnloadSceneAsync(m_levels[p_levelID]);
        }

        /// <summary>
        /// It moves to the next level
        /// </summary>
        public bool NextLevel()
        {
            if ((m_currentLevel + 1) >= m_levels.Count)
            {
                return false;
            }

            if (m_currentLevel != 0) //<(e) The game scene is Game Scene = id0
                Inactive(m_currentLevel);

            m_currentLevel++;
            Active(m_currentLevel);

            return true;
        }

        /// <summary>
        /// Load level configuration to game scene when this loaded
        /// Source: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
        /// </summary>
        /// <param name="p_scene"></param>
        /// <param name="p_mode"></param>
        public void OnLoadedLevel(Scene p_scene, LoadSceneMode p_mode)
        {
            /**
             * <(!) Suscription to SceneManager.sceneLoaded
             * Add a delegate to this to get notifications when a Scene has loaded.
             * Rather than being called directly this script code shows use of a delegate.
             * NOTE: It's necessery waiting for all components are loaded for load scene's levels.
             */
            if (p_scene.name != m_levels[m_currentLevel])
                return;
#if GAME_DEBUG
            //SCENES LOADED STATUS//
            Scene l_scene = SceneManager.GetActiveScene();
            Scene l_levelScene = SceneManager.GetSceneByName(m_levels[m_currentLevel]);
            Debug.Log($"<DEBUG>Loaded Scene qty: {SceneManager.loadedSceneCount}");
            Debug.Log($"<DEBUG>Level Scene is loaded: {l_levelScene.isLoaded}");
#endif
            SetupLevel();
        }

        /// <summary>
        /// Config level and player data object
        /// </summary>
        /// <returns></returns>
        public bool SetupLevel()
        {
            // Setups //
            Level l_level = GameObject.FindFirstObjectByType<Level>(); //<(i) Search playable level data: start-point, goal-point, etc.

            if (l_level == null)
            {
                Debug.LogWarning($"Not found data-level in {SceneManager.GetSceneAt(m_currentLevel).name} scene. The scene isn't configured!");
                return false;
            }

            SetupPlayer(l_level);
            SetupCamera(l_level);

            return true;
        }


        /// <summary>
        /// Config main camera with Level Data in Game scene follower-camera component
        /// </summary>
        public bool SetupCamera(Level p_level)
        {
            //Load main camera bounds and position
            GameObject l_bounds = GameObject.FindWithTag("tCameraBounds");//GameObject.FindGameObjectWithTag("tCameraBounds");
            if (l_bounds == null)
            {
                Debug.LogWarning($"Not found main-camera bounds for the level in {SceneManager.GetSceneAt(m_currentLevel).name} scene!");
                return false;
            }

            Camera l_camera = Camera.main;
            FollowerCamera l_follower = l_camera.GetComponent<FollowerCamera>();

            l_follower.SetBounds(l_bounds.GetComponent<BoxCollider2D>());
            l_follower.SetCameraPosition(p_level.Data.CameraPosition);
            l_follower.TurnOn = true;

            return true;
        }

        /// <summary>
        /// Config Player Level Data in Game scene components
        /// </summary>
        public bool SetupPlayer(Level p_level)
        {
            //<(i) It's a method from Generic -Object- Class => Object.FindFirstObjectByType =>
            // In Documentation doesn't notice about You can search inactive objects!
            // <(i) Nota: Se puede configurar los componentes de inactive objects. Posibilidad de refactorin!
            // Source: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Object.FindFirstObjectByType.html
            Player l_player = FindFirstObjectByType<Player>(FindObjectsInactive.Include);
            //GameObject l_player = GameObject.FindWithTag("tPlayer");

            //if (l_player == null || !l_player.activeSelf)
            if (l_player == null)
            {
                Debug.LogWarning($"Player isn't active for the level in {SceneManager.GetSceneAt(m_currentLevel).name} scene. It couldn't be configured!");
                return false;
            }
            else
            {
                Debug.Log($"Player will be configured for {p_level.name} scene");
            }

            //<(i) Config player position on level
            Vector2 l_startPos = p_level.Data.StartPoint;
            l_player.GetComponent<Player>().SpawnPosition = l_startPos;
            l_player.GetComponent<Player>().TranslateToSpawnPosition();

            /// <(i) Load data for status player
            ExperienceManager l_StatusManager = null;
            if (l_player.TryGetComponent<ExperienceManager>(out l_StatusManager))
                l_StatusManager.LevelData = p_level.Data;
            else
                Debug.LogWarning("<DEBUG> Level component isn't loaded for Player Experience Manager!");

            return true;
        }
    }
} //namespace System.Game
