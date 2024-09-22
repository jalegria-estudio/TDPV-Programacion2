using UnityEngine;

namespace Movements
{
    /**
     * C# PARTIAL CLASS
     * Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
     */
    public partial class Moves
    {
        /// <summary>
        /// Player character jumping
        /// </summary>
        /// <param name="p_impulse"></param>
        public static void Jump(Rigidbody2D p_rigidBody, float p_impulse)
        {
            p_rigidBody.AddForce(Vector2.up * p_impulse, ForceMode2D.Impulse);
            //transform.Translate(Vector2.up * Time.deltaTime * m_inputPlayer);
        }
    }
}
