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
            get => this.m_currentLevel;
            protected set => this.m_currentLevel = value;
        }

        /// <summary>
        /// Indicate if current Level is reloaded
        /// </summary>
        protected bool m_reloaded = false;
        public bool Reloaded
        {
            get => this.m_reloaded;
            protected set => this.m_reloaded = value;
        }

        /// <summary>
        /// Get current level scene
        /// </summary>
        /// <returns></returns>
        public Scene GetLevelScene()
        {
            return SceneManager.GetSceneByName(this.m_levels[this.m_currentLevel]);
        }

        /// <summary>
        /// Get current level component
        /// </summary>
        /// <returns></returns>
        public Level GetLevel()
        {
            Level l_level = null;
            GameObject.FindWithTag("tLevel")?.TryGetComponent<Level>(out l_level); //<(!) I can use GetLevelData mode
            return (l_level != null) ? l_level : null;
        }


        /// <summary>
        /// Get current level data 
        /// </summary>
        /// <returns></returns>
        public LevelData GetLevelData()
        {
            Level l_level = GameObject.FindFirstObjectByType<Level>();

            return (l_level != null) ? l_level.Data : null;
        }

        /// <summary>
        /// Initialize the level manager
        /// </summary>
        public void Init()
        {
            this.m_levels.Add("Game");//<(e) Game scene =>id0
            this.m_levels.Add("Main");
            this.m_levels.Add("Level1-1");
            this.m_levels.Add("Level1-2");
            this.m_levels.Add("Level1-3");

            /// Suscription to SceneManager Events => https://docs.unity3d.com/2022.3/Documentation/ScriptReference/SceneManagement.SceneManager.html
            SceneManager.sceneLoaded += this.OnLoadedLevel;
        }

        /// <summary>
        /// Load level configuration to game scene when this loaded
        /// Source: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
        /// </summary>
        /// <param name="p_scene">Scene content in Unity - Data Structure</param>
        /// <param name="p_mode">Type of Scene loads. Single mode loads a standard Unity Scene which then appears on its own in the Hierarchy window. Additive loads a Scene which appears in the Hierarchy window while another is active.</param>
        protected void OnLoadedLevel(Scene p_scene, LoadSceneMode p_mode)
        {
            /**
             * <(!) Suscription to SceneManager.sceneLoaded
             * Add a delegate to this to get notifications when a Scene has loaded.
             * Rather than being called directly this script code shows use of a delegate.
             * NOTE: It's necessery waiting for all components are loaded for load scene's levels.
             */
            if (p_scene.name != this.m_levels[this.m_currentLevel])
                return;
#if GAME_DEBUG
            //SCENES LOADED STATUS//
            Scene l_scene = SceneManager.GetActiveScene();
            Scene l_levelScene = SceneManager.GetSceneByName(m_levels[m_currentLevel]);
            Debug.Log($"<DEBUG>Loaded Scene qty: {SceneManager.loadedSceneCount}");
            Debug.Log($"<DEBUG>Level Scene is loaded: {l_levelScene.isLoaded}");
#endif
            this.SetupLevel();
        }

        /////////////////////////
        /// LOW-LEVEL METHODS
        ////////////////////////

        /// <summary>
        /// /Low-Level/ Enable a game object level
        /// </summary>
        /// <param name="p_levelID">Level Index</param>
        protected void Active(int p_levelID)
        {
            if (p_levelID < 0 || p_levelID > this.m_levels.Count)
            {
                Debug.LogWarning($"(!) Scene Level didn't load by out-rangue index. > {p_levelID}");
                return;
            }

            SceneManager.LoadSceneAsync(this.m_levels[p_levelID], LoadSceneMode.Additive);
            this.m_currentLevel = p_levelID;
        }

        /// <summary>
        /// /Low-Level/ Disable a game object level
        /// </summary>
        /// <param name="p_levelID">Level Index</param>
        protected void Inactive(int p_levelID)
        {
            if (p_levelID <= 0 || p_levelID > this.m_levels.Count)
            {
                Debug.LogWarning($"(!) Scene Level didn't unload by out-rangue index. > {p_levelID}");
                return;
            }

            SceneManager.UnloadSceneAsync(this.m_levels[p_levelID]);
        }


        ////////////////////////
        /// SETUPS
        ///////////////////////

        /// <summary>
        /// Config level and player data object
        /// </summary>
        /// <returns></returns>
        protected bool SetupLevel()
        {
            // Setups //
            Level l_level = GameObject.FindFirstObjectByType<Level>(); //<(i) Search playable level data: start-point, goal-point, etc.

            if (l_level == null)
            {
                Debug.LogWarning($"Not found data-level in {SceneManager.GetSceneAt(this.m_currentLevel).name} scene. The scene isn't configured!");
                return false;
            }

            this.SetupPlayer(l_level);
            this.SetupCamera(l_level);

            return true;
        }

        /// <summary>
        /// Config main camera with Level Data in Game scene follower-camera component
        /// </summary>
        protected bool SetupCamera(Level p_level)
        {
            //Load main camera bounds and position
            GameObject l_bounds = GameObject.FindWithTag("tCameraBounds");//GameObject.FindGameObjectWithTag("tCameraBounds");
            if (l_bounds == null)
            {
                Debug.LogWarning($"Not found main-camera bounds for the level in {SceneManager.GetSceneAt(this.m_currentLevel).name} scene!");
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
        protected bool SetupPlayer(Level p_level)
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
                Debug.LogWarning($"Player isn't active for the level in {SceneManager.GetSceneAt(this.m_currentLevel).name} scene. It couldn't be configured!");
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

        //////////////////////////////
        /// LEVELS MANAGER METHODS
        /////////////////////////////
        public void ReloadLevel()
        {
            this.m_reloaded = true;
            SceneManager.UnloadSceneAsync(this.m_levels[this.m_currentLevel]);
            SceneManager.LoadScene(this.m_levels[this.m_currentLevel], LoadSceneMode.Additive);
            Debug.Log("<DEBUG> RELOADED SCENE!");
        }

        /// <summary>
        /// It moves to the next level
        /// </summary>
        public bool NextLevel()
        {
            if ((this.m_currentLevel + 1) >= this.m_levels.Count)
            {
                return false;
            }

            if (this.m_currentLevel != 0) //<(e) The game scene is Game Scene = id0
                this.Inactive(this.m_currentLevel);

            this.m_currentLevel++;
            this.Active(this.m_currentLevel);
            this.m_reloaded = false;

            return true;
        }

        /// <summary>
        /// It moves to specific level
        /// </summary>
        /// <param name="p_lvlID">Index level on Level Manager</param>
        /// <returns></returns>
        public bool GoToLevel(uint p_lvlID)
        {
            if (p_lvlID > this.m_levels.Count || p_lvlID <= 0)
            {
                Debug.LogWarning($"<DEBUG> Attempt to go to out-rangue level! Out-rangue Level-ID:{p_lvlID}. Don't use ID0 because it is used by system.!");
                return false;
            }

            if (this.m_currentLevel != 0) //<(e) The game scene is Game Scene = id0
                this.Inactive(this.m_currentLevel);

            this.m_currentLevel = (int)p_lvlID;
            this.Active(this.m_currentLevel);
            this.m_reloaded = false;

            return true;
        }

        /// <summary>
        /// It moves to Main Menu Scene
        /// </summary>
        public void GoToMain()
        {
            this.GoToLevel(1);
        }

        /// <summary>
        /// It adds Report Stage Clear Scene
        /// </summary>
        public void GoToReport()
        {
            SceneManager.LoadSceneAsync("Report", LoadSceneMode.Additive);
        }

        /// <summary>
        /// It adds Report Stage Clear Scene
        /// </summary>
        public void ActiveEnding()
        {
            Scene l_scene = SceneManager.GetSceneByName("Ending");
            if (l_scene.IsValid())//<(!) It is used to don't twice active the scene
                return;

            this.Inactive(this.m_currentLevel);
            SceneManager.LoadSceneAsync("Ending", LoadSceneMode.Additive);
        }

        /// <summary>
        /// Ends the level
        /// </summary>
        public bool FinishCurrentLevel()
        {
            LevelData l_data = this.GetLevelData();
            if (l_data != null && l_data.ShowStageClear)
            {
                this.GoToReport();
                return false;
            }

            return true;
        }

        /// <summary>
        /// A simple Scene Unloader 
        /// </summary>
        /// <param name="p_name">Scene name</param>
        /// <returns>False if the scene manager didn't found the scene</returns>
        public bool UnloadSceneByName(string p_name)
        {
            Scene l_scene = SceneManager.GetSceneByName(p_name);
            if (l_scene == null)
                return false;

            SceneManager.UnloadSceneAsync(l_scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            return true;
        }

        /// <summary>
        /// A simple Scene loader 
        /// </summary>
        /// <param name="p_name">Scene name</param>
        public void LoadSceneByName(string p_name)
        {
            SceneManager.LoadSceneAsync(p_name, LoadSceneMode.Additive);
        }

        public void ActiveMain()
        {
            this.Active(1);
        }
    }
} //namespace System.Game
