using System.Collections;
using System.Collections.Generic;
using System.Game;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] protected PlayableDirector m_director = null;
    GameObject m_opening = null;

    // Start is called before the first frame update
    void Start()
    {
        m_opening = GameObject.Find("Opening");
        m_director = m_opening.GetComponentInChildren<PlayableDirector>();
        //TimelineAsset l_asset = (TimelineAsset)m_director.playableAsset;

        //foreach (var track in l_asset.GetOutputTracks())//A PlayableAsset representing a track inside a timeline
        //{
        //    if (track.GetType() == typeof(SignalTrack))
        //    {
        //        SignalTrack l_track = (SignalTrack)track;
        //    }
        //}

        /// Suscription to PlayableDirector Event: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Playables.PlayableDirector-stopped.html
        m_director.stopped += OnStopped;

        Play();
    }

    protected void Update()
    {
        //if (m_director.state != PlayState.Playing)
        //    Unload();
    }

    /// <summary>
    /// Start a opening animation timeline
    /// </summary>
    public void Play()
    {
        //Instantiate(m_opening);
        gameObject.SetActive(true);
        //PlayableDirector l_director = GameObject.FindObjectOfType<PlayableDirector>();
        if (m_director == null)
        {
            Debug.LogWarning("<DEBUG> Playable Director isn't allocated!");
            return;
        }

        m_director.Play();
    }

    /// <summary>
    /// Stop opening if it's playing
    /// </summary>
    public void Stop()
    {
        //Debug.Log(m_opening.GetComponentInChildren<PlayableDirector>().state);
        //if (m_opening.GetComponentInChildren<PlayableDirector>().state == PlayState.Playing)
        //    m_opening.GetComponentInChildren<PlayableDirector>().Stop();
        //m_opening.SetActive(false);
        if (m_director == null)
        {
            Debug.LogWarning("<DEBUG> Playable Director isn't allocated!");
            return;
        }

        if (m_director.state == PlayState.Playing)
            m_director.Stop();
    }

    public void LoadLevel()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().MoveNextLevel();
        Debug.Log("AVISO LEVEL");
    }

    public void OnStopped(PlayableDirector p_director)
    {
        //<(i) Unload
        SceneManager.UnloadSceneAsync("Opening", UnloadSceneOptions.None);
        GameObject.Find("GameManager").GetComponent<GameManager>().GamePlay();
    }
}
