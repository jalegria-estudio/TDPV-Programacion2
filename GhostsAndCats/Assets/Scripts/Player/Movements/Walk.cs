using UnityEngine;

namespace Movements
{
    /**
     * C# PARTIAL CLASS
     * Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
     */
    public partial class Moves
    {
        /// <summary> Player character walking and running </summary>
        /// <remarks>Note: With keyboard the movements is precise, with joystick from 0 to 1 is instant.</remarks> 
        /// <param name="p_speed"></param>
        /// <param name="p_horizontalInput"></param>
        public static void Walk(Rigidbody2D p_rigidBody, float p_speed, float p_horizontalInput)
        {
            float l_step = p_speed * Time.deltaTime * p_horizontalInput;//<(!) the move is more precise with horizontal input var
            //<(!) Moves the transform in the direction and distance of translation.
            // Source: https://docs.unity3d.com/ScriptReference/Transform.Translate.html
            //p_rigidBody.transform.Translate(l_step, .0f, .0f); //<(!) Precise movement
            p_rigidBody.AddForce(new Vector2(p_speed * p_horizontalInput, .0f), ForceMode2D.Force); //<(!) Physics enviroment
        }
    }
}
