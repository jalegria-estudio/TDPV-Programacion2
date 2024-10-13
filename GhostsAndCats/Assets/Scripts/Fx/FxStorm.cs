using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage and synchronize RainFX and ThunderFX
/// </summary>
public class FxStorm : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected FxRain m_rain = null;
    [SerializeField] protected FxThunder m_thunder = null;
    protected bool m_startedStorm = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_startedStorm && m_rain.IsRaining)
        {
            m_startedStorm = true;
        }
        else if (m_startedStorm && !m_rain.IsRaining)
        {
            m_thunder.Stop();
            m_startedStorm = false;

        }
    }
}
