using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss Enemy Class
/// </summary>
public class Boss : MonoBehaviour
{
    /////////////////////////////////////
    /// EVENTS
    ////////////////////////////////////
    public event System.Action EVT_KNOCK;
    public event System.Action EVT_KNOCK_OUT;

    /////////////////////////////////////
    /// CONFIGURATION EDITOR ATTRIBUTES
    ////////////////////////////////////
    [Header("Boss Configuraton")]
    [Range(0, 10)]
    [SerializeField] protected int m_lifes = 5;
    [SerializeField] protected int m_bossScore = 1000;
    [SerializeField] protected int m_GhostyScore = 100;
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
        this.SetupPool();
        this.m_jukebox = this.gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (this.m_lifes <= 0 && !this.m_knockOut)
        {
            this.KnockOut();
        }
    }

    /// <summary>
    /// Add a new Ghosty to scene
    /// </summary>
    public void SummonAGhosty()
    {
        if (this.m_knockOut)
            return;

        this.m_jukebox.PlayOneShot(this.m_sfxAngry);

        if (this.m_pool.Count == 0)
            return;

        this.GetFromPool();
    }

    /////////////////////////////////
    /// OBJECT POOL PATTERN METHODS
    ////////////////////////////////

    /// <summary>
    /// Config the pool
    /// </summary>
    protected void SetupPool()
    {
        this.m_pool = new Stack<Ghosty>();
        Ghosty l_poolableGhosty;

        for (uint i = 0; i < this.m_poolSize; i++)
        {
            l_poolableGhosty = Instantiate(this.m_ghosty, this.transform); //<(!) I use the transform to add a ghosty with boss as parent and easy destroy
            l_poolableGhosty.transform.localScale = new Vector3(0.5f, 0.5f, 1); //<(!) To reduce the normal size from parent transform
            l_poolableGhosty.Pool = this;
            l_poolableGhosty.IndexPos = i; //<(i) Make a reverse position to appear not so sticky
            l_poolableGhosty.UpdatePos();
            l_poolableGhosty.gameObject.SetActive(false);
            this.m_pool.Push(l_poolableGhosty);
        }
    }

    /// <summary>
    /// Push a ghosty object to pool
    /// </summary>
    public void ReturnToPool(Ghosty p_object)
    {
        p_object.EVT_DEFEAT -= this.OnKnock;
        //p_object.GetComponent<SpriteRenderer>().color = Color.white;
        p_object.gameObject.SetActive(false);
        this.m_pool.Push(p_object);
    }

    /// <summary>
    /// Instance a new ghosty object or pop it from pool
    /// </summary>
    public Ghosty GetFromPool()
    {
        Ghosty l_object = null;

        if (this.m_pool.Count != 0)
        {
            l_object = this.m_pool.Pop();
            l_object.gameObject.SetActive(true);
        }
        else
        {
            l_object = Instantiate(this.m_ghosty, this.transform);
            this.m_ghosty.Pool = this;
        }

        l_object.EVT_DEFEAT += this.OnKnock;

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
        if (this.gameObject.TryGetComponent<Animator>(out Animator l_animator))
        {
            l_animator.SetTrigger("pKnock");
            this.m_jukebox.PlayOneShot(this.m_sfxSad);
            this.m_lifes--;
            this.EVT_KNOCK?.Invoke();
            return;
        }

        Debug.Assert(l_animator != null, "<DEBUG ASSERT> Can not knock the boss! There isn't boss animator.");
    }

    /// <summary>
    /// Handler when the Boss is defeated
    /// </summary>
    protected void KnockOut()
    {
        if (this.gameObject.TryGetComponent<Animator>(out Animator l_animator))
        {
            l_animator.SetTrigger("pKnockOut");
            this.m_jukebox.PlayOneShot(this.m_sfxKnockOut);
        }

        Ghosty[] l_ghosties = GameObject.FindObjectsOfType<Ghosty>();
        for (int i = 0; i < l_ghosties.Length; i++)
        {
            l_ghosties[i].AnimeDefeat();
            this.ReturnToPool(l_ghosties[i]);
        }

        this.m_pool.Clear();

        this.m_knockOut = true;
    }

    /// <summary>
    /// Animation Event Handler
    /// </summary>
    public void OnScare()
    {
        this.m_jukebox.PlayOneShot(this.m_sfxScare);
    }

    /// <summary>
    /// Callback when the boss disappeared from screen
    /// </summary>
    public void OnDisappear()
    {
        EVT_KNOCK_OUT?.Invoke();
    }
}
