using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Trade and experience manager
    /// </summary>
    public class ExperienceManager : MonoBehaviour
    {
        ///// EVENTS AVAILABLES /////
        public event System.Action EVT_1UP;

        protected Player m_player = null;
        protected HudManager m_hud = null;
        protected PlayerData m_playerData = null;
        protected LevelData m_levelData = null;
        public LevelData LevelData { get => m_levelData; set => m_levelData = value; }

        // Start is called before the first frame update
        void Start()
        {
            /// <(i) Load Player
            if (!gameObject.TryGetComponent<Player>(out m_player))
            {
                Debug.LogError("<DEBUG> Player component isn't loaded!");
            }

            m_playerData = m_player.Data;
            m_player.EVT_COLLECT_TUNA += OnCollectTuna;
            m_player.EVT_COLLECT_SOUL += OnCollectSoul;

            m_hud = FindFirstObjectByType<HudManager>();
            if (m_hud == null)
            {
                Debug.LogWarning("<DEBUG> HudManager not found!");
            }

        }

        private void OnDestroy()
        {
            if (m_player != null)
            {
                m_player.EVT_COLLECT_TUNA -= OnCollectTuna;
                m_player.EVT_COLLECT_SOUL -= OnCollectSoul;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (CanTrade1Up())
                ExchangeTunas(m_playerData.TunasCost1up);

            if (IsReadyGoal())
                TurnOnGoal();
            else
                TurnOffGoal();

            if (m_hud != null)
            {
                m_hud.UpdateData(m_playerData, m_levelData);
            }
        }

        /// <summary>
        /// Indicate if the players tunas to trade for 1UP
        /// </summary>
        /// <returns></returns>
        public bool CanTrade1Up()
        {
            return (m_playerData.Tunas >= m_playerData.TunasCost1up);
        }

        /// <summary>
        /// Trade tunas for power-up
        /// </summary>
        /// <param name="p_tunasQty"></param>
        public void ExchangeTunas(int p_tunasQty)
        {
            if (p_tunasQty == m_playerData.TunasCost1up)
            {
                m_playerData.RemoveTunas(m_playerData.TunasCost1up);
                m_playerData.AddLifes();
                EVT_1UP?.Invoke();
            }
        }

        /// <summary>
        /// Indicate if the goal trigger region is ready to next level
        /// </summary>
        public bool IsReadyGoal()
        {
            if (m_levelData == null || m_playerData == null)
                return false;

            return (m_playerData.Souls >= m_levelData.m_requiredSoulsQty);
        }

        /// <summary>
        /// Active ready goal animation
        /// </summary>
        public void TurnOnGoal()
        {
            //m_levelData.m_readyGoal = true;
            GameObject l_goal = GameObject.FindWithTag("tGoal");
            if (l_goal != null)
                l_goal.GetComponent<Animator>().SetBool("pGoalReady", true);
        }

        /// <summary>
        /// Active unready goal animation
        /// </summary>
        private void TurnOffGoal()
        {
            GameObject l_goal = GameObject.FindWithTag("tGoal");
            if (l_goal != null)
                l_goal.GetComponent<Animator>().SetBool("pGoalReady", false);
        }

        /// <summary>
        /// Actions when the player collects a tuna
        /// </summary>
        protected void OnCollectTuna()
        {
            m_playerData.AddTunas();

        }

        /// <summary>
        /// Actions when the player collects a soul
        /// </summary>
        protected void OnCollectSoul()
        {
            m_playerData.AddSouls();

        }


        /// <summary>
        /// Simple Render Status Player
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(100, 80, 200, 30), "< PLAYER STATUS >");
            GUI.Label(new Rect(100, 100, 100, 30), $"TUNAS: {m_playerData.Tunas}");
            GUI.Label(new Rect(100, 120, 100, 30), $"SOULS: {m_playerData.Souls}");
            GUI.Label(new Rect(100, 140, 100, 30), $"Lifes: {m_playerData.Lifes}");
            GUI.Label(new Rect(100, 160, 200, 30), "Press Key <H> for controls.");
        }
    }
} //Namespace Managers
