/**
 * @file ToolboxLib - Include my library tools for Unity-C# XD
 * @version 0.1
 * @category Utils - Toolbox!
 * @brief Multi-tooled Library
 * @details Mi caja de herramientas.
 *  Aqui incluyo mis herramientas para incluirla en mis proyectos. En este caso Unity.
 * @author: Juan P. Alegria
 * @date 2024-08-31
 */

using UnityEngine;

//Mi caja de herramientas
namespace Toolbox //Incluir a la caja de herramientas las funciones que se necesiten llevar
{
    //
    // Summary:
    //     Move's direction. Note: It's a similar to>> using Direction = NavigationMoveEvent.Direction;
    public enum Direction
    {
        //
        // Summary:
        //     No specific direction.
        None,
        //
        // Summary:
        //     Left.
        Left,
        //
        // Summary:
        //     Up.
        Up,
        //
        // Summary:
        //     Right.
        Right,
        //
        // Summary:
        //     Down.
        Down,
        //
        // Summary:
        //     Forwards, toward next element.
        Next,
        //
        // Summary:
        //     Backwards, toward previous element.
        Previous
    }

    //Herramientas de fisica Unity
    public class PhysicsTools
    {
        public const float m_rayDistDefault = 1.0f;
        public const float m_rayPosOffset = 0.01f;

        /// <summary>
        /// Crea un raycast a partir de una bounding box. El beam sale desde afuera de la caja a partir de las coords del centro.
        /// </summary>
        public static RaycastHit2D CreateRaycastHitFrom(Bounds p_boundingBox, Direction p_rayDirection, float p_rayDistance = m_rayDistDefault, int p_layerMask = Physics2D.DefaultRaycastLayers)
        {
            Vector2 l_origin = p_boundingBox.center;
            Vector2 l_dir = Vector2.zero;

            switch (p_rayDirection)
            {
                case Direction.Up:
                    l_origin.y += p_boundingBox.extents.y + m_rayPosOffset;
                    l_dir = Vector2.up;
                    break;
                case Direction.Right:
                    l_origin.x += p_boundingBox.extents.x + m_rayPosOffset;
                    l_dir = Vector2.right;
                    break;
                case Direction.Down:
                    l_origin.y -= p_boundingBox.extents.y + m_rayPosOffset;
                    l_dir = Vector2.down;
                    break;
                case Direction.Left:
                    l_origin.x -= p_boundingBox.extents.x + m_rayPosOffset;
                    l_dir = Vector2.left;
                    break;
                default:
                    Debug.Assert(true, "Create a raycast-hit without a direction");
                    break;
            }

            return CreateRaycastHit(l_origin, l_dir, p_rayDistance, p_layerMask);
        }

        /// <summary>
        /// Wrapper Physics2D.Raycast - Crea un raycast - My Galactic Beam Caster :)
        /// </summary>
        /// <remarks>
        /// LAYERMASK => USADO PARA FILTRAR RAYCAST
        /// Specifies Layers to use in a Physics.Raycast.
        /// A GameObject can use up to 32 LayerMasks supported by the Editor.
        /// The first 8 of these Layers are specified by Unity; the following 24 are controllable by the user. 
        /// Bitmasks represent the 32 Layers and define them as true or false. 
        /// Each bitmask describes whether the Layer is used.As an example, bit 5 can be set to 1 (true).
        /// This will allow the use of the built-in Water setting.
        /// https://docs.unity3d.com/ScriptReference/LayerMask.html => bitmask => 0b0001
        /// https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html => raycast()
        /// public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
        /// </remarks>
        public static RaycastHit2D CreateRaycastHit(Vector2 p_origin, Vector2 p_direction, float p_distance = m_rayDistDefault, int p_layerMask = Physics2D.DefaultRaycastLayers)
        {
            RaycastHit2D l_raycastHit = Physics2D.Raycast(p_origin, p_direction, p_distance, p_layerMask);
            Debug.DrawRay(p_origin, p_direction * p_distance);

            return l_raycastHit;
        }
    }

    public class RenderingTools
    {
        /// <summary>
        /// Resize a sprite scale
        /// </summary>
        public static void ChangeSpriteScale(SpriteRenderer p_sprtRndr, Vector2 p_newRelativeScale)
        {
            Vector2 l_currentScale = p_sprtRndr.transform.localScale;
            Debug.Log("Change scale -> Before: " + l_currentScale);
            p_sprtRndr.transform.localScale = Vector2.Scale(l_currentScale, p_newRelativeScale);
            Debug.Log("Change scale -> After: " + p_sprtRndr.transform.localScale);
        }
    }

    public class AnimationTools
    {
        public static bool isPlaying(Animator p_animator, string p_animeName)
        {
            AnimatorStateInfo l_currentState = p_animator.GetCurrentAnimatorStateInfo(0);
            bool l_state = l_currentState.IsName(p_animeName);
            return l_state;
        }

        /// <summary>
        /// Get normalized time of current animation state
        /// </summary>
        /// <remarks>Wrapper - AnimatorStateInfo.normalizedTime</remarks>
        /// <returns>The normalized time is a progression ratio. The integer part is the number of times the State has looped. The fractional part is a percentage (0-1) that represents the progress of the current loop. For example, a normalized time of 2.5 means that the State has looped twice (2) and has progressed halfway (50% or .5) through its third loop.</returns>
        public static float getAnimeCurrentTime(Animator p_animator)
        {
            AnimatorStateInfo l_currentState = p_animator.GetCurrentAnimatorStateInfo(0);
            return l_currentState.normalizedTime;
        }
    }
}
