using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class Spawner : MonoBehaviour
    {
        public GameObject m_item = null;

        public void Spawn(Vector3 p_position)
        {
            //<(i) Clones the object original and returns the clone. This function makes a copy of an object in a similar way to the Duplicate command in the editor. 
            Instantiate(m_item, p_position, Quaternion.identity);
        }
    }

} // namespace Items

////////////////////////////////////////////////////////////
//
// Extensive Personal Documentation Notes
//
////////////////////////////////////////////////////////////

/**
 * Info about Instantiating Prefabs at run time => https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
 */

/**
 * QUATERNION - CORE MODULE
 * 
 * Description
 * quaternions are used to represent rotations.
 * A quaternion is a four-tuple of real numbers {x,y,z,w}. A quaternion is a mathematically convenient alternative to the euler angle representation. You can interpolate a quaternion without experiencing gimbal lock. You can also use a quaternion to concatenate a series of rotations into a single representation.
 * Unity internally uses Quaternions to represent all rotations.
 * In most cases, you can use existing rotations from methods such as Transform.localRotation or Transform.rotation to construct new rotations. For example, use existing rotations to smoothly interpolate between two rotations. The most used Quaternion functions are as follows: Quaternion.LookRotation, Quaternion.Angle, Quaternion.Euler, Quaternion.Slerp, Quaternion.FromToRotation, and Quaternion.identity.
 * You can use the Quaternion.operator * to rotate one rotation by another, or to rotate a vector by a rotation.
 * Note that Unity expects Quaternions to be normalized.
 * 
 * Help note: la identity rotation (rotación identidad) se refiere a una rotación que no cambia la orientación del objeto.
 * 
 * Source: https://docs.unity3d.com/ScriptReference/Quaternion.html
 */
