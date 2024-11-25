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
    /// LEVEL TIME
    /////////////////////////////
    [SerializeField]
    [Tooltip("Finish the level before the time")]
    [Range(0, 999)]
    internal int m_time = 300;
    public int Time { get => m_time; set => m_time = value; }
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

    /////////////////////
    /// STAGE CLEAR
    ////////////////////
    [SerializeField]
    [Tooltip("Show Stage Clear Report")]
    protected bool m_showReport = false;
    public bool ShowStageClear
    {
        get => m_showReport;
        set => m_showReport = value;
    }

    [SerializeField]
    [Tooltip("Audio Clip for gameplay")]
    protected AudioClip m_audioClipGameplay = null;
    public AudioClip AudioClipGameplay
    {
        get => m_audioClipGameplay;
        set => m_audioClipGameplay = value;
    }
    /////////////////////
    /// STAGE: ROUND
    ////////////////////
    [Header("ROUND STAGE CONFIGURATION")]

    [SerializeField]
    [Tooltip("Show Round Stage Report")]
    protected bool m_showStageRound = false;
    public bool ShowStageRound
    {
        get => m_showStageRound;
        set => m_showStageRound = value;
    }

    [SerializeField]
    [Tooltip("Audio Clip Round Stage Report")]
    protected AudioClip m_audioClipStageRound = null;
    public AudioClip AudioClipStageRound
    {
        get => m_audioClipStageRound;
        set => m_audioClipStageRound = value;
    }

    [Tooltip("Waiting time in seconds Round Stage")]
    [Range(1.0f, 10.0f)]
    [SerializeField]
    protected float m_waitTime = 3.5f;
    public float WaitTime { get => m_waitTime; set => m_waitTime = value; }

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
