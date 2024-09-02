using Toolbox;
using UnityEngine;

public class PlayerCollisionsHandler : MonoBehaviour
{
    public Vector2 m_spawnPos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Bounds l_bounds = this.GetComponent<Collider2D>().bounds;
        Vector2 l_origin = new Vector2(l_bounds.min.x, l_bounds.min.y - 0.01f);
        RaycastHit2D l_raycasthitBottom = PhysicsTools.CreateRaycastHit(l_origin, Vector2.right, l_bounds.size.x);
        StompCollision(l_raycasthitBottom);
    }

    //Sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        Debug.Log($"Trigger entry game-object: Name->{p_collider.name}. Tag->{p_collider.tag}");

        if (p_collider.isTrigger && p_collider.CompareTag("tAbyss"))
        {
            this.PlayerLostLife();
        }
    }

    //Sent when an incoming collider makes contact with this object's collider (2D physics only).
    protected void OnCollisionEnter2D(Collision2D p_collision)
    {
        Debug.Log($"Touch a game-object: Name->{p_collision.collider.name}. Tag->{p_collision.collider.tag}");
        if (p_collision.collider.CompareTag("tEnemy"))
        {
            this.PlayerLostLife();
        }
    }

    /// <summary>
    /// Detects and resolves a hop on enemy's head for stomp it - "player attack"
    /// </summary>
    /// <param name="p_raycastHit">Player raycast-hit result </param>
    void StompCollision(RaycastHit2D p_raycastHit)
    {
        if (p_raycastHit.collider != null && p_raycastHit.collider.tag == "tEnemy")
        {
            Debug.Log($"Raycast touch! Object name:{p_raycastHit.collider} Tag: {p_raycastHit.collider.tag}");
            this.gameObject.GetComponent<PlayerInputHandler>().Plop(5.0f);
            p_raycastHit.collider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Actions when player lost a life
    /// </summary>
    protected void PlayerLostLife()
    {
        transform.position = m_spawnPos;
    }

}
