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
        public static RaycastHit2D CreateRaycastHitFrom(Bounds p_boundingBox, Direction p_rayDirection, float p_rayDistance = m_rayDistDefault)
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

            return CreateRaycastHit(l_origin, l_dir, p_rayDistance);
        }

        /// <summary>
        /// Wrapper Physics2D.Raycast - Crea un raycast - My Galactic Beam Caster :)
        /// </summary>
        public static RaycastHit2D CreateRaycastHit(Vector2 p_origin, Vector2 p_direction, float p_distance = m_rayDistDefault)
        {
            RaycastHit2D l_raycastHit = Physics2D.Raycast(p_origin, p_direction, p_distance);
            Debug.DrawRay(p_origin, p_direction * p_distance);

            return l_raycastHit;
        }
    }
}
