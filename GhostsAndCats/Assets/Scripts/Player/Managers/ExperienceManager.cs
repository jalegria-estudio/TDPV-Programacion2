#define GAME_DEBUG
#undef GAME_DEBUG

using System.Collections;
using System.Game;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Player's trade and experience manager
    /// </summary>
    public class ExperienceManager : MonoBehaviour
    {
        ///// EVENTS AVAILABLES /////
        public event System.Action EVT_1UP;
        public event System.Action EVT_SCORE;
        public event System.Action EVT_SCORE_TIME;
        public event System.Action EVT_SCORE_BOSS;
        public event System.Action EVT_HISCORE;

        protected Player m_player = null;
        protected HudManager m_hud = null;
        protected PlayerData m_playerData = null;
        protected LevelData m_levelData = null;

        protected int m_LifesExchanged = 0;
        /// <summary>
        /// Indicate if Score Counting (on Stage Clear) is performing 
        /// </summary>
        protected bool m_countingScoreFlag = false;

        /////////////////////////
        /// GETTERS & SETTERS
        ///////////////////////
        public LevelData LevelData { get => this.m_levelData; set => this.m_levelData = value; }

        // Start is called before the first frame update
        void Start()
        {
            /// <(i) Load Player
            if (!this.gameObject.TryGetComponent<Player>(out this.m_player))
            {
                Debug.LogError("<DEBUG> Player component isn't loaded!");
            }

            this.m_playerData = this.m_player.Data;
            this.m_player.EVT_COLLECT_TUNA += this.OnCollectTuna;
            this.m_player.EVT_COLLECT_SOUL += this.OnCollectSoul;
            this.m_player.EVT_GOAL += this.OnGoalRecount;

            this.m_hud = FindFirstObjectByType<HudManager>();
            if (this.m_hud == null)
            {
                Debug.LogWarning("<DEBUG> HudManager not found!");
            }

            this.m_playerData.ResetDefault();
        }

        private void OnDestroy()
        {
            if (this.m_player != null)
            {
                this.m_player.EVT_COLLECT_TUNA -= this.OnCollectTuna;
                this.m_player.EVT_COLLECT_SOUL -= this.OnCollectSoul;
                this.m_player.EVT_GOAL -= this.OnGoalRecount;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (CanTrade1Up())
            //    ExchangeTunasOnRuntime();

            if (!this.m_countingScoreFlag)
                this.GoalStatusUpdate();

            if (this.m_hud != null)
            {
                this.m_hud.UpdateData(this.m_playerData, this.m_levelData);
            }
        }


        /// <summary>
        /// Indicate if the players tunas to trade for 1UP
        /// </summary>
        /// <returns></returns>
        public bool CanTrade1Up()
        {
            return (this.m_playerData.Tunas >= this.m_playerData.TunasCost1up);
        }

        /// <summary>
        /// Trade tunas by power-up lifes in runtime -OBSOLETO
        /// </summary>
        public void ExchangeTunasOnRuntime()
        {
            int l_tunasQty = this.m_playerData.Tunas;
            float l_lifesAvailableToChange = l_tunasQty / this.m_playerData.TunasCost1up;

            if ((l_tunasQty % 2 == 0) && (l_lifesAvailableToChange > this.m_LifesExchanged))
            {
                this.m_LifesExchanged += 1;
                this.m_playerData.AddLifes();
                EVT_1UP?.Invoke();
            }
        }

        /// <summary>
        /// Trade tunas for power-up
        /// </summary>
        public void Exchange1up()
        {
            this.m_player.Jukebox.Stop();
            EVT_1UP?.Invoke();
            this.m_playerData.AddLifes();
        }

        /// <summary>
        /// Trade tunas for power-up -OBSOLETO
        /// </summary>
        /// <param name="p_tunasQty"></param>
        public void TradeTunas(int p_tunasQty)
        {

            if (p_tunasQty == this.m_playerData.TunasCost1up)
            {
                this.m_playerData.RemoveTunas(this.m_playerData.TunasCost1up);
                this.Exchange1up();
            }
        }

        /// <summary>
        /// Update goal status
        /// </summary>
        protected void GoalStatusUpdate()
        {
            if (this.IsReadyGoal())
                this.TurnOnGoal();
            else
                this.TurnOffGoal();
        }

        /// <summary>
        /// Indicate if the goal trigger region is ready to next level
        /// </summary>
        public bool IsReadyGoal()
        {
            if (this.m_levelData == null || this.m_playerData == null)
                return false;

            return (this.m_playerData.Souls >= this.m_levelData.m_requiredSoulsQty);
        }

        /// <summary>
        /// Active ready goal animation
        /// </summary>
        public void TurnOnGoal()
        {
            //m_levelData.m_readyGoal = true;
            GameObject l_goal = GameObject.FindWithTag("tGoal");
            if (l_goal != null && !l_goal.GetComponent<Animator>().GetBool("pGoalReady"))
            {
                GameManager.Instance.SmsService.SendSMS("Find the Jack-O'-Lantern!", 5.0f);
                l_goal.GetComponent<Animator>().SetBool("pGoalReady", true);
            }
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
            this.m_playerData.AddTunas();
            this.m_playerData.Score += this.m_playerData.TunaScore;
        }

        /// <summary>
        /// Actions when the player collects a soul
        /// </summary>
        protected void OnCollectSoul()
        {
            this.m_playerData.AddSouls();
            this.m_playerData.Score += this.m_playerData.SoulScore;
        }

        /// <summary>
        /// Start scoring counting on stage-clear
        /// </summary>
        public void OnGoalRecount()
        {
            GameManager.Instance.LevelManager.GetLevel().StopGameplay();
            this.StartCoroutine(nameof(RecountItems), false);
        }

        /// <summary>
        /// Recount collected items, remaining time and update the player's score
        /// </summary>
        /// <returns></returns>
        protected IEnumerator RecountItems(bool p_boss = false)
        {
            this.m_player.DisableActions();
            Level l_lvl = GameManager.Instance.LevelManager.GetLevel();
            int l_time = (int)GameManager.Instance.TimerService.RemainingTime;

            ///<(!) Show sms about stage clear and play success melody
            GameManager.Instance.SmsService.Publish($"Stage {this.m_levelData.m_level}-{this.m_levelData.m_sublevel} Clear");
            this.m_player.Jukebox.Stop();
            this.m_player.Jukebox.Play(this.m_playerData.SfxStageClear);
            while (this.m_player.Jukebox.IsPlayingClip())
            {
                yield return new WaitForEndOfFrame();
            }

            /// COUNT BOSS
            if (p_boss)
            {
                int l_bossScoring = this.m_playerData.KnockOutScore;

                this.m_player.Jukebox.Stop();
                EVT_SCORE_BOSS?.Invoke(); //<(!) This invoke to audio observer for play sfx-add-score
                while (l_bossScoring > 0)
                {
                    this.m_playerData.Score += l_bossScoring;
                    l_bossScoring = 0;

                    yield return new WaitForSeconds(this.m_playerData.SfxScoreBoss.length);//<(i) Alternative recount but doesn't well on WebGL platform: this.m_playerData.SfxScoreBoss.length / this.m_playerData.KnockOutScore
                }

                /// LIFES SOULS
                this.m_player.Jukebox.Stop();
                while (this.m_playerData.Lifes > 0)
                {
                    EVT_1UP?.Invoke();//<(!) This invoke to audio observer for play sfx-add-score
                    this.m_playerData.Score += this.m_playerData.LifeScore;
                    this.m_playerData.RemoveLifes(1);
                    yield return new WaitForSeconds(this.m_playerData.Sfx1up.length);
                }
            }

            /// COUNT SOULS
            this.m_player.Jukebox.Stop();
            while (this.m_playerData.Souls > 0)
            {
                EVT_SCORE?.Invoke();//<(!) This invoke to audio observer for play sfx-add-score
                this.m_playerData.Score += this.m_playerData.SoulScore;
                this.m_playerData.RemoveSouls(1);
                yield return new WaitForSeconds(0.03f);
            }

            /// COUNT TUNAS
            this.m_player.Jukebox.Stop();
            while (this.m_playerData.Tunas > 0)
            {
                this.m_playerData.Score += this.m_playerData.TunaScore;
                if ((this.m_playerData.Tunas % this.m_playerData.TunasCost1up) == 0)
                    this.Exchange1up();
                else
                    EVT_SCORE?.Invoke(); //<(!) This invoke to audio observer for play sfx-add-score

                this.m_playerData.RemoveTunas(1);

                yield return new WaitForSeconds(0.03f);
            }

            /// COUNT REMAINING TIME
            this.m_player.Jukebox.Stop();
            while (l_time > 0)
            {
                EVT_SCORE_TIME?.Invoke();
                this.m_playerData.Score += this.m_playerData.TimeScore;
                GameManager.Instance.TimerService.RemoveTimer(1);
                l_time--;
                yield return new WaitForFixedUpdate();
            }

            /// RESET FLAG
            this.m_LifesExchanged = 0;
            this.UpdateHiScore();

            yield return new WaitForSeconds(3.5f);

            GameManager.Instance.SmsService.Cease();
            GameManager.Instance.MoveNextLevel();
        }

        /// <summary>
        /// Count point on knock-out boss
        /// </summary>
        public void OnKnockOutRecount()
        {
            this.StartCoroutine(this.RecountItems(true));
        }

        /// <summary>
        /// Reset status
        /// </summary>
        public void Reset()
        {
            this.m_playerData.RemoveTunas(this.m_playerData.Tunas);
            this.m_playerData.RemoveSouls(this.m_playerData.Souls);
            this.m_LifesExchanged = 0;
        }

        /// <summary>
        /// Update hi-score data
        /// </summary>
        public void UpdateHiScore()
        {
            if (this.m_playerData.Score > this.m_playerData.HiScore)
            {
                this.m_player.Jukebox.Stop();
                this.m_playerData.HiScore = this.m_playerData.Score;
                this.EVT_HISCORE?.Invoke();
            }
        }

#if GAME_DEBUG
        /// <summary>
        /// Simple Render Status Player
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(100, 80, 200, 30), "< PLAYER STATUS >");
            GUI.Label(new Rect(100, 100, 100, 30), $"TUNAS: {this.m_playerData.Tunas}");
            GUI.Label(new Rect(100, 120, 100, 30), $"SOULS: {this.m_playerData.Souls}");
            GUI.Label(new Rect(100, 140, 100, 30), $"Lifes: {this.m_playerData.Lifes}");
            GUI.Label(new Rect(100, 160, 200, 30), "Press Key <H> for controls.");
        }
#endif
    }
} //Namespace Managers
