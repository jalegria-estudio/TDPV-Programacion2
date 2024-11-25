using System.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A simple menu controller
/// </summary>
public class MenuManager : MonoBehaviour
{
    GameObject m_btnStart = null;
    GameObject m_btnCredits = null;
    GameObject m_btnHelp = null;

    // Start is called before the first frame update
    void Start()
    {
        this.m_btnStart = GameObject.FindWithTag("tStartButton");
        this.m_btnStart.GetComponent<Button>().onClick.AddListener(this.HandleStartBtn);

        this.m_btnCredits = GameObject.FindWithTag("tCreditsButton");
        this.m_btnCredits.GetComponent<Button>().onClick.AddListener(this.HandleCreditsBtn);

        this.m_btnHelp = GameObject.FindWithTag("tHelpButton");
        this.m_btnHelp.GetComponent<Button>().onClick.AddListener(this.HandleHelpBtn);
    }

    /// <summary>
    /// Callback to view game credits
    /// </summary>
    protected void HandleCreditsBtn()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Callback to start the gameplay
    /// </summary>
    protected void HandleStartBtn()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GameStart();
    }

    /// <summary>
    /// Callback to view game help
    /// </summary>
    protected void HandleHelpBtn()
    {
        SceneManager.LoadScene("Help", LoadSceneMode.Additive);
    }
}
