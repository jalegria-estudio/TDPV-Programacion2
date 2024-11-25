using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Audio manager
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        ///// COMPONENTS /////
        protected PlayerData m_data = null;
        protected Rigidbody2D m_rigidBody = null;
        protected InputController m_inputController = null;
        protected SpriteRenderer m_spriteRenderer = null;
        protected Animator m_animator = null;
        protected AudioSource m_audioSourcer = null;

        protected void Awake()
        {
            this.m_audioSourcer = this.gameObject.GetComponent<AudioSource>();
            this.m_rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            this.m_inputController = this.gameObject.GetComponent<InputController>();
            this.m_data = this.gameObject.GetComponent<Player>().Data;

            this.configEvents();
        }

        protected void OnDestroy()
        {
            JumpManager l_jumper = this.m_inputController.GetComponent<JumpManager>();

            if (l_jumper != null)
            {
                l_jumper.EVT_JUMP -= this.OnJumpSfx;
            }

            ///Stomp Event///
            if (this.gameObject.GetComponent<Player>() != null)
                this.gameObject.GetComponent<Player>().EVT_STOMP -= this.OnStompSfx;

            /// 1-UP, SCORE POINT, TIME SCORE POINT///
            if (this.gameObject.GetComponent<ExperienceManager>() != null)
            {
                this.gameObject.GetComponent<ExperienceManager>().EVT_1UP -= this.On1UpSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE -= this.OnScoreSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE_TIME -= this.OnScoreTimeSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE_BOSS -= this.OnScoreBossSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_HISCORE -= this.OnHiScoreSfx;
            }


        }

        /// <summary>
        /// Suscribe to events of observer pattern
        /// </summary>
        protected void configEvents()
        {
            ///Jump Event///
            JumpManager l_jumper = this.m_inputController.GetComponent<JumpManager>();
            if (l_jumper != null)
            {
                l_jumper.EVT_JUMP += this.OnJumpSfx;
            }

            ///Stomp Event///
            if (this.gameObject.GetComponent<Player>() != null)
                this.gameObject.GetComponent<Player>().EVT_STOMP += this.OnStompSfx;

            /// 1-UP ///
            if (this.gameObject.GetComponent<ExperienceManager>() != null)
            {
                this.gameObject.GetComponent<ExperienceManager>().EVT_1UP += this.On1UpSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE += this.OnScoreSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE_TIME += this.OnScoreTimeSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_SCORE_BOSS += this.OnScoreBossSfx;
                this.gameObject.GetComponent<ExperienceManager>().EVT_HISCORE += this.OnHiScoreSfx;
            }
        }

        /// <summary>
        /// Play one-shot for a audio clip
        /// </summary>
        /// <param name="p_audio">Audio Clip Object</param>
        /// <returns>Return false if audio source is playing other sound</returns>
        public bool Play(AudioClip p_audio)
        {
            if (this.m_audioSourcer.isPlaying)
                return false;

            this.m_audioSourcer.PlayOneShot(p_audio);
            return true;
        }

        /// <summary>
        /// Stop a audio clip
        /// </summary>
        /// <returns>Return false if audio source isn't playing a sound</returns>
        public bool Stop()
        {
            if (!this.m_audioSourcer.isPlaying)
                return false;

            this.m_audioSourcer.Stop();
            return true;
        }

        /// <summary>
        /// Wrapper - Indicate if it's playing a audio clip 
        /// </summary>
        /// <returns>Return false if audio source isn't playing a sound</returns>
        public bool IsPlayingClip()
        {
            return this.m_audioSourcer.isPlaying;
        }

        ///////////////////////////////////
        /// ACTIONS SOUNDS
        ///////////////////////////////////

        public void OnJumpSfx()
        {
            this.Play(this.m_data.SfxJump);
        }

        public void OnStompSfx()
        {
            this.Play(this.m_data.SfxStomp);
        }

        public void OnDamageSfx()
        {
            this.Play(this.m_data.SfxDamage);
        }

        public void OnDefeatSfx()
        {
            this.Play(this.m_data.SfxDefeat);
        }

        public void On1UpSfx()
        {
            this.Play(this.m_data.Sfx1up);
        }

        public void OnScoreSfx()
        {
            //m_audioSourcer.Stop();
            this.Play(this.m_data.SfxScore);
        }

        public void OnScoreTimeSfx()
        {
            //m_audioSourcer.Stop();
            this.Play(this.m_data.SfxScoreTime);
        }
        public void OnScoreBossSfx()
        {
            //m_audioSourcer.Stop();
            this.Play(this.m_data.SfxScoreBoss);
        }

        public void OnHiScoreSfx()
        {
            //m_audioSourcer.Stop();
            this.Play(this.m_data.SfxHiScore);
        }
    }
} //namespace Managers

/**
 * O B S E RV E R PAT T E R N
 * At runtime, any number of things can occur in your game. What happens when
 * you destroy an enemy? How about when you collect a power-up or complete an
 * object? You often need a mechanism that allows some objects to notify others
 * without directly referencing them, thereby creating unnecessary dependencies.
 * The observer pattern is a common solution to this sort of problem. It allows
 * your objects to communicate but stay loosely coupled using a “one-to-many”
 * dependency. When one object changes states, all dependent objects get
 * notified automatically. This is analogous to a radio tower that broadcasts to
 * many different listeners.
 * 
 * The object that is broadcasting is called the subject. The other objects that are
 * listening are called the observers.
 * This pattern loosely decouples the subject, which doesn’t really know the
 * observers or care what they do once they receive the signal. While the
 * observers have a dependency on the subject, the observers themselves don’t
 * know about each other.
 * 
 * Events concepts:
 * An event is simply a notification that indicates something has happened. It
involves a few parts:
 *  -The publisher (the subject) creates an event based on a delegate,
 *      establishing a specific function signature. The event is just some action
 *      that the subject will perform at runtime (e.g., take damage, click a button,
 *      and so on).
 *  -The subscribers (the observers) then each make a method called an event
 *      handler, which must match the delegate’s signature.
 *   -Each observer’s event handler subscribes to the publisher’s event. You can
 *      have as many observers join the subscription as necessary. All of them will
 *      wait for the event to trigger.
 *   -When the publisher signals the occurrence of an event at runtime, you
 *      say that it raises the event. This, in turn, invokes the subscribers’ event
 *      handlers, which run their own internal logic in response.
 *      
 * In this way, you make many components react to a single event from the
 * subject. If the subject indicates that a button is clicked, the observers could play
 * back an animation or sound, trigger a cutscene, or save a file. Their response
 * could be anything, which is why you’ll frequently find the observer pattern used
 * to send messages between objects.
 * 
 * Source: LEVEL UP YOUR CODE WITH GAME PROGRAMMING PATTERNS - Unity Doc 2022.
 */
