using Managers;
using System;
using System.Collections.Generic;
using System.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GameObject m_btnStart = null;
    GameObject m_btnCredits = null;

    // Start is called before the first frame update
    void Start()
    {
        m_btnStart = GameObject.FindWithTag("tStartButton");
        m_btnStart.GetComponent<Button>().onClick.AddListener(HandleStartBtn);

        m_btnCredits = GameObject.FindWithTag("tCreditsButton");
        m_btnCredits.GetComponent<Button>().onClick.AddListener(HandleCreditsBtn);
    }

    protected void HandleCreditsBtn()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }

    protected void HandleStartBtn()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GameStart();
    }
}
