using Movements;
using Settings;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Jumping Manager
    /// </summary>
    public class JumpManager : MonoBehaviour
    {
        ///// EVENTS ACTIONS /////
        //** Observer pattern => Managing Game Events with the Event Bus
        public event System.Action EVT_JUMP;

        ///// COMPONENTS /////
        protected PlayerData m_data = null;
        protected Rigidbody2D m_rigidBody = null;

        ///// STATUS /////
        protected bool m_jump = false;
        protected bool m_jumped = false;
        protected bool m_doubleJumped = false;

        public bool Jump { get => m_jump; set => m_jump = value; }
        public bool Jumped { get { return m_jumped; } }
        public bool DoubleJumped { get { return m_doubleJumped; } }

        protected void Start()
        {
            m_rigidBody = this.GetComponent<Rigidbody2D>();
            m_data = GameObject.Find("Player").GetComponent<Player>().Data;
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
                Moves.Jump(m_rigidBody, m_data.Impulse);
                m_jumped = true;
                EVT_JUMP?.Invoke();
            }
            //<(i) Double Jump!
            else if (m_jumped && !m_doubleJumped && p_verticalInput == 1)
            {
                Moves.Jump(m_rigidBody, m_data.Impulse * m_data.ImpulseUp);
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
