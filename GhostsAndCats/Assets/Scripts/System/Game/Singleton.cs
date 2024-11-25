using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T m_instance;

    /// <summary>
    /// Singleton Getter
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                /// Object.FindObjectOfType
                /// T Object => The first active loaded object that matches the specified type. It returns null if no Object matches the type.
                /// Source: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Object.FindObjectOfType.html
                m_instance = (T)FindObjectOfType(typeof(T));
                if (m_instance == null)
                {
                    SetupInstance();
                }
            }

            return m_instance;
        }
    }

    public virtual void Awake()
    {
        this.RemoveDuplicates();
    }

    /// <summary>
    /// Create and config the instance
    /// </summary>
    protected static void SetupInstance()
    {
        //m_instance = (T)FindObjectOfType(typeof(T));

        GameObject l_gameObject = new GameObject();
        l_gameObject.name = typeof(T).Name;
        m_instance = l_gameObject.AddComponent<T>();
        DontDestroyOnLoad(l_gameObject);
    }

    /// <summary>
    /// Destroys duplicates instances
    /// </summary>
    protected void RemoveDuplicates()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}