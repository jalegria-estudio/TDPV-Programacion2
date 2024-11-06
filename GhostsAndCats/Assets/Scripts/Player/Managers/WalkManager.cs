using Movements;
using Settings;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Walking Manager
    /// </summary>
    public class WalkManager : MonoBehaviour
    {
        ///// EVENTS ACTIONS /////
        public System.Action<bool> EVT_DUCK;

        ///// COMPONENTS /////
        protected PlayerData m_data = null;
        protected Rigidbody2D m_rigidBody = null;
        protected SpriteRenderer m_spriteRenderer = null;

        ///// RENDERING /////
        protected float m_duckScale = Config.PLAYER_DUCK_SCALE; //Low-level Config
        protected float m_unduckScale = Config.PLAYER_UNDUCK_SCALE; //Low-level Config

        ///// STATUS /////
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
            m_data = GameObject.Find("Player").GetComponent<Player>().Data;
        }

        /// <summary>
        /// Manager to the horizontal character's moves: to walk
        /// </summary>
        /// <param name="p_horizontalInput"></param>
        public void HandleInput(float p_horizontalInput)
        {
            if (canRun())
                run(p_horizontalInput);
            else if (CanDuck())
                duck(p_horizontalInput);
            else if (CanUnduck())//<(i) Only Debug: if (Input.GetKey(KeyCode.Z))
                unduck(p_horizontalInput);
            else
                walk(p_horizontalInput);
        }

        //
        // Summary:
        //    Indicate if the sprite is on condition to duck transform
        protected bool CanDuck()
        {
            return m_duck && (m_rigidBody.velocity.y == 0);
        }

        //
        // Summary:
        //    <(!) Platform distance between sides to ducking It must be minor than 0.5 size.
        protected bool CanUnduck()
        {
            //<(e) Check if sprite is colliding with platform with normal-up
            if (gameObject.GetComponents<Collider2D>()[1].enabled)
            {
                Collider2D l_collider = gameObject.GetComponents<Collider2D>()[1];//Duck Collider == 2nd Collider
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
                Moves.Walk(m_rigidBody, m_data.WalkSpeed, p_horizontalInput);
        }

        protected void run(float p_horizontalInput)
        {
            Moves.Run(m_rigidBody, m_data.RunSpeed, p_horizontalInput);
        }

        protected void duck(float p_horizontalInput)
        {
            if (!m_ducked)
            {
                Moves.DuckRenderer(gameObject.GetComponents<BoxCollider2D>());
                m_ducked = true;
                EVT_DUCK?.Invoke(m_ducked);
            }

            Moves.Duck(m_rigidBody, m_data.DuckSpeed, p_horizontalInput);
        }

        protected void unduck(float p_horizontalInput)
        {
            m_rigidBody.mass = 1;
            Moves.Unduck(m_rigidBody, m_data.WalkSpeed, p_horizontalInput);
            Moves.UnduckRenderer(gameObject.GetComponents<BoxCollider2D>());
            m_ducked = false;
            EVT_DUCK?.Invoke(m_ducked);
        }
    }
}//namespace Managers
