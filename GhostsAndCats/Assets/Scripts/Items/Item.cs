using Settings;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected float m_disappearTime = Config.ITEM_DISAPPEAR_TIME;
        [SerializeField] protected float m_elapsedTime = 0.0f;
        protected float m_initTime = 0.0f;
        protected Animator m_animator = null;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = this.GetComponent<Animator>();
            m_initTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            m_elapsedTime = Time.time - m_initTime;

            if (m_elapsedTime >= m_disappearTime)
            {
                m_animator.SetTrigger("pDisappear");
            }
        }

        protected void OnCollisionEnter2D(Collision2D p_collision)
        {
            if (p_collision.collider.CompareTag("tPlayer"))
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
            this.gameObject.SetActive(false);
        }
    }

} // namespace Items
