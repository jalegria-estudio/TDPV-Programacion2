using Movements;
using Toolbox;
using UnityEngine;

public class EnemyGenericHandler : MonoBehaviour
{
    public enum MoveType
    {
        GuidedHorizontal,
        GuidedVertical,
        GuidedTopFoward,
        GuidedTopBack,
        Simple,
        Automata
    }

    /**
     * Attributes info: https://docs.unity3d.com/Manual/Attributes.html; https://docs.unity3d.com/ScriptReference/ => unity engine => attributes
     */
    [Header("CONFIG")]
    [SerializeField] protected MoveType m_moveType = MoveType.Automata;
    [SerializeField] protected float m_speed = 1.0f;
    [SerializeField] protected Vector2 m_moveDir = Vector2.zero;
    protected int m_automataToken = 0;

    public Vector2 MoveDirection { get => m_moveDir; set => m_moveDir = value; }

    //Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    protected void OnValidate()
    {
        //Don't delete
    }

    public void setupMoveDirection()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        if (m_moveType == MoveType.GuidedHorizontal && (m_moveDir == Vector2.zero || m_moveDir != Vector2.right || m_moveDir != Vector2.left))
            m_moveDir = Vector2.right;

        else if (m_moveType == MoveType.GuidedVertical && (m_moveDir == Vector2.zero || m_moveDir != Vector2.up || m_moveDir != Vector2.down))
            m_moveDir = Vector2.up;

        else if (m_moveType == MoveType.GuidedTopFoward && (m_moveDir == Vector2.zero || m_moveDir != Vector2.one || m_moveDir != Vector2.one * -1))
            m_moveDir = Vector2.one;

