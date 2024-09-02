using Toolbox;
using UnityEngine;

public class EnemyGenericHandler : MonoBehaviour
{
    //public BoxCollider2D m_guidePath;
    public bool m_moveHorizontal = false;
    public bool m_moveVertical = false;
    public bool m_moveTopFoward = false;
    public bool m_moveTopBack = false;
    public bool m_moveWalk = false;
    //public bool m_follower = false;
    public float m_speed = 1.0f;
    public Vector2 m_moveDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (m_moveHorizontal && m_moveDir == Vector2.zero)
            m_moveDir = Vector2.right;

        else if (m_moveVertical && m_moveDir == Vector2.zero)
            m_moveDir = Vector2.up;

        else if (m_moveTopFoward && m_moveDir == Vector2.zero)
            m_moveDir = Vector2.one;

        else if (m_moveTopBack && m_moveDir == Vector2.zero)
            m_moveDir = new Vector2(-1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_moveHorizontal)
            AutomaticMoveH();
        else if (m_moveVertical)
            AutomaticMoveV();
        else if (m_moveTopFoward || m_moveTopBack)
            AutomaticMoveD(m_moveDir);
        else if (m_moveWalk)
            AutomaticWalk();
        else
            Debug.Log($"(!)DEBUG: The Enemy doesn't have a option to moving.");
    }

    /// <summary>
    /// Moves a sprite - Wrapper for transform.Translate()
    /// </summary>
    /// <param name="p_direction"></param>
    /// <param name="p_speed"></param>
    public void MoveTo(Vector2 p_direction, float p_speed)
    {
        this.transform.Translate(p_direction * m_speed * Time.deltaTime);
    }

    /// <summary>
    /// Moves a enemy on a floor
    /// </summary>
    /// <returns></returns>
    public bool AutomaticWalk()
    {
        this.MoveTo(m_moveDir, m_speed);
        return true;
    }

    /// <summary>
    /// Moves a enemy on horizontal direction
    /// </summary>
    /// <returns></returns>
    public bool AutomaticMoveH()
    {
        RaycastHit2D l_raycastHitLeft = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Left, 0.1f);
        RaycastHit2D l_raycastHitRight = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Right, 0.1f);

        if (l_raycastHitLeft.collider == null)
        {
            this.m_moveDir = Vector2.right;
        }
        else if (l_raycastHitRight.collider == null)
        {
            this.m_moveDir = Vector2.left;
        }

        this.MoveTo(m_moveDir, m_speed);
        return true;
    }

    /// <summary>
    /// Moves a enemy on vertical direction
    /// </summary>
    /// <returns></returns>
    public bool AutomaticMoveV()
    {
        RaycastHit2D l_raycastHitTop = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Up, 0.1f);
        RaycastHit2D l_raycastHitBottom = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Down, 0.1f);

        if (l_raycastHitTop.collider == null)
        {
            this.m_moveDir = Vector2.down;
        }
        else if (l_raycastHitBottom.collider == null)
        {
            this.m_moveDir = Vector2.up;
        }

        this.MoveTo(m_moveDir, m_speed);
        return true;
    }

    /// <summary>
    /// Moves a enemy on diagonal direction
    /// </summary>
    /// <param name="p_originDPoint">Diagonal bound-box point </param>
    /// <returns></returns>
    public bool AutomaticMoveD(Vector2 p_originDPoint)
    {
        Vector2 l_diagonalPointTop = Vector2.zero;
        Vector2 l_diagonalPointBottom = Vector2.zero;
        Bounds l_enemyBounds = this.GetComponent<BoxCollider2D>().bounds;
        Vector2 l_dir = Vector2.one;

        //<(i) Config the raycast point origin
        l_diagonalPointTop.y = l_enemyBounds.max.y;
        l_diagonalPointBottom.y = l_enemyBounds.min.y;

        if ((p_originDPoint.x > 0 && p_originDPoint.y > 0) || (p_originDPoint.x < 0 && p_originDPoint.y < 0))
        {
            l_diagonalPointTop.x = l_enemyBounds.max.x;
            l_diagonalPointBottom.x = l_enemyBounds.min.x;
        }
        else if ((p_originDPoint.x < 0 && p_originDPoint.y > 0) || (p_originDPoint.x > 0 && p_originDPoint.y < 0))
        {
            l_dir.x = -1;
            l_diagonalPointTop.x = l_enemyBounds.min.x;
            l_diagonalPointBottom.x = l_enemyBounds.max.x;
        }
        else
        {
            Debug.Assert(true, $"Some value from Vector2 Direction movement {p_originDPoint} is empty!");
        }

        RaycastHit2D l_raycastHitTop = Toolbox.PhysicsTools.CreateRaycastHit(l_diagonalPointTop, l_dir, 0.1f);
        RaycastHit2D l_raycastHitBottom = Toolbox.PhysicsTools.CreateRaycastHit(l_diagonalPointBottom, (-1 * l_dir), 0.1f);

        //<(i) Change polarity move
        if (l_raycastHitTop.collider == null || l_raycastHitBottom.collider == null)
        {
            this.m_moveDir *= -1;
        }

        this.MoveTo(m_moveDir, m_speed);
        return true;
    }
}
