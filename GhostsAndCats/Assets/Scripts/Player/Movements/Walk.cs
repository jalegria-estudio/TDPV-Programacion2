using UnityEngine;

namespace Movements
{
    public partial class Moves
    {
        /// <summary> Player character walking and running </summary>
        /// <remarks>Note: With keyboard the movements is precise, with joystick from 0 to 1 is instant.</remarks> 
        /// <param name="p_speed"></param>
        /// <param name="p_horizontalInput"></param>
        public static void Walk(Rigidbody2D p_rigidBody, float p_speed, float p_horizontalInput)
        {
            float l_step = p_speed * Time.deltaTime * p_horizontalInput;//<(!) the move is more precise with horizontal input var
            p_rigidBody.transform.Translate(l_step, .0f, .0f); //<(!) Precise movement
        }
    }
}

//<(!) Moves the transform in the direction and distance of translation.
// Source: https://docs.unity3d.com/ScriptReference/Transform.Translate.html

