using System.Collections;
using UnityEngine;

/// <summary>
/// Rain FX Script
/// </summary>
[RequireComponent(typeof(Camera))]
public class FxThunder : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected Camera m_camera = null;
    [SerializeField] protected AudioSource m_jukebox = null;
    [SerializeField] protected AudioClip m_thunderSfx = null;
    [Range(0.0f, 15.0f)]
    [SerializeField] protected float m_lapse = 0.0f;
    protected float m_lapseDefault = 0.0f;
    [SerializeField] protected Color m_color = new Color(0, 0, 0, 1.0f);
    protected Color m_colorDefault = new Color();
    protected float m_elapseTime = 0.0f;
    protected bool m_isThundering = false;
    [SerializeField] bool m_autoPlay = true;
    [SerializeField] bool m_random = true;
    protected int m_randomMultiplier = 3;

    /// <summary>
    /// Indicate if ThunderFX is active
    /// </summary>
    public bool IsThundering { get => m_isThundering; protected set => m_isThundering = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_colorDefault = m_camera.backgroundColor;
        m_camera.clearFlags = CameraClearFlags.SolidColor;

        if (m_autoPlay)
            Play();

        m_lapseDefault = m_lapse;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_isThundering)
            return;

        m_elapseTime += Time.deltaTime;

        if (m_elapseTime >= m_lapse)
        {
            //<(i) First thunder
            StartCoroutine(CoThrowThunder());
            m_elapseTime = 0.0f;

            //<(i) Second sfx
            if (m_thunderSfx != null)
                m_jukebox?.PlayOneShot(m_thunderSfx);

            if (m_random)
                m_lapse = Random.Range(m_lapseDefault, m_lapseDefault * m_randomMultiplier);
        }
    }

    /// <summary>
    /// Coroutine to throw a thunder GFX
    /// </summary>
    /// <returns></returns>
    IEnumerator CoThrowThunder()
    {
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
    }

    /// <summary>
    /// Execute a Thunder GFX
    /// </summary>
    protected void ThrowThunder()
    {
        m_camera.backgroundColor = m_camera.backgroundColor == m_colorDefault ? m_color : m_colorDefault;
    }

    /// <summary>
    /// Iterative play the thunder FX 
    /// </summary>
    public void Play()
    {
        m_isThundering = true;
        m_elapseTime = 0.0f;
    }

    /// <summary>
    /// Stop the thunder FX
    /// </summary>
    public void Stop()
    {
        m_isThundering = false;
    }
}
