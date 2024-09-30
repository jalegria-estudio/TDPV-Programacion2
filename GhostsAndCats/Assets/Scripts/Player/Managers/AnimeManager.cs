using UnityEngine;

namespace Managers
{
    public class AnimeManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] float m_inputH = 0.0f;
        [SerializeField] float m_inputV = 0.0f;

        protected Rigidbody2D m_rigidBody = null;
        protected SpriteRenderer m_spriteRenderer = null;
        protected Animator m_animator = null;

        // Called when the object becomes enabled and active, always after Awake (on the same object) and before any Start.
        //Source: https://docs.unity3d.com/Manual/ExecutionOrder.html
        protected void OnEnable()
        {
            //Not delete
        }

        protected void OnDestroy()
        {
            unsubscribesToEvents();
        }

        // Start is called before the first frame update
        protected void Start()
        {
            m_animator = this.GetComponent<Animator>();
            m_rigidBody = this.GetComponent<Rigidbody2D>();
            m_spriteRenderer = this.GetComponent<SpriteRenderer>();

            subscribesToEvents();
        }

        protected void Update()
        {
            //<(i) Input.GetAxisRaw
            // Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
            // Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
            // Source: https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
            m_inputH = Input.GetAxisRaw("Horizontal");
            m_inputV = Input.GetAxisRaw("Vertical");
        }

        protected void subscribesToEvents()
        {
            if (gameObject.GetComponent<WalkManager>())
                gameObject.GetComponent<WalkManager>().EVT_DUCK += HandleDuck;
        }

        protected void unsubscribesToEvents()
        {
            if (gameObject.GetComponent<WalkManager>())
                gameObject.GetComponent<WalkManager>().EVT_DUCK -= HandleDuck;
        }

        public void HandleInput()
        {
            int l_anime = 0;

            if (m_inputH != 0)
                l_anime = (m_inputH > 0) ? 1 : -1;

            m_animator.SetInteger("pInputH", l_anime);

            if (l_anime < 0)
                m_spriteRenderer.flipX = true;
            else if (l_anime > 0)
                m_spriteRenderer.flipX = false;

            l_anime = (int)m_inputV;
            m_animator.SetInteger("pInputV", l_anime);

            int l_velocity = (int)m_rigidBody.velocity.normalized.y;
            m_animator.SetInteger("pVelocityV", (int)m_rigidBody.velocity.normalized.y);
        }

        public void HandleDamage()
        {
            //m_animator.Play("Base Layer.Damage");
            resetParameters();
            m_animator.SetTrigger("pDamaged");
        }

        public void HandleDefeat()
        {
            resetParameters();
            m_animator.SetTrigger("pDefeated");
        }

        public void HandleDuck(bool p_ducked)
        {
            m_animator.SetBool("pDucked", p_ducked);
        }

        public bool isPlaying(string p_animeName)
        {
            AnimatorStateInfo l_currentState = m_animator.GetCurrentAnimatorStateInfo(0);
            bool l_state = l_currentState.IsName(p_animeName);
            return l_state;
        }

        protected void resetParameters()
        {
            m_animator.SetInteger("pInputV", 0);
            m_animator.SetInteger("pInputH", 0);
            m_animator.SetInteger("pVelocityV", 0);
            m_animator.SetBool("pDucked", false);
        }
    }
}//namespace Managers
