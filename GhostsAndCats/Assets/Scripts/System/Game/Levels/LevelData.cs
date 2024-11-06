using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    ///////////////////////////////
    /// GENERAL DATA
    /////////////////////////////
    [SerializeField]
    [Tooltip("Level Name")]
    internal string m_name = "";
    [SerializeField]
    [Tooltip("Level")]
    internal int m_level = 0;
    [SerializeField]
    [Tooltip("Stage")]
    internal int m_sublevel = 0;

    ///////////////////////////////
    /// LEVEL SOUL COST
    /////////////////////////////
    [SerializeField]
    [Tooltip("How many defeated enemies are required to finish the level?")]
    [Range(0, 100)]
    internal int m_requiredSoulsQty = 0;

    ///////////////////////////////
    /// SPAWN-START POINT
    /////////////////////////////
    [SerializeField]
    [Tooltip("Player Spawn Position")]
    protected Vector2 m_startPoint = new Vector2(0, 0);

    public Vector2 StartPoint
    {
        get => m_startPoint;
    }

    ///////////////////////////////////
    /// GOAL - EXIT POINT (!) Unused
    //////////////////////////////////
    [SerializeField]
    [Tooltip("Stage Goal - Unused")]
    protected Vector2 m_goalPoint = new Vector2(10, 10);

    public Vector2 GoalPoint
    {
        get => m_goalPoint;
    }

    ///////////////////////////////
    /// DEFAULT CAMERA ON START
    /////////////////////////////
    [SerializeField]
    [Tooltip("Default camera position when the level starts")]
    protected Vector3 m_defaultCameraPosition = new Vector3(0, 0, 0);

    public Vector3 CameraPosition
    {
        get => m_defaultCameraPosition;
        set => m_defaultCameraPosition = value;
    }
}

///////////////////////////////////////
/// EXTENSIVE NOTE INFO
//////////////////////////////////////
/**
 * ScriptableObject
 * A ScriptableObject is a data container that you can use to save large amounts of data,
 * independent of class instances. One of the main use cases for ScriptableObjects is to reduce your 
 * Project’s memory usage by avoiding copies of values. This is useful if your Project has a Prefab
 * that stores unchanging data in attached MonoBehaviour scripts.
 * 
 * Using a ScriptableObject
    The main use cases for ScriptableObjects are:
        Saving and storing data during an Editor session
        Saving data as an Asset in your Project to use at run time
 * To use a ScriptableObject, create a script in your application’s Assets
 * folder and make it inherit from the ScriptableObject class. You can use the CreateAssetMenu attribute
 * to make it easy to create custom assets using your class.
 * */
