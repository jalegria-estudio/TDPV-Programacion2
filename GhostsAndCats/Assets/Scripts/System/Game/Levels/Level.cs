using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    /// <summary>
    /// A wrapper to allocate level data
    /// </summary>
    [SerializeField] protected LevelData m_data;

    public LevelData Data { get => m_data; }
}
