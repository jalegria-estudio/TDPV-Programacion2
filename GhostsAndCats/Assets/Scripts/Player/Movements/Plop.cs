using UnityEngine;

namespace Movements
{
    public partial class Moves
    {
        /// <summary>
        /// Player character ploping (after the enemy defeating)
        /// </summary>
        public static void Plop(Rigidbody2D p_rigidBody, float p_impulse)
        {
            Vector2 l_velocity = p_rigidBody.velocity;
            l_velocity.y -= l_velocity.y;
            p_rigidBody.velocity = l_velocity;
            Jump(p_rigidBody, p_impulse);
        }
    }
}