        else if (m_moveType == MoveType.GuidedTopBack && (m_moveDir == Vector2.zero || (m_moveDir.x != -1 && m_moveDir.y != 1) || (m_moveDir.x != 1 && m_moveDir.y != -1)))
            m_moveDir = new Vector2(-1, 1);
    }

    public void updateMovePerformance()
    {
        if (m_moveType == MoveType.GuidedHorizontal)
            HandleGuidedMoveH();
        else if (m_moveType == MoveType.GuidedVertical)
            HandleGuidedMoveV();
        else if (m_moveType == MoveType.GuidedTopFoward || m_moveType == MoveType.GuidedTopBack)
            HandleGuidedMoveD(m_moveDir);
        else if (m_moveType == MoveType.Simple)
            handleSimpleMove();
        else
            HandleAutomataMove();
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
    /// Moves a enemy on a floor.
    /// </summary>
    /// <returns></returns>
    public bool handleSimpleMove()
    {
        this.MoveTo(m_moveDir, m_speed);
        return true;
    }

    /// <summary>
    /// Moves a enemy on horizontal direction.
    /// Note: It requiere a guide-path component(with collider2d) to follow.
    /// </summary>
    /// <returns></returns>
    public bool HandleGuidedMoveH()
    {
        LayerMask l_filter = LayerMask.GetMask("lPaths");
        RaycastHit2D l_raycastHitLeft = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Left, 0.1f, l_filter);
        RaycastHit2D l_raycastHitRight = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Right, 0.1f, l_filter);

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
    /// Moves a enemy on vertical direction.
    /// Note: It requiere a guide-path component(with collider2d) to follow.
    /// </summary>
    /// <returns></returns>
    public bool HandleGuidedMoveV()
    {
        LayerMask l_filter = LayerMask.GetMask("lPaths");
        RaycastHit2D l_raycastHitTop = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Up, 0.1f, l_filter);
        RaycastHit2D l_raycastHitBottom = PhysicsTools.CreateRaycastHitFrom(this.GetComponent<BoxCollider2D>().bounds, Toolbox.Direction.Down, 0.1f, l_filter);

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
    /// Moves a enemy on diagonal direction.
    /// Note: It requiere a guide-path component(with collider2d) to follow.
    /// </summary>
    /// <param name="p_originDPoint">Diagonal bound-box point </param>
    /// <returns></returns>
    public bool HandleGuidedMoveD(Vector2 p_originDPoint)
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

        LayerMask l_filter = LayerMask.GetMask("lPaths");
        RaycastHit2D l_raycastHitTop = Toolbox.PhysicsTools.CreateRaycastHit(l_diagonalPointTop, l_dir, 0.1f, l_filter);
        RaycastHit2D l_raycastHitBottom = Toolbox.PhysicsTools.CreateRaycastHit(l_diagonalPointBottom, (-1 * l_dir), 0.1f, l_filter);


        //<(i) Change polarity move
        if (l_raycastHitTop.collider == null || l_raycastHitBottom.collider == null)
        {
            this.m_moveDir *= -1;
        }

        this.MoveTo(m_moveDir, m_speed);
        return true;
    }

    /// <summary>
    /// Moves a enemy like a player automata follower
    /// Note: this collide with mask "lPlatforms" and "lWall" and a tag "tPlayer".
    /// </summary>
    void HandleAutomataMove()
    {
        /////// ASSERTS ///////
        Debug.Assert((GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic), "Automata Movement needs a Body Type Dynamic!");
        Debug.Assert((m_moveDir.x != 0), "Automata Movement needs a horizontal direction to move!");
        /////// END-ASSERTS ///////

        //Raycast Init
        Vector2 l_diagonalPointTop = Vector2.zero;
        Vector2 l_diagonalPointBottom = Vector2.zero;
        Bounds l_bounds = this.GetComponent<BoxCollider2D>().bounds;
        int l_layerMask = LayerMask.GetMask("lPlatforms");
        RaycastHit2D l_rhitRB = PhysicsTools.CreateRaycastHit(new Vector2(l_bounds.max.x, l_bounds.min.y), Vector2.down, PhysicsTools.m_rayDistDefault, l_layerMask);
        RaycastHit2D l_rhitLB = PhysicsTools.CreateRaycastHit(new Vector2(l_bounds.min.x, l_bounds.min.y), Vector2.down, PhysicsTools.m_rayDistDefault, l_layerMask);
        l_layerMask = LayerMask.GetMask("lWall");
        RaycastHit2D l_rhitL = PhysicsTools.CreateRaycastHitFrom(l_bounds, Toolbox.Direction.Left, PhysicsTools.m_rayDistDefault, l_layerMask);
        RaycastHit2D l_rhitR = PhysicsTools.CreateRaycastHitFrom(l_bounds, Toolbox.Direction.Right, PhysicsTools.m_rayDistDefault, l_layerMask);

        //Move on platform: side by side
        if ((!l_rhitRB.collider || l_rhitR.collider) && m_automataToken <= 1 && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            m_moveDir = Vector2.left;
            m_automataToken++;
        }
        else if ((!l_rhitLB.collider || l_rhitL.collider) && m_automataToken <= 1 && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            m_moveDir = Vector2.right;
            m_automataToken++;
        }

        //Follow to player
        if (m_automataToken >= 2)
        {
            //<(i)Unity’s GameObject class represents anything which can exist in a Scene. https://docs.unity3d.com/ScriptReference/GameObject.html
            Vector2 l_playerPos = GameObject.FindGameObjectWithTag("tPlayer").transform.position;
            Vector2 l_enemyPos = transform.position;

            if (l_rhitL.collider)
                m_moveDir = Vector2.right;
            else if (l_rhitR.collider)
                m_moveDir = Vector2.left;

            //MoveTo(m_moveDir, m_speed);//<(e)Apura el movimiento<optional>
            Vector2 l_automataDir = Vector2.zero;
            l_automataDir.y = l_playerPos.y - l_enemyPos.y;

            //Decide go up or fall/walk
            if (((int)l_automataDir.y) > 0)//<(e)Distance positive -> player is on roof-platform
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Moves.Jump(GetComponent<Rigidbody2D>(), 9.0f);
            }

            //Reset the persecution and it starts to move side by side
            if (GetComponent<Rigidbody2D>().velocity.y != 0)
            {
                m_automataToken = 0;
            }
        }

        MoveTo(m_moveDir, m_speed);
    }
}
