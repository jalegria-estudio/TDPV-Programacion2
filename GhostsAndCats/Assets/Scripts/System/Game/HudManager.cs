using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    ///////////////////////////////
    /// HUD CANVAS
    /////////////////////////////
    [Header("HUD CANVAS")]
    [SerializeField] Text m_goalCounter = null;
    [SerializeField] Text m_goalRequire = null;
    [SerializeField] Text m_soulsCounter = null;
    [SerializeField] Text m_lifesCounter = null;
    [SerializeField] Text m_tunasCounter = null;

    public Text GoalCounter { get => m_goalCounter; set => m_goalCounter = value; }
    public Text GoalRequire { get => m_goalRequire; set => m_goalRequire = value; }

    protected PlayerData m_playerData = null;
    protected LevelData m_levelData = null;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject l_player = GameObject.FindWithTag("tPlayer");
        ////<(i) Source: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/GameObject.TryGetComponent.html
        //Debug.Assert(l_player != null, "<DEBUG ASSERTION> Player not found!");

        //if (l_player.TryGetComponent<Player>(out Player l_playerScript))
        //{
        //    m_playerData = l_playerScript.Data;
        //}

        //if (l_player.TryGetComponent<ExperienceManager>(out ExperienceManager l_manager))
        //{
        //    m_levelData = l_manager.LevelData;
        //}
        //m_goalCounter = GameObject.Find("GoalPanel").chi;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateSoul();
    }
    public void UpdateData(PlayerData p_plyData, LevelData p_lvlData)
    {
        if (p_plyData == null || p_lvlData == null)
        {
            Debug.LogWarning($"<DEBUG> Hud Data doesn't update because some source is null! PlayerData: {p_plyData} LevelData: {p_lvlData}");
            return;
        }

        UpdateGoal(p_plyData.Souls, p_lvlData.m_requiredSoulsQty);
        UpdateStatus(p_plyData.Lifes, p_plyData.Tunas, p_plyData.Souls);
    }
    protected void UpdateStatus(int p_lifes, int p_tunas, int p_souls)
    {
        m_lifesCounter.text = string.Format("{0:D3}", p_lifes);
        m_tunasCounter.text = string.Format("{0:D3}", p_tunas);
        m_soulsCounter.text = string.Format("{0:D3}", p_souls);
    }

    protected void UpdateGoal(int p_qty, int p_require)
    {
        if (p_qty <= p_require)
        {
            m_goalCounter.text = string.Format("{0:D2}", p_qty);
            m_goalRequire.text = string.Format("{0:D2}", p_require);
        }

    }
}
