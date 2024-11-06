using UnityEngine;

/// <summary>
/// Manager for enemy animations
/// </summary>
public class EnemyAnimeManager : MonoBehaviour
{
    [Header("Configuration")]
    protected Rigidbody2D m_rigidBody = null;
    protected SpriteRenderer m_spriteRenderer = null;
    protected Animator m_animator = null;

    // Called when the object becomes enabled and active, always after Awake (on the same object) and before any Start.
    //Source: https://docs.unity3d.com/Manual/ExecutionOrder.html
    protected void OnEnable()
    {
        m_animator = this.GetComponent<Animator>();
        m_rigidBody = this.GetComponent<Rigidbody2D>();
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Animations actions when enemy is defeated
    /// </summary>
    public void HandleDefeat()
    {
        m_animator.SetTrigger("pDefeated");
    }

    /// <summary>
    /// Update sprite enemy direction
    /// </summary>
    /// <param name="p_currentDir"> Where the enemy points</param>
    public void UpdateDirection(Vector2 p_currentDir)
    {
        m_spriteRenderer.flipX = (p_currentDir.x < 0);
    }
}
