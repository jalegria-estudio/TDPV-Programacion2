using Settings;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Animation Manager
    /// </summary>
    public class AnimeManager : MonoBehaviour
    {
        ///// COMPONENTS /////
        protected Rigidbody2D m_rigidBody = null;
        protected SpriteRenderer m_spriteRenderer = null;
        protected Animator m_animator = null;

        ///// STATUS /////
        float m_inputH = 0.0f;
        float m_inputV = 0.0f;

        // Called when the object becomes enabled and active, always after Awake (on the same object) and before any Start.
        //Source: https://docs.unity3d.com/Manual/ExecutionOrder.html
        protected void OnEnable()
        {
            //Not delete
        }

        protected void OnDestroy()
        {
            this.unsubscribesToEvents();
        }

        // Start is called before the first frame update
        protected void Start()
        {
            this.m_animator = this.GetComponent<Animator>();
            this.m_rigidBody = this.GetComponent<Rigidbody2D>();
            this.m_spriteRenderer = this.GetComponent<SpriteRenderer>();

            this.subscribesToEvents();
        }

        protected void Update()
        {
            //<(i) Input.GetAxisRaw
            // Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
            // Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
            // Source: https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
            this.m_inputH = Input.GetAxisRaw("Horizontal");
            this.m_inputV = Input.GetAxisRaw("Vertical");
        }

        /// <summary>
        /// Indicate if animation is playing
        /// </summary>
        /// <param name="p_animeName">Animation clip name</param>
        /// <returns>boolean</returns>
        public bool IsPlaying(string p_animeName)
        {
            AnimatorStateInfo l_currentState = this.m_animator.GetCurrentAnimatorStateInfo(Config.ANIMATOR_BASE_LAYER);
            bool l_state = l_currentState.IsName(p_animeName);
            return l_state;
        }

        /// <summary>
        /// Reset animator parameters
        /// </summary>
        protected void resetParameters()
        {
            this.m_animator.SetInteger("pInputV", 0);
            this.m_animator.SetInteger("pInputH", 0);
            this.m_animator.SetInteger("pVelocityV", 0);
            this.m_animator.SetBool("pDucked", false);
        }

        ///////////////////////////////////
        /// OBSERVER PATTERN SUSCRIPTION
        ///////////////////////////////////
        protected void subscribesToEvents()
        {
            if (this.gameObject.GetComponent<WalkManager>() != null)
                this.gameObject.GetComponent<WalkManager>().EVT_DUCK += this.HandleDuck;

            if (this.gameObject.GetComponent<ExperienceManager>() != null)
                this.gameObject.GetComponent<ExperienceManager>().EVT_1UP += this.Handle1Up;
        }

        protected void unsubscribesToEvents()
        {
            if (this.gameObject.GetComponent<WalkManager>() != null)
                this.gameObject.GetComponent<WalkManager>().EVT_DUCK -= this.HandleDuck;

            if (this.gameObject.GetComponent<ExperienceManager>() != null)
                this.gameObject.GetComponent<ExperienceManager>().EVT_1UP -= this.Handle1Up;
        }

        ///////////////////////////////////
        /// ACTIONS HANDLER
        ///////////////////////////////////
        public void Reset()
        {
            this.resetParameters();
            this.m_animator.Play("Idle");
        }

        public void HandleInput()
        {
            int l_anime = 0;

            if (this.m_inputH != 0)
                l_anime = (this.m_inputH > 0) ? 1 : -1;

            this.m_animator.SetInteger("pInputH", l_anime);

            if (l_anime < 0)
                this.m_spriteRenderer.flipX = true;
            else if (l_anime > 0)
                this.m_spriteRenderer.flipX = false;

            l_anime = (int)this.m_inputV;
            this.m_animator.SetInteger("pInputV", l_anime);

            int l_velocity = (int)this.m_rigidBody.velocity.normalized.y;
            this.m_animator.SetInteger("pVelocityV", (int)this.m_rigidBody.velocity.normalized.y);
        }

        public void HandleDamage()
        {
            this.resetParameters();
            this.m_animator.SetTrigger("pDamaged");
            Movements.Moves.Plop(this.m_rigidBody, Config.PLAYER_JUMP_IMPULSE / 2.0f);
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        public void HandleDefeat()
        {
            this.resetParameters();
            this.m_animator.SetTrigger("pDefeated");
        }

        public void HandleDuck(bool p_ducked)
        {
            this.m_animator.SetBool("pDucked", p_ducked);
        }

        public void Handle1Up()
        {
            this.m_animator.SetTrigger("p1up");
        }
    }
}//namespace Managers
