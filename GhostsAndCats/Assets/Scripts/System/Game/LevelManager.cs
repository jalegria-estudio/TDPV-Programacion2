using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static Cinemachine.DocumentationSortingAttribute;

namespace System.Game
{
    public class LevelManager : MonoBehaviour // Equivalente BikeController
    {
        [Header("Configuration")]
        protected int m_currentLevel = 0;
        protected const int m_QTY_LEVELS = 2;
        protected List<GameObject> m_levels = new List<GameObject>(); //<(i) Source: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1

        public int CurrentLevel
        {
            get => m_currentLevel;
            set => m_currentLevel = value; //<(!) Danger code
        }
        // Start is called before the first frame update
        void Start()
        {
            //<(!!!) Hardcodeo => Refactoring!
            m_levels.Add(GameObject.Find("Level1-1"));
            m_levels.Add(GameObject.Find("Level1-2"));

            foreach (GameObject l_level in m_levels)
            {
                l_level.SetActive(false);
            }

            Active(0);
        }

        public void Active(int p_levelID)
        {
            m_levels[p_levelID].SetActive(true);
        }

        public void Inactive(int p_levelID)
        {
            m_levels[p_levelID].SetActive(false);
        }

        public bool NextLevel(out GameObject p_level)
        {
            if ((m_currentLevel + 1) < m_levels.Count)
            {
                Inactive(m_currentLevel);
                m_currentLevel++;
                p_level = m_levels[m_currentLevel];
                Active(m_currentLevel);
                return true;
            }

            p_level = new GameObject();
            return false;
        }

    }
} //namespace System.Game
