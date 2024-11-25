using System.Collections;
using System.Game;
using System.Game.States;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game Ending Controller
/// </summary>
public class EndingManager : MonoBehaviour
{
    [Header("ENDING CONFIGURATION")]
    [Header("References")]
    [Tooltip("Background image reference")]
    [SerializeField] protected Image m_room;
    [Tooltip("Tittle text to show")]
    [SerializeField] protected Text m_title;
    [Tooltip("Subtittle text to show")]
    [SerializeField] protected Text m_subtitle;
    [Tooltip("History text reference component")]
    [SerializeField] protected Text m_end;
    [Tooltip("History image reference")]
    [SerializeField] protected Image m_picture;
    [Header("Background")]
    [Tooltip("Background sprite picture - generic")]
    [SerializeField] protected Sprite m_bgdDefault;
    [Tooltip("Background sprite picture when losing gameplay")]
    [SerializeField] protected Sprite m_bgdGameOver;
    [Tooltip("Background sprite picture when winning gameplay")]
    [SerializeField] protected Sprite m_bgdGameWin;
    [Header("Titles")]
    [Tooltip("Text Color")]
    [SerializeField] protected Color m_textColorWin;
    [Tooltip("Text Color")]
    [SerializeField] protected Color m_textColorLose;
    [Header("Music")]
    [Tooltip("Audio music to play")]
    [SerializeField] protected AudioClip m_sfxDefault;
    //[SerializeField] protected AudioClip m_sfxWin;
    //[SerializeField] protected AudioClip m_sfxLose;
    [Tooltip("Sprite picture when losing gameplay")]
    [Header("History")]
    [SerializeField] protected Sprite m_gfxWin;
    [Tooltip("String history to show when wining gameplay")]
    [SerializeField] protected string m_txtWin;
    [Tooltip("Sprite picture when wining gameplay")]
    [SerializeField] protected Sprite m_gfxLose;
    [Tooltip("String history to show when losing gameplay")]
    [SerializeField] protected string m_txtLose;

    // Start is called before the first frame update
    void Start()
    {
        this.SetupEnding();
        this.StartCoroutine(this.Exit());
    }

    /// <summary>
    /// Config ending stuffs
    /// </summary>
    protected void SetupEnding()
    {
        StateGameOver l_state = (StateGameOver)GameManager.Instance.State;
        GameOverMode l_mode = l_state.Mode;
        ///STUFFS ENDING ELEMENTS
        string l_subtitle = "NONE";
        Sprite l_bgd = this.m_bgdDefault;
        Color l_subtitleColor = Color.white;
        AudioClip l_sfxMusic = this.m_sfxDefault;
        Sprite l_historyGfx = null;
        string l_history = "";

        ///SWITCHER
        switch (l_mode)
        {
            case GameOverMode.NONE:
                break;
            case GameOverMode.GAME_OVER_WIN:
                l_subtitle = "You win!";
                l_bgd = this.m_bgdGameWin;
                l_subtitleColor = this.m_textColorWin;
                l_history = this.m_txtWin;
                l_historyGfx = this.m_gfxWin;
                break;
            case GameOverMode.GAME_OVER_LOSE:
                l_subtitle = "You lose...";
                l_bgd = this.m_bgdGameOver;
                l_subtitleColor = this.m_textColorLose;
                l_history = this.m_txtLose;
                l_historyGfx = this.m_gfxLose;
                break;
            default:
                break;
        }

        this.UpgradeSceneData(l_subtitle, l_history, l_historyGfx, l_bgd, l_sfxMusic);
    }

    /// <summary>
    /// Exit Corrutine
    /// </summary>
    protected IEnumerator Exit()
    {
        yield return new WaitForSeconds(16.0f);
        GameManager.Instance.LevelManager.UnloadSceneByName("Ending");
        GameManager.Instance.LevelManager.ActiveMain();
    }

    /// <summary>
    /// Setup ending components data
    /// </summary>
    protected void UpgradeSceneData(string p_subtitle, string p_history, Sprite p_historyPicture, Sprite p_background, AudioClip p_music)
    {
        this.m_subtitle.text = p_subtitle;
        //this.m_subtitle.color = l_color;
        this.m_end.text = p_history;
        this.m_picture.sprite = p_historyPicture;
        this.m_room.sprite = p_background;
        GameManager.Instance.AudioService.PlayOneShot(p_music);
    }
}
