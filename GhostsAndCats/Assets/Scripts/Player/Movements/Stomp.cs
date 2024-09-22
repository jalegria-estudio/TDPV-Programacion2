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
        /// Resolves a hop on enemy's head for stomp it - "player attack"
        /// </summary>
        public static void Stomp(Rigidbody2D p_rigidBody, Collider2D p_collider)
        {
            Debug.Log($"<DEBUG> Stomp to game-object name:{p_collider} Tag: {p_collider.tag}");
            //this.gameObject.GetComponent<PlayerInputHandler>().Plop(5.0f);
            Moves.Plop(p_rigidBody, 5.0f);
            p_collider.gameObject.SetActive(false);
        }
    }
}
