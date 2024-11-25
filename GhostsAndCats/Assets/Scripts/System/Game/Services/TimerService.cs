using UnityEngine;

namespace System.Game.Services
{
    /// <summary>
    /// Simple Timer for gameplay. Note: It's in seconds!
    /// </summary>
    public class TimerService : MonoBehaviour
    {
        /////////////////////////
        /// EVENTS
        ////////////////////////
        public event System.Action EVT_OVER_TIME;

        /////////////////////////
        /// ATTRIBUTES
        ////////////////////////
        protected float m_currentTime = 0;
        public float CurrentTime { get => this.m_currentTime; protected set => this.m_currentTime = value; }

        protected float m_timerLimit = 0;
        public float TimerLimit
        {
            get => this.m_timerLimit;
            protected set
            {
                if (value < 0)
                    value = 0;

                this.m_timerLimit = value;
            }
        }

        protected bool m_on = false;
        public bool IsOn { get => this.m_on; }

        /////////////////////////
        /// METHODS
        ////////////////////////
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.Player.RegisterOverTime();
            this.m_currentTime = 0;
            this.InvokeRepeating(nameof(UpdateTimer), 1.0f, 1.0f); //<(!) Use nameof to get a reference method on ide
        }

        // Update is called once per frame
        void Update()
        {
            if (this.m_on && this.m_timerLimit <= this.m_currentTime) //<(!) Timer is over and the player is defeated!
            {
                EVT_OVER_TIME?.Invoke();
                GameManager.Instance.TimerService.ResetTimer();//<(!) Necessary to avoid player-loop-death
            }
        }

        /// <summary>
        /// Start timer control
        /// </summary>
        /// <param name="m_limit">Time limit in seconds</param>
        public bool PlayTimer(float p_limit)
        {
            if (p_limit <= 0)
                return false;

            this.m_timerLimit = p_limit;
            this.m_currentTime = 0;
            this.m_on = true;

            return true;
        }

        /// <summary>
        /// Update timer in 1sec
        /// </summary>
        public void UpdateTimer()
        {
            if (!this.m_on)
                return;

            this.m_currentTime++;
        }

        /// <summary>
        /// Reset timer to 0sec
        /// </summary>
        public void ResetTimer()
        {
            this.m_currentTime = 0;
        }

        /// <summary>
        /// Add a mount time to timer-time
        /// </summary>
        /// <param name="p_timeQty"></param>
        public void RemoveTimer(uint p_timeQty)
        {
            //m_currentTime += p_timeQty;

            //if (m_currentTime > Data.Time)
            //    m_currentTime = Data.Time;
            this.m_timerLimit -= p_timeQty;
            if (this.m_timerLimit < 0)
                this.m_timerLimit = 0;
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void StopTimer()
        {
            this.m_on = false;
        }

        /// <summary>
        /// Get remaining time
        /// </summary>
        public float RemainingTime
        {
            get
            {
                float l_remainingTime = this.m_timerLimit - this.m_currentTime;
                return (l_remainingTime < 0) ? 0 : l_remainingTime;
            }
        }
    }
}//namespace System.Game.Services