using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A simple Navigator for scenes with main menu
/// </summary>
public class NavigateManager : MonoBehaviour
{
    protected void Start()
    {
        GameObject l_btnBack = GameObject.Find("BtnBack");
        l_btnBack.GetComponentInChildren<Button>().GetComponent<Button>().onClick.AddListener(this.BackToMain);

    }

    /// <summary>
    /// Callback to back
    /// </summary>
    public void BackToMain()
    {
        if (SceneManager.GetSceneByName("Credits").IsValid())
            SceneManager.UnloadSceneAsync("Credits", UnloadSceneOptions.None);
        else if (SceneManager.GetSceneByName("Help").IsValid())
            SceneManager.UnloadSceneAsync("Help", UnloadSceneOptions.None);
    }
}
