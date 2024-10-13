using Managers;
using UnityEngine;

[RequireComponent(typeof(JumpManager), typeof(WalkManager))]
public class InputController : MonoBehaviour
{
    protected WalkManager m_WalkManager = null;
    protected JumpManager m_JumpManager = null;
    protected Vector2 m_velocity = Vector2.zero;
    protected float m_inputPlayerVertical = .0f;
    protected float m_inputPlayerHorizontal = .0f;

    // This function is called when the object becomes enabled and active.
    protected void OnEnable()
    {
        this.GetComponent<Rigidbody2D>().freezeRotation = true;//<(e) No rotate over z-axis
    }

    // Start is called before the first frame update
    protected void Start()
    {
        m_JumpManager = this.GetComponent<JumpManager>();
        m_WalkManager = this.GetComponent<WalkManager>();
    }

    // Update is called once per frame [variable intervale]
    protected void Update()
    {
        m_velocity = this.GetComponent<Rigidbody2D>().velocity;
        m_inputPlayerVertical = Input.GetAxis("Vertical");
        m_inputPlayerHorizontal = Input.GetAxis("Horizontal");
        m_WalkManager.Run = Input.GetKey(KeyCode.Q);
        m_WalkManager.Duck = Input.GetKey(KeyCode.DownArrow);
        m_JumpManager.Jump = Input.GetKey(KeyCode.UpArrow);

        OnPressHelp();
    }

    // Update is called once per frame [fixed intervale]
    protected void FixedUpdate()
    {
        if (m_JumpManager.enabled) //<(i) I disassembled the if-conditions for clarity only
            m_JumpManager.HandleInput(m_inputPlayerVertical);

        if (m_WalkManager.enabled)
            m_WalkManager.HandleInput(m_inputPlayerHorizontal);
    }

    /// <summary>
    /// Display info about player controls on screen
    /// </summary>
    protected void OnPressHelp()
    {
        if (!Input.GetKeyDown(KeyCode.H))
            return;

        Canvas l_rules = GameObject.FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);

        if (l_rules && l_rules.CompareTag("tRules"))
            l_rules.enabled = !l_rules.enabled;

    }
}
