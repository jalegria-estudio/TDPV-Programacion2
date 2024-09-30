using Movements;
using Settings;
using UnityEngine;

namespace Managers
{
    public class WalkManager : MonoBehaviour
    {
        ///// EVENTS ACTIONS /////
        public System.Action<bool> EVT_DUCK;

        ///// INSPECTOR CONFIGURATION /////
        [Header("CONFIGURATION")]
        [SerializeField] protected float m_walkSpeed = Config.PLAYER_WALK_SPEED;
        [SerializeField] protected float m_runSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_RUN_FACTOR;
        [SerializeField] protected float m_duckSpeed = Config.PLAYER_WALK_SPEED * Config.PLAYER_WALK_DUCK_FACTOR;
        [SerializeField] protected float m_duckScale = Config.PLAYER_DUCK_SCALE;
        [SerializeField] protected float m_unduckScale = Config.PLAYER_UNDUCK_SCALE;
        protected Rigidbody2D m_rigidBody = null;
        protected SpriteRenderer m_spriteRenderer = null;

        protected bool m_run = false;
        protected bool m_duck = false;
        protected bool m_ducked = false;
        protected float m_speed = .0f;

        public bool Run { get => m_run; set => m_run = value; }
        public bool Duck { get => m_duck; set => m_duck = value; }

        private void Start()
        {
            m_rigidBody = this.GetComponent<Rigidbody2D>();
            m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Manager to the horizontal character's moves: to walk
        /// </summary>
        /// <param name="p_horizontalInput"></param>
        public void HandleInput(float p_horizontalInput)
        {
            if (canRun())
                run(p_horizontalInput);
            else if (canDuck())
                duck(p_horizontalInput);
            else if (canUnduck())//<(i) Only Debug: if (Input.GetKey(KeyCode.Z))
                unduck(p_horizontalInput);
            else
                walk(p_horizontalInput);
        }

        protected bool canDuck()
        {
            return m_duck && (m_rigidBody.velocity.y == 0);
        }

        protected bool canUnduck()
        {
            //<(e) Check if sprite is colliding with platform with normal-up
            if (gameObject.GetComponents<Collider2D>()[1].enabled)
            {
                Collider2D l_collider = gameObject.GetComponents<Collider2D>()[1];
                ContactFilter2D l_filter = new ContactFilter2D();
                l_filter.SetLayerMask(LayerMask.GetMask("lplatforms"));
                ContactPoint2D[] l_contacts = new ContactPoint2D[4];
                int l_found = l_collider.GetContacts(l_contacts);
                if (l_found > 0)
                {
                    for (int i = 0; i < l_contacts.Length; i++)
                    {
                        if (l_contacts[i].collider != null && l_contacts[i].collider.CompareTag("tPlatform") && l_contacts[i].normal == Vector2.down)
                        {
                            return false;
                        }
                    }
                }
            }

            return !m_duck && m_ducked;
        }

        protected bool canRun()
        {
            return m_run && !m_ducked;
        }

        protected void walk(float p_horizontalInput)
        {
            if (m_ducked)
                duck(p_horizontalInput);
            else
                Moves.Walk(m_rigidBody, m_walkSpeed, p_horizontalInput);
        }

        protected void run(float p_horizontalInput)
        {
            Moves.Run(m_rigidBody, m_runSpeed, p_horizontalInput);
        }

        protected void duck(float p_horizontalInput)
        {
            if (!m_ducked)
            {
                Moves.DuckRenderer(gameObject.GetComponents<BoxCollider2D>());
                m_ducked = true;
                EVT_DUCK?.Invoke(m_ducked);
            }

            Moves.Duck(m_rigidBody, m_duckSpeed, p_horizontalInput);
        }

        protected void unduck(float p_horizontalInput)
        {
            m_rigidBody.mass = 1;
            Moves.Unduck(m_rigidBody, m_walkSpeed, p_horizontalInput);
            Moves.UnduckRenderer(gameObject.GetComponents<BoxCollider2D>());
            m_ducked = false;
            EVT_DUCK?.Invoke(m_ducked);
        }
    }
}//namespace Managers
