using UnityEngine;

namespace System.Game.Services
{
    /// <summary>
    /// Audio's Game Service 
    /// </summary>
    public class AudioService : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource m_jukebox = null;

        // Start is called before the first frame update
        void Start()
        {
            //m_jukebox = this.gameObject.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Play a music audio (in loop)
        /// </summary>
        /// <param name="p_audioClip"></param>
        public void PlayMusic(AudioClip p_audioClip)
        {
            Debug.Assert(this.m_jukebox != null, "<DEBUG ASSERT> AudioSource component is null!");

            this.m_jukebox.loop = true;
            this.m_jukebox.clip = p_audioClip;
            this.m_jukebox.Play();
        }

        /// <summary>
        /// Stop playing music
        /// </summary>
        public void StopMusic()
        {
            Debug.Assert(this.m_jukebox != null, "<DEBUG ASSERT> AudioSource component is null!");

            this.m_jukebox.Stop();
            this.m_jukebox.loop = false;
            this.m_jukebox.clip = null;
        }

        /// <summary>
        /// Play a audio clip one-time
        /// </summary>
        /// <param name="p_audioClip"></param>
        public void PlayOneShot(AudioClip p_audioClip)
        {
            Debug.Assert(this.m_jukebox != null, "<DEBUG ASSERT> AudioSource component is null!");

            this.m_jukebox.PlayOneShot(p_audioClip);
        }
    }
} //namespace System.Game.Services