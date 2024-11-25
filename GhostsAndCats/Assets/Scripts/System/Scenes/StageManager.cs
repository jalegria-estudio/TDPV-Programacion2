#define GAME_DEBUG
#undef GAME_DEBUG

using System.Collections;
using System.Game;
using Toolbox;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for Stage-Round screen
/// </summary>
public class StageManager : MonoBehaviour
{
    protected LevelData m_data;

    // Start is called before the first frame update
    void Start()
    {
#if GAME_DEBUG
        Debug.Log("STAGE-ROUND ON START");
#endif
        this.m_data = GameManager.Instance.LevelManager.GetLevelData();

        if (this.m_data != null && this.m_data.AudioClipStageRound != null && GameObject.Find("AudioSource").TryGetComponent<AudioSource>(out AudioSource l_jukebox))
        {
            l_jukebox.PlayOneShot(this.m_data.AudioClipStageRound);
        }

        this.StartCoroutine(this.OnLoadedReport());
    }

    /// <summary>
    /// Main Callback to show report
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLoadedReport()
    {
        if (this.m_data.ShowStageRound)
        {
            this.SetRoundTexts(this.m_data);
        }

        GameManager.Instance.Player.DisableActions();
        yield return new WaitForSeconds(this.m_data.WaitTime);

        SceneManager.UnloadSceneAsync("Report");
#if GAME_DEBUG
        Debug.Log("<Debug> STAGE-ROUND ON-LOADED-REPORT WAS FINISHED BEFORE START GAMEPLAY");
#endif
        GameManager.Instance.LevelManager.GetLevel().StartGameplay();
#if GAME_DEBUG
        Debug.Log("<Debug> STAGE-ROUND ON-LOADED-REPORT WAS FINISHED");
#endif
    }

    /// <summary>
    /// Set level data in Components of Stage-Clear Report Scene 
    /// </summary>
    /// <param name="l_lvlData"></param>
    public void SetRoundTexts(LevelData l_lvlData)
    {
        string l_round = "Stage " + l_lvlData.m_level + "-" + l_lvlData.m_sublevel;
        CanvasTools.ReplaceText("Title", l_round);
        CanvasTools.ReplaceText("Subtitle", l_lvlData.m_name);
    }
}
