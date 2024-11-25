using System.Game;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Opening Controller
/// </summary>
public class OpeningManager : MonoBehaviour
{
    [SerializeField] protected PlayableDirector m_director = null;
    GameObject m_opening = null;
    GameObject m_btnSkip = null;

    // Start is called before the first frame update
    void Start()
    {
        this.m_opening = GameObject.Find("Opening");

        /// Suscription to PlayableDirector Event: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Playables.PlayableDirector-stopped.html
        Debug.Assert(this.m_director != null, "Director Opening Component is null!");
        this.m_director.stopped += this.OnStopped;

        //SKIP BUTTON CONFIG
        this.m_btnSkip = GameObject.FindWithTag("tSkipButton");
        this.m_btnSkip.GetComponent<Button>().onClick.AddListener(this.OnStop);

        this.Play();
    }

    /// <summary>
    /// Start a opening animation timeline
    /// </summary>
    public void Play()
    {
        //Instantiate(m_opening);
        this.gameObject.SetActive(true);
        Debug.Assert(this.m_director != null, "<DEBUG ASSERT> Playable Director isn't allocated on Play()!");

        this.m_director.Play();
    }

    /// <summary>
    /// Stop opening if it's playing
    /// </summary>
    public void OnStop()
    {
        Debug.Assert(this.m_director != null, "<DEBUG ASSERT> Playable Director isn't allocated on OnStop()!");

        if (this.m_director.state == PlayState.Playing)
            this.m_director.Stop();
    }

    /// <summary>
    /// Callback when the opening animation is stopped
    /// </summary>
    /// <param name="p_director"></param>
    public void OnStopped(PlayableDirector p_director)
    {
        GameManager.Instance.GamePlay();
        SceneManager.UnloadSceneAsync("Opening", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);//<(i) Unload
    }
}
