using UnityEngine;
using UnityEngine.UI;

namespace System.Game.States
{
    public enum GameOverMode
    {
        NONE,
        GAME_OVER_WIN,
        GAME_OVER_LOSE
    }

    class StateGameOver : IGameState
    {
        protected readonly GameManager m_gameManager = null;
        public GameOverMode m_mode = GameOverMode.NONE;

        public GameOverMode Mode { get => m_mode; set => m_mode = value; }

        public StateGameOver(GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
            m_mode = GameOverMode.NONE;
        }

        // Logic that runs when enter to the state
        public void Enter()
        {
            GameObject.FindGameObjectWithTag("tPlayer").SetActive(false);

            Debug.Log("GAME OVER!");
            switch (m_mode)
            {
                case GameOverMode.NONE:
                    break;
                case GameOverMode.GAME_OVER_WIN:
                    SetTextCanvas("You win!");
                    Debug.Log("YOU WIN!");
                    break;
                case GameOverMode.GAME_OVER_LOSE:
                    SetTextCanvas("Game Over");
                    Debug.Log("YOU LOSE!");
                    break;
                default:
                    break;
            }
        }

        // Logic that runs when exit from the state
        public void Exit()
        {
        }

        // Fixed coded logic runs every frame. Note: include here a condition to transition to a new state
        public void Update()
        {

        }
        // Gener
        protected void SetTextCanvas(string p_text)
        {
            // Canvas Object
            // Element that can be used for screen rendering.
            // Elements on a canvas are rendered AFTER Scene rendering, either from an attached camera or using overlay mode.
            // Source: https://docs.unity3d.com/ScriptReference/Canvas.html
            GameObject l_gameObject = new GameObject();
            l_gameObject.name = "GameOver";
            l_gameObject.AddComponent<Canvas>();

            Canvas l_canvas = l_gameObject.GetComponent<Canvas>();
            l_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            l_gameObject.AddComponent<CanvasScaler>();//The Canvas Scaler component is used for controlling the overall scale and pixel density of UI elements in the Canvas. This scaling affects everything under the Canvas, including font sizes and image borders.
            l_gameObject.AddComponent<GraphicRaycaster>();//A derived BaseRaycaster to raycast against Graphic elements.

            GameObject l_textObject = new GameObject();
            l_textObject.transform.parent = l_gameObject.transform;
            l_textObject.name = "GameOverText";

            UnityEngine.UI.Text l_text = l_textObject.AddComponent<UnityEngine.UI.Text>();
            l_text.text = p_text;
            l_text.font = UnityEngine.Resources.Load<Font>("Fonts/BlackAndWhitePicture-Regular"); //<(!) all-fonts resources must be in Assets/Resources to be load with UnityEngine.Resources.Load()
            l_text.fontSize = 100;

            // Text position
            RectTransform l_rectTransform = l_text.GetComponent<RectTransform>();
            l_rectTransform.localPosition = Vector3.zero;
            l_rectTransform.sizeDelta = new Vector2(400, 200);

            GameObject.FindGameObjectWithTag("tGameOverText");

        }
    }
}//namespace System.Game.States
