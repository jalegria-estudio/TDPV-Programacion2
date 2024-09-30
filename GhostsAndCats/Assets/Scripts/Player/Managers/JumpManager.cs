using Movements;
using Settings;
using UnityEngine;

namespace Managers
{
    public class JumpManager : MonoBehaviour
    {
        //** Observer pattern => Managing Game Events with the Event Bus
        public event System.Action EVT_JUMP;

        [Header("CONFIGURATION")]
        [SerializeField] protected float m_impulse = Config.PLAYER_JUMP_IMPULSE;
        [SerializeField] protected float m_impulseUp = Config.PLAYER_JUMP_IMPULSE_UP;
        protected bool m_jump = false;
        protected bool m_jumped = false;
        protected bool m_doubleJumped = false;
        protected Rigidbody2D m_rigidBody = null;

        public bool Jump { get => m_jump; set => m_jump = value; }
        public bool Jumped { get { return m_jumped; } }
        public bool DoubleJumped { get { return m_doubleJumped; } }

        protected void Start()
        {
            m_rigidBody = this.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Manager to the character's jump and attack
        /// </summary>
        /// <remarks>Note: With keyboard the movements is precise, with joystick from 0 to 1 is instant.</remarks> 
        /// <param name="p_verticalInput"></param>
        /// <returns>True if the player character is jumping.</returns>
        public bool HandleInput(float p_verticalInput)
        {
            Vector2 l_currentVelocity = m_rigidBody.GetPointVelocity(transform.position);

            //<(i) Simple Jump
            if (p_verticalInput > 0 && !m_jumped)
            {
                Moves.Jump(m_rigidBody, m_impulse);
                m_jumped = true;
                EVT_JUMP?.Invoke();
            }
            //<(i) Double Jump!
            else if (m_jumped && !m_doubleJumped && p_verticalInput == 1)
            {
                Moves.Jump(m_rigidBody, m_impulse * m_impulseUp);
                m_doubleJumped = m_jumped;
            }
            //<(i) Landing
            else if (l_currentVelocity.y == 0 && m_jumped && p_verticalInput == 0)
            {
                m_jumped = m_doubleJumped = false;
            }

            return m_jumped;
        }
    }
}//namespace Managers
