using Movements;
using Settings;
using UnityEngine;

namespace Managers
{
    public class WalkManager : MonoBehaviour
    {
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
            //return !m_ducked && (transform.localScale.y == 1) && (m_rigidBody.velocity.y == 0);
            return m_duck && !m_ducked && (m_rigidBody.velocity.y == 0);
        }

        protected bool canUnduck()
        {
            //return m_ducked && transform.localScale.y < 1.0f;
            return !m_duck && m_ducked;
        }

        protected bool canRun()
        {
            return m_run && !m_ducked;
        }

        protected void walk(float p_horizontalInput)
        {
            Moves.Walk(m_rigidBody, m_walkSpeed, p_horizontalInput);
        }

        protected void run(float p_horizontalInput)
        {
            Moves.Walk(m_rigidBody, m_runSpeed, p_horizontalInput);
        }

        protected void duck(float p_horizontalInput)
        {
            if (!m_ducked)
            {
                Moves.DuckRenderer(m_spriteRenderer, m_duckScale);
                m_ducked = true;
            }
            m_rigidBody.mass = 1.5f;
            m_rigidBody.AddForce(new Vector2(m_duckSpeed * p_horizontalInput, 0));
            //Moves.Duck(m_rigidBody, m_duckSpeed, p_horizontalInput);
        }

        protected void unduck(float p_horizontalInput)
        {
            m_rigidBody.mass = 1;
            Moves.Unduck(m_rigidBody, m_walkSpeed, p_horizontalInput);
            Moves.UnduckRenderer(m_spriteRenderer, m_unduckScale);
            m_ducked = false;
        }
    }
}
