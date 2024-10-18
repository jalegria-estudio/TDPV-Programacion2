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
            m_audioSourcer = this.gameObject.GetComponent<AudioSource>();
            m_rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            m_inputController = this.gameObject.GetComponent<InputController>();
            m_data = this.gameObject.GetComponent<Player>().Data;

            configEvents();
        }

        protected void OnDestroy()
        {
            JumpManager l_jumper = m_inputController.GetComponent<JumpManager>();

            if (l_jumper != null)
            {
                l_jumper.EVT_JUMP -= OnJumpSfx;
            }
        }

        /// <summary>
        /// Suscribe to events of observer pattern
        /// </summary>
        protected void configEvents()
        {
            ///Jump Event///
            JumpManager l_jumper = m_inputController.GetComponent<JumpManager>();
            if (l_jumper != null)
            {
                l_jumper.EVT_JUMP += OnJumpSfx;
            }

            ///Stomp Event///
            this.gameObject.GetComponent<Player>().EVT_STOMP += OnStompSfx;
        }

        /// <summary>
        /// Play one-shot for a audio clip
        /// </summary>
        /// <param name="p_audio">Audio Clip Object</param>
        /// <returns>Return false if audio source is playing other sound</returns>
        public bool Play(AudioClip p_audio)
        {
            if (m_audioSourcer.isPlaying)
                return false;

            m_audioSourcer.PlayOneShot(p_audio);
            return true;
        }

        ///////////////////////////////////
        /// ACTIONS SOUNDS
        ///////////////////////////////////

        public void OnJumpSfx()
        {
            this.Play(m_data.SfxJump);
        }

        public void OnStompSfx()
        {
            this.Play(m_data.SfxStomp);
        }

        public void OnDamageSfx()
        {
            this.Play(m_data.SfxDamage);
        }

        public void OnDefeatSfx()
        {
            this.Play(m_data.SfxDefeat);
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
