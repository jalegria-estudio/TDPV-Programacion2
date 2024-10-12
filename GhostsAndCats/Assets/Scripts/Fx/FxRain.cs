using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxRain : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected AudioSource m_jukebox = null;
    [SerializeField] protected float m_startTime = 0.0f;
    [SerializeField] protected float m_duration = 0.0f;
    protected ParticleSystem m_rainGfx = null;
    protected float m_lapsetime = 0.0f;
    protected float m_rainSfxVolume = 1;
    protected bool m_isRaining = false;
    public bool IsRaining { get => m_isRaining; set => m_isRaining = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_rainGfx = this.GetComponent<ParticleSystem>();
        var particleSystemSettings = m_rainGfx.main; //Access the main Particle System settings.
        particleSystemSettings.playOnAwake = false;
        m_jukebox.playOnAwake = false;
        m_jukebox.Stop();
        m_rainSfxVolume = m_jukebox.volume;
        m_jukebox.volume = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_lapsetime += Time.fixedDeltaTime;

        if (m_lapsetime >= m_startTime && !m_rainGfx.isPlaying)
        {
            this.GetComponent<ParticleSystem>().Play();
            m_jukebox?.Play();
            m_isRaining = true;
        }

        if (m_jukebox.isPlaying)
        {
            FadeInUpdaterVolume();
        }

        if (m_lapsetime >= m_startTime + m_duration)
        {
            this.GetComponent<ParticleSystem>().Stop();
            m_jukebox?.Stop();
            m_isRaining = false;
        }
    }

    void FadeInUpdaterVolume()
    {
        if (m_jukebox.volume < m_rainSfxVolume)
        {
            m_jukebox.volume += 0.01f;
        }
    }

    void FadeOutUpdaterVolume()
    {
        if (m_jukebox.volume > 0.0f)
        {
            m_jukebox.volume -= 0.01f;
        }
    }
}
