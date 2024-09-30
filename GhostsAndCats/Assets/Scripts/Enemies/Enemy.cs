using UnityEngine;

[RequireComponent(typeof(EnemyGenericHandler), typeof(EnemyAnimeManager))]
public class Enemy : MonoBehaviour
{
    protected EnemyGenericHandler m_genericHandler = null;
    protected EnemyAnimeManager m_animeManager = null;
    protected Items.Spawner m_itemSpawner = null;

    public void OnEnable()
    {
        m_genericHandler = GetComponent<EnemyGenericHandler>();
        m_animeManager = GetComponent<EnemyAnimeManager>();
        m_itemSpawner = GameObject.FindFirstObjectByType<Items.Spawner>();
        m_genericHandler.setupMoveDirection();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        m_genericHandler.updateMovePerformance();
        m_animeManager.UpdateDirection(m_genericHandler.MoveDirection);
    }

    public void OnDefeat()
    {
        m_animeManager.HandleDefeat();
    }

    public void OnDefeatedExit()
    {
        m_itemSpawner.Spawn(this.gameObject.transform.position);
        this.gameObject.SetActive(false);
    }
}
