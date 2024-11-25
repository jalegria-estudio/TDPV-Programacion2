using System.Game;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    ///////////////////////////////
    /// HUD CANVAS
    /////////////////////////////
    [Header("HUD CANVAS STATUS")]
    [SerializeField] Text m_goalCounter = null;
    [SerializeField] Text m_goalRequire = null;
    [SerializeField] Text m_soulsCounter = null;
    [SerializeField] Text m_lifesCounter = null;
    [SerializeField] Text m_tunasCounter = null;
    [SerializeField] Text m_timer = null;
    [SerializeField] Text m_score = null;
    [SerializeField] Text m_hiscore = null;
    public Text GoalCounter { get => this.m_goalCounter; set => this.m_goalCounter = value; }
    public Text GoalRequire { get => this.m_goalRequire; set => this.m_goalRequire = value; }

    protected PlayerData m_playerData = null;
    protected LevelData m_levelData = null;

    // Start is called before the first frame update
    void Start()
    {
        this.Reset();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Update hud with data from level and player
    /// </summary>
    /// <param name="p_plyData">Scriptable Data</param>
    /// <param name="p_lvlData">Scriptable Data</param>
    public void UpdateData(PlayerData p_plyData, LevelData p_lvlData)
    {
        if (p_plyData == null || p_lvlData == null)
        {
            Debug.LogWarning($"<DEBUG> Hud Data doesn't update because some source is null! PlayerData: {p_plyData} LevelData: {p_lvlData}");
            return;
        }

        this.UpdateGoal(p_plyData.Souls, p_lvlData.m_requiredSoulsQty);
        this.UpdateStatus(p_plyData.Lifes, p_plyData.Tunas, p_plyData.Souls);
        this.UpdateTime();
        this.UpdateScore(p_plyData.Score, p_plyData.HiScore);
    }

    /// <summary>
    /// Update player status
    /// </summary>
    protected void UpdateStatus(int p_lifes, int p_tunas, int p_souls)
    {
        this.m_lifesCounter.text = string.Format("{0:D3}", p_lifes);
        this.m_tunasCounter.text = string.Format("{0:D3}", p_tunas);
        this.m_soulsCounter.text = string.Format("{0:D3}", p_souls);
    }

    /// <summary>
    /// Update goal stage items require status
    /// </summary>
    protected void UpdateGoal(int p_qty, int p_require)
    {
        if (p_qty <= p_require)
        {
            this.m_goalCounter.text = string.Format("{0:D2}", p_qty);
            this.m_goalRequire.text = string.Format("{0:D2}", p_require);
        }

    }

    /// <summary>
    /// Update time status
    /// </summary>
    protected void UpdateTime()
    {
        if (GameManager.Instance.TimerService == null)
            return;

        this.m_timer.text = string.Format("{0:D3}", (int)GameManager.Instance.TimerService.RemainingTime);
    }

    /// <summary>
    /// Update score status
    /// </summary>
    protected void UpdateScore(int p_score, int p_hiscore)
    {
        this.m_score.text = string.Format("{0:D6}", p_score);
        this.m_hiscore.text = string.Format("{0:D6}", p_hiscore);
    }

    /// <summary>
    /// Reset counters in game
    /// </summary>
    protected void Reset()
    {
        this.m_lifesCounter.text = string.Format("{0:D3}", 0);
        this.m_tunasCounter.text = string.Format("{0:D3}", 0);
        this.m_soulsCounter.text = string.Format("{0:D3}", 0);
        this.m_goalCounter.text = string.Format("{0:D2}", 0);
        this.m_goalRequire.text = string.Format("{0:D2}", 0);
    }

}
