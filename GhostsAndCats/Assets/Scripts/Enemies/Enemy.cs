using UnityEngine;

/// <summary>
/// Generic Enemy class
/// </summary>
[RequireComponent(typeof(EnemyGenericHandler), typeof(EnemyAnimeManager))]
public class Enemy : MonoBehaviour
{
    protected EnemyGenericHandler m_genericHandler = null;
    protected EnemyAnimeManager m_animeManager = null;
    protected Items.Spawner m_itemSpawner = null;

    /// This function is called when the object becomes enabled and active.
    public void OnEnable()
    {
        m_genericHandler = GetComponent<EnemyGenericHandler>();
        m_animeManager = GetComponent<EnemyAnimeManager>();
        m_itemSpawner = GameObject.FindFirstObjectByType<Items.Spawner>();
        m_genericHandler.SetupMoveDirection();
    }

    /// Update is called once per frame
    public void FixedUpdate()
    {
        m_genericHandler.UpdateMovePerformance();
        m_animeManager.UpdateDirection(m_genericHandler.MoveDirection);
    }

    /// <summary>
    /// Actions when the enemy is defeated
    /// </summary>
    public void OnDefeat()
    {
        m_animeManager.HandleDefeat();
    }

    /// <summary>
    /// Actions after the enemy is defeated
    /// </summary>
    public void OnDefeatedExit()
    {
        m_itemSpawner.Spawn(this.gameObject.transform.position);
        this.gameObject.SetActive(false);
    }
}
