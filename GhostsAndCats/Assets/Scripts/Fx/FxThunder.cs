using System.Collections;
using UnityEngine;

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
    public bool IsThundering { get => m_isThundering; set => m_isThundering = value; }

    // Start is called before the first frame update
    void Start()
    {
        //m_camera = GetComponent<Camera>();
        m_colorDefault = m_camera.backgroundColor;
        m_camera.clearFlags = CameraClearFlags.SolidColor;
        //m_camera.backgroundColor = m_color;
        //StartCoroutine(CoThrowThunder());
        //InvokeRepeating(nameof(ThrowThunder), 1.0f, 0.1f);
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
                m_lapse = Random.Range(m_lapseDefault, m_lapseDefault * 3);
        }

        //print(m_elapseTime);
        //m_camera.clearFlags = CameraClearFlags.SolidColor;

    }

    IEnumerator CoThrowThunder()
    {
        //m_camera.backgroundColor = m_color;
        //yield return new WaitForSeconds(.1f);
        //m_camera.backgroundColor = m_colorDefault;
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
        yield return new WaitForSeconds(.1f);
        ThrowThunder();
    }
    protected void ThrowThunder()
    {
        m_camera.backgroundColor = m_camera.backgroundColor == m_colorDefault ? m_color : m_colorDefault;
        //m_camera.backgroundColor = m_colorDefault;
    }

    public void Play()
    {
        m_isThundering = true;
        m_elapseTime = 0.0f;
    }

    public void Stop()
    {
        m_isThundering = false;
    }
}
