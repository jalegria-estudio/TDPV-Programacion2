using UnityEngine;

/// <summary>
/// Boss Minion Enemy Class
/// </summary>
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

    public uint IndexPos { get => this.m_index; set => this.m_index = value; }

    public void UpdatePos()
    {
        Vector3 l_targetPos = this.m_positions[this.m_index];
        l_targetPos.z = this.transform.position.z;
        this.gameObject.transform.position = l_targetPos;
    }

    ////////////////////////////////////////////////////////////////////////
    /// OBJECT POOL PATTERN ATTRIBUTE
    /// Source: Level-Up Your Code With Game Programming Patterns. p.49;
    //////////////////////////////////////////////////////////////////////
    protected Boss m_pool;
    public Boss Pool { get => this.m_pool; set => this.m_pool = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.TryGetComponent<Animator>(out this.m_animator);

        this.m_positions[0] = new Vector2(7, -7);
        this.m_positions[1] = new Vector2(-7, 2);
        this.m_positions[2] = new Vector2(-7, -7);
        this.m_positions[3] = new Vector2(7, 2);

        this.UpdatePos();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Invoke("Scare", 0);
        AnimatorStateInfo l_info = this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (l_info.IsName("Appear") && l_info.normalizedTime < 1.0f)
        {
            return;
        }

        if ((Vector2)this.transform.position == this.m_positions[this.m_index])
        {
            this.m_index = (this.m_index + 1 == this.m_positions.Length) ? 0 : this.m_index + 1;
        }

        this.Scare();

    }

    /// <summary>
    /// Moves the Ghosty across the scenes
    /// </summary>
    protected void Scare()
    {
        float l_step = Time.deltaTime * 5.0f;
        Vector3 l_targetPos = this.m_positions[this.m_index];
        l_targetPos.z = this.transform.position.z;
        this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, l_targetPos, l_step);
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
        this.AnimeDefeat();
        EVT_DEFEAT?.Invoke();
    }

    /// <summary>
    /// Animation handler when ghosty is defeated
    /// </summary>
    public void AnimeDefeat()
    {
        this.ToggleBoxCollider();
        this.m_animator.SetTrigger("pDefeated");
    }

    /// <summary>
    /// Actions after the ghosty is defeated
    /// </summary>
    public void OnDisappear()
    {
        this.ToggleBoxCollider();
        this.Dive();
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
        this.m_pool.ReturnToPool(this);
    }
}
