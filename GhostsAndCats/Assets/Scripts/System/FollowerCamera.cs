/**
 * @file Follower Camera Script - Include my library tools for Unity-C# XD
 * @version 0.1
 * @category Utils - Script!
 * @brief A simple script to follow a gameObject
 * @details Cinemachine es un excelente y profesional modulo que lleva a una compleja forma
 *          de resolver que una camara siga a un jugador. Tiene un furniture's overload
 *          de configuraciones y funciones para algo simple. Es por ello que a veces es mejor
 *          escribir sus propias soluciones que usar una herramienta compleja solo para usarla
 *          en hacer algo simple.
 *          Nota: Encima el cinemachie-confiner no usa box collider! XD
 *  Aqui incluyo mis herramientas para incluirla en mis proyectos. En este caso Unity.
 * @author: Juan P. Alegria
 * @date 2024-10-05
 */

using UnityEngine;

/// <summary>
/// A simple script to follow a gameObject
/// </summary>
public class FollowerCamera : MonoBehaviour
{
    ///// CONFIG INSPECTOR /////
    [Header("Configuration")]
    [SerializeField] bool m_turnOn = true;
    [SerializeField] GameObject m_objetive = null;
    [SerializeField] Camera m_camera = null;
    [SerializeField] Collider2D m_bounds = null;
    public bool TurnOn { get => m_turnOn; set => m_turnOn = value; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(m_camera.orthographic, "<DEBUG ASSERT> The Main Camera must be a orthographic camera!");
        if (m_bounds == null)
            Debug.LogWarning("<DEBUG> The Follower Camera needs bounds!");
        //m_camera = gameObject.GetComponent<Camera>();
        //m_camera.orthographic = true; //Force orthographic camera 
        //m_camera.transform.Translate(m_objetive.transform.position);
        TranslateCamera();
    }

    /**
     * LateUpdate()
     * LateUpdate is called once per frame, after Update has finished. 
     * Any calculations performed in Update will have completed when LateUpdate begins.
     * A common use for LateUpdate would be a following third-person camera. 
     * If you make your character move and turn inside Update, you can perform all camera
     * movement and rotation calculations in LateUpdate.
     * This will ensure that the character has moved completely before the camera tracks its position.
     * Source: https://docs.unity3d.com/Manual/ExecutionOrder.html
     */
    void LateUpdate()
    {
        TranslateCamera();
        //ShowDebugData();
    }

    /// <summary>
    /// Translate the camera position
    /// </summary>
    protected void TranslateCamera()
    {
        if (!m_turnOn)
            return;

        float l_viewportHeightHalfSize = m_camera.orthographicSize;
        float l_viewportWidthHalfSize = m_camera.orthographicSize * m_camera.aspect;//<(e) Es la inversa del aspect ratio =>  The aspect ratio (width divided by height).
        Vector3 l_cameraPos = m_camera.transform.position;
        Vector2 l_objetivePos = m_objetive.transform.position;

        if (m_bounds == null)
        {
            l_cameraPos = l_objetivePos;
            m_camera.transform.position = l_cameraPos;
            return;
        }

        //<(e) Only It updates camera-pos if the viewport (with a new pos) doesn't touch a limit-camera-bounds
        /// Horizontal Limit
        if (
            (l_objetivePos.x + l_viewportWidthHalfSize) < m_bounds.bounds.max.x &&
            (l_objetivePos.x - l_viewportWidthHalfSize) > m_bounds.bounds.min.x
        )
        {
            l_cameraPos.x = l_objetivePos.x;
        }

        /// Vertical Limit
        if (
            (l_objetivePos.y - l_viewportHeightHalfSize) > m_bounds.bounds.min.y &&
            (l_objetivePos.y + l_viewportHeightHalfSize) < m_bounds.bounds.max.y
         )
        {
            l_cameraPos.y = l_objetivePos.y;
        }

        /// Update Pos
        m_camera.transform.position = l_cameraPos;
    }

    /// <summary>
    /// Show debug data only
    /// </summary>
    protected void ShowDebugData()
    {
        /**
        * Viewport formula
        * Orthographic Size (vertical size of camera view)
        * The orthographicSize is half the size of the vertical viewing volume.
        * The horizontal size of the viewing volume depends on the aspect ratio.
        * Orthographic size is ignored when the camera is not orthographic.
        * Source: https://docs.unity3d.com/ScriptReference/Camera-orthographicSize.html
        * 
        *  Vertical Size => m_camera.orthographicSize * 2;
        *  Horizontal Size => m_camera.aspect * vertical_size.
        */
        //<(i) When ortho is true, camera's viewing volume is defined by orthographicSize.
        float l_cameraSize = m_camera.orthographicSize;
        int l_ppu = m_camera.GetComponent<UnityEngine.U2D.PixelPerfectCamera>().assetsPPU;
        float l_viewportHeight = l_cameraSize * 2;
        float l_viewportWidth = l_viewportHeight * m_camera.aspect;
        //(!) NOTAR QUE LA RESOLUCION DEL VIEWPORT FINAL ES LA RESOLUCION DE REFERENCIA DE PIXEL PERFECT SCRIPT => green rect
        Debug.Log($"Viewport Size => Width: {l_viewportWidth * l_ppu} Height: {l_viewportHeight * l_ppu}");
        Debug.Log($"Camera Rectangle => {m_camera.rect}");
    }

    /// <summary>
    /// Set a new camera bounds
    /// </summary>
    /// <param name="p_cameraBounds"></param>
    public void SetBounds(Collider2D p_cameraBounds)
    {
        m_camera.gameObject.SetActive(false); //<(!) Reset camera view
        m_bounds = p_cameraBounds;
        m_camera.gameObject.SetActive(true);

    }

    /// <summary>
    /// Set a camera position
    /// </summary>
    /// <param name="p_cameraBounds"></param>
    public void SetCameraPosition(Vector3 p_cameraPos)
    {
        m_camera.gameObject.SetActive(false); //<(!) Reset camera view
        m_camera.transform.position = p_cameraPos;
        m_camera.gameObject.SetActive(true);
    }

    /// <summary>
    /// Set a camera position
    /// </summary>
    /// <param name="p_cameraBounds"></param>
    public void SetCameraPosition(Vector2 p_cameraPos)
    {
        m_camera.gameObject.SetActive(false); //<(!) Reset camera view
        m_camera.transform.position = new Vector3(p_cameraPos.x, p_cameraPos.y, m_camera.transform.position.z);
        m_camera.gameObject.SetActive(true);
    }
}
