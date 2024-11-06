using System.Collections;
using System.Collections.Generic;
using System.Game;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /////////////////////////////////////
    /// CONFIGURATION EDITOR ATTRIBUTES
    ////////////////////////////////////
    [Header("Boss Configuraton")]
    [Range(0, 10)]
    [SerializeField] protected int m_lifes = 5;
    [Header("Sounds")]
    [SerializeField] AudioClip m_sfxScare = null;
    [SerializeField] AudioClip m_sfxKnockOut = null;
    [SerializeField] AudioClip m_sfxAngry = null;
    [SerializeField] AudioClip m_sfxSad = null;

    //////////////////////////
    /// ATTRIBUTES
    /////////////////////////
    protected bool m_knockOut = false;
    protected AudioSource m_jukebox = null;

    //////////////////////////
    /// OBJECT POOL PATTERN
    /// Source: Level-Up Your Code With Game Programming Patterns. p.49;
    /////////////////////////
    [Header("Pool Configuration")]
    [SerializeField] protected uint m_poolSize = 4;
    [SerializeField] protected Ghosty m_ghosty;

    protected Stack<Ghosty> m_pool;

    // Start is called before the first frame update
    void Start()
    {
        SetupPool();
        m_jukebox = gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (m_lifes <= 0 && !m_knockOut)
        {
            KnockOut();
        }
    }

    /// <summary>
    /// Add a new Ghosty to scene
    /// </summary>
    public void SummonAGhosty()
    {
        if (m_knockOut)
            return;

        m_jukebox.PlayOneShot(m_sfxAngry);

        if (m_pool.Count == 0)
            return;

        GetFromPool();
    }

    /////////////////////////////////
    /// OBJECT POOL PATTERN METHODS
    ////////////////////////////////

    /// <summary>
    /// Config the pool
    /// </summary>
    protected void SetupPool()
    {
        m_pool = new Stack<Ghosty>();
        Ghosty l_poolableGhosty;

        for (uint i = 0; i < m_poolSize; i++)
        {
            l_poolableGhosty = Instantiate(m_ghosty);
            l_poolableGhosty.Pool = this;
            l_poolableGhosty.IndexPos = i; //<(i) Make a reverse position to appear not so sticky
            l_poolableGhosty.UpdatePos();
            l_poolableGhosty.gameObject.SetActive(false);
            m_pool.Push(l_poolableGhosty);
        }
    }

    /// <summary>
    /// Push a ghosty object to pool
    /// </summary>
    public void ReturnToPool(Ghosty p_object)
    {
        p_object.EVT_DEFEAT -= OnKnock;
        //p_object.GetComponent<SpriteRenderer>().color = Color.white;
        p_object.gameObject.SetActive(false);
        m_pool.Push(p_object);
    }

    /// <summary>
    /// Instance a new ghosty object or pop it from pool
    /// </summary>
    public Ghosty GetFromPool()
    {
        Ghosty l_object = null;

        if (m_pool.Count != 0)
        {
            l_object = m_pool.Pop();
            l_object.gameObject.SetActive(true);
        }
        else
        {
            l_object = Instantiate(m_ghosty);
            m_ghosty.Pool = this;
        }

        l_object.EVT_DEFEAT += OnKnock;

        return l_object;
    }

    /////////////////////////
    /// EVENTS HANDLER
    ////////////////////////

    /// <summary>
    /// Handler when the Boss lost a life 
    /// </summary>
    protected void OnKnock()
    {
        m_lifes--;

        if (this.gameObject.TryGetComponent<Animator>(out Animator l_animator))
        {
            l_animator.SetTrigger("pKnock");
            m_jukebox.PlayOneShot(m_sfxSad);
        }
    }

    /// <summary>
    /// Handler when the Boss is defeated
    /// </summary>
    protected void KnockOut()
    {
        if (this.gameObject.TryGetComponent<Animator>(out Animator l_animator))
        {
            l_animator.SetTrigger("pKnockOut");
            m_jukebox.PlayOneShot(m_sfxKnockOut);
        }

        Ghosty[] l_ghosties = GameObject.FindObjectsOfType<Ghosty>();
        for (int i = 0; i < l_ghosties.Length; i++)
        {
            l_ghosties[i].AnimeDefeat();
            ReturnToPool(l_ghosties[i]);
        }

        m_knockOut = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().MoveNextLevel();
    }

    /// <summary>
    /// Animation Event Handler
    /// </summary>
    public void OnScare()
    {
        m_jukebox.PlayOneShot(m_sfxScare);
    }
}
