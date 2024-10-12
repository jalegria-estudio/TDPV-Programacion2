using Managers;
using Settings;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected float m_disappearTime = Config.ITEM_DISAPPEAR_TIME;
        [SerializeField] protected bool m_disappear = false;
        [SerializeField] protected bool m_idleAnimation = true;

        protected float m_elapsedTime = 0.0f;
        protected float m_initTime = 0.0f;
        protected AudioSource m_jukebox = null;
        protected Animator m_animator = null;
        protected bool m_collected = false;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = this.GetComponent<Animator>();
            m_jukebox = this.GetComponent<AudioSource>();
            m_initTime = Time.time;
            m_animator.SetBool("pIdle", m_idleAnimation);
        }

        // Update is called once per frame
        void Update()
        {
            m_elapsedTime = Time.time - m_initTime;

            ControlDisappear();
            ControlCollected();
        }

        protected void OnTriggerEnter2D(Collider2D p_collision)
        {
            if (p_collision.CompareTag("tPlayer"))
            {
                OnCollect();
            }
        }

        public void OnDisappear()
        {
            this.gameObject.SetActive(false);
        }

        public void OnCollect()
        {
            m_jukebox.Play();
            m_collected = true;
        }

        protected void ControlDisappear()
        {
            if (!m_disappear)
            {
                return;
            }
            else if (m_elapsedTime >= m_disappearTime)
            {
                m_animator.SetTrigger("pDisappear");
            }
        }

        protected void ControlCollected()
        {
            if (this.gameObject.activeSelf && m_collected && !m_jukebox.isPlaying)
                this.gameObject.SetActive(false);
        }
    }

} // namespace Items
