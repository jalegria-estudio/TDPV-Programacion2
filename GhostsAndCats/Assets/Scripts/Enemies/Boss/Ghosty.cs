using Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ghosty : MonoBehaviour
{
    ///// EVENTS /////
    public event System.Action EVT_DEFEAT;

    //////////////////////////
    /// ATTRIBUTES
    /////////////////////////
    protected Vector2[] m_positions = new Vector2[4];
    protected uint m_index = 0;
    protected Animator m_animator = null;

    public uint IndexPos { get => m_index; set => m_index = value; }

    public void UpdatePos()
    {
        Vector3 l_targetPos = m_positions[m_index];
        l_targetPos.z = transform.position.z;
        gameObject.transform.position = l_targetPos;
    }

    ////////////////////////////////////////////////////////////////////////
    /// OBJECT POOL PATTERN ATTRIBUTE
    /// Source: Level-Up Your Code With Game Programming Patterns. p.49;
    //////////////////////////////////////////////////////////////////////
    protected Boss m_pool;
    public Boss Pool { get => m_pool; set => m_pool = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.TryGetComponent<Animator>(out m_animator);

        m_positions[0] = new Vector2(7, -7);
        m_positions[1] = new Vector2(-7, 2);
        m_positions[2] = new Vector2(-7, -7);
        m_positions[3] = new Vector2(7, 2);

        UpdatePos();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Invoke("Scare", 0);
        AnimatorStateInfo l_info = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (l_info.IsName("Appear") && l_info.normalizedTime < 1.0f)
        {
            return;
        }

        if ((Vector2)transform.position == m_positions[m_index])
        {
            m_index = (m_index + 1 == m_positions.Length) ? 0 : m_index + 1;
        }

        Scare();

    }

    /// <summary>
    /// Moves the Ghosty across the scenes
    /// </summary>
    protected void Scare()
    {
        float l_step = Time.deltaTime * 5.0f;
        Vector3 l_targetPos = m_positions[m_index];
        l_targetPos.z = transform.position.z;
        this.gameObject.transform.position = Vector3.MoveTowards(transform.position, l_targetPos, l_step);
    }

    /// <summary>
    /// Enable/Disable the sprite's box collider
    /// </summary>
    protected void ToggleBoxCollider()
    {
        if (this.gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D l_boxCollider))
        {
            l_boxCollider.enabled = !l_boxCollider.enabled;
        }
    }

    /// <summary>
    /// Actions when the ghosty is defeated
    /// </summary>
    public void OnDefeat()
    {
        Debug.Log("COLLISION ON DEFEAT!");
        AnimeDefeat();
        EVT_DEFEAT?.Invoke();
    }

    /// <summary>
    /// Animation handler when ghosty is defeated
    /// </summary>
    public void AnimeDefeat()
    {
        ToggleBoxCollider();
        m_animator.SetTrigger("pDefeated");
    }

    /// <summary>
    /// Actions after the ghosty is defeated
    /// </summary>
    public void OnDisappear()
    {
        ToggleBoxCollider();
        Dive();
    }

    /////////////////////////////////
    /// OBJECT POOL PATTERN METHODS
    ////////////////////////////////

    /// <summary>
    /// Return this instance object to Boss's pool
    /// </summary>
    public void Dive()
    {
        //m_pool.GetComponent<Animator>().SetTrigger("pRecover");
        m_pool.ReturnToPool(this);
    }
}
