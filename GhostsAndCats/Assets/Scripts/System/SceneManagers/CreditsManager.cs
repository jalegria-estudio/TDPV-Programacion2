using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    protected void Start()
    {
        GameObject l_btnBack = GameObject.Find("BtnBack");
        l_btnBack.GetComponentInChildren<Button>().GetComponent<Button>().onClick.AddListener(BackToMain);

    }

    public void BackToMain()
    {
        SceneManager.UnloadSceneAsync("Credits", UnloadSceneOptions.None);
    }
}
