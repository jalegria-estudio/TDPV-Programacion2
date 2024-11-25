using System.Collections;
using UnityEngine;

namespace System.Game.Services
{
    /// <summary>
    /// Message Sender Game Service
    /// </summary>
    public class SmsService : MonoBehaviour
    {

        [SerializeField]
        protected GameObject m_panel;
        [SerializeField]
        protected UnityEngine.UI.Text m_message;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Show a message on screen
        /// </summary>
        /// <param name="p_message">Text to show</param>
        /// <param name="p_duration">Message duration in seconds</param>
        public void SendSMS(string p_message, float p_duration)
        {
            this.StartCoroutine(this.ShowSMS(p_message, p_duration));
        }

        /// <summary>
        /// Low-Level - Show a message on screen
        /// </summary>
        /// <param name="p_smsTxt">Text to show</param>
        /// <param name="p_duration">Message duration in seconds</param>
        /// <returns></returns>
        protected IEnumerator ShowSMS(string p_smsTxt, float p_duration)
        {
            this.Publish(p_smsTxt);
            yield return new WaitForSeconds(p_duration);

            this.Cease();
        }

        /// <summary>
        /// Indefinitely show a message on screen
        /// </summary>
        /// <param name="p_sms"></param>
        public void Publish(string p_sms)
        {
            this.m_message.text = p_sms;
            this.m_panel.SetActive(true);
        }

        /// <summary>
        /// Remove a message on screen
        /// </summary>
        public void Cease()
        {
            this.m_panel.SetActive(false);
        }
    }
} //namespace System.Game.Services
