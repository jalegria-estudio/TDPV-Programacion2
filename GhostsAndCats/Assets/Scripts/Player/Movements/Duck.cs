using UnityEngine;

namespace Movements
{
    public partial class Moves
    {
        /// <summary>
        /// Player character ducking
        /// </summary>
        public static void Duck(Rigidbody2D p_rigidBody, float p_speed, float p_horizontalInput)
        {
            Moves.Walk(p_rigidBody, p_speed, p_horizontalInput);
            //p_rigidBody.mass = 2.5f;
            //p_rigidBody.AddForce(new Vector2(p_speed * p_horizontalInput, 0));
        }

        public static void Unduck(Rigidbody2D p_rigidBody, float p_speed, float p_horizontalInput)
        {
            Moves.Walk(p_rigidBody, p_speed, p_horizontalInput);
        }

        public static void DuckRenderer(BoxCollider2D[] p_colliders)
        {
            //Vector2 l_currentMaxBound = p_sprtRndr.bounds.max;
            ////Change scale transform
            //Vector2 l_newScale = Vector2.one;
            //l_newScale.y *= p_duckingScaleFactor;
            //Toolbox.RenderingTools.ChangeSpriteScale(p_sprtRndr, l_newScale);
            ////Change position transform
            //Vector2 l_newMaxBound = p_sprtRndr.bounds.max;
            //Vector2 l_newOffset = l_currentMaxBound - l_newMaxBound;
            //Vector2 l_currentPos = p_sprtRndr.transform.position;
            //l_currentPos.y -= l_newOffset.y;
            //p_sprtRndr.transform.position = l_currentPos;

            ////<(i) Debug Only
            //Color l_currentColor = p_sprtRndr.color;
            //l_currentColor.b = 1.0f;
            //p_sprtRndr.color = l_currentColor;
            //Debug.Log("Ducked! Reduce: " + l_newScale);

            p_colliders[0].enabled = false;
            p_colliders[1].enabled = true;
        }

        /// <summary>
        /// Player character unducking
        /// </summary>
        public static void UnduckRenderer(BoxCollider2D[] p_colliders)
        {
            //Vector2 l_currentMaxBound = p_sprtRndr.bounds.max;
            ////Changes scale transform
            //Vector2 l_newScale = Vector3.one;
            //l_newScale.y = p_unduckScaleFactor;
            //Toolbox.RenderingTools.ChangeSpriteScale(p_sprtRndr, l_newScale);
            ////Change position transform
            //Vector2 l_newMaxBound = p_sprtRndr.bounds.max;
            //Vector2 l_newOffset = l_currentMaxBound - l_newMaxBound;
            //Vector2 l_currentPos = p_sprtRndr.transform.position;
            //l_currentPos.y -= l_newOffset.y;
            //p_sprtRndr.transform.position = l_currentPos;
            ////<(i) Debug Only
            //Color l_currentColor = p_sprtRndr.color;
            //l_currentColor.b = .0f;
            //p_sprtRndr.color = l_currentColor;
            //Debug.Log("UnDucked! Grow: " + l_newScale);
            p_colliders[1].enabled = false;
            p_colliders[0].enabled = true;
        }
    }
}
