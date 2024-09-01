using Settings;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float m_inputPlayerVertical = .0f;
    public float m_inputPlayerHorizontal = .0f;
    public Vector2 m_currentVelocity = Vector2.zero;
    protected bool m_jumped = false;
    protected bool m_doubleJumped = false;
    protected bool m_ducked = false;
    public bool Ducked
    {
        get { return m_ducked; }
    }

    public float m_jumpImpulse = Config.PLAYER_JUMP_IMPULSE;
    public float m_jumpImpulseUp = Config.PLAYER_JUMP_IMPULSE_UP;
    public float m_walkSpeed = Config.PLAYER_WALK_SPEED;
    public float m_walkSpeedUp = Config.PLAYER_WALK_SPEED_UP;
    public float m_walkSpeedDucking = Config.PLAYER_WALK_SPEED_DUCKING;
    public float m_duckUnderScale = Config.PLAYER_DUCKING_SCALE;

    // Start is called before the first frame update
    void Start()
    {
        //Don't erase!
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Assert(m_player == null);
        InputPlayerControl();
    }

    /// <summary>
    /// Player input controller
    /// </summary>
    void InputPlayerControl()
    {
        m_currentVelocity = GetComponent<Rigidbody2D>().GetPointVelocity(transform.position);
        m_inputPlayerVertical = Input.GetAxis("Vertical");
        m_inputPlayerHorizontal = Input.GetAxis("Horizontal");

        JumpManager(m_inputPlayerVertical);
        MoveManager(m_inputPlayerHorizontal);
    }

    /// <summary>
    /// Manager to the horizontal character's moves: to walk
    /// </summary>
    /// <param name="p_horizontalInput"></param>
    void MoveManager(float p_horizontalInput)
    {
        float l_throttle = 1.0f;

        if (Input.GetKey(KeyCode.Q))
        {
            l_throttle = m_walkSpeedUp;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Duck();
            l_throttle = m_walkSpeedDucking;
        }
        else //<(i) Only Debug: if (Input.GetKey(KeyCode.Z))
        {
            UnDuck();
        }

        Walk(m_walkSpeed * l_throttle, p_horizontalInput);
    }

    /// <summary>
    /// Manager to the character's jump and attack
    /// </summary>
    /// <param name="p_verticalInput"></param>
    /// <returns>True if the player character is jumping.</returns>
    bool JumpManager(float p_verticalInput)
    {

        //<(i) Simple Jump
        if (p_verticalInput > 0 && !m_jumped)
        {
            Jump(m_jumpImpulse);
        }
        //<(i) Double Jump!
        else if (m_jumped && !m_doubleJumped && p_verticalInput == 1)
        {
            Jump(m_jumpImpulse * m_jumpImpulseUp);
            m_doubleJumped = m_jumped;
        }
        //<(i) Landing
        else if (m_currentVelocity.y == 0 && m_jumped && p_verticalInput == 0)
        {
            m_jumped = m_doubleJumped = false;
        }

        return m_jumped;
    }

    /// <summary>
    /// Player character jumping
    /// </summary>
    /// <param name="p_impulse"></param>
    public void Jump(float p_impulse)
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * p_impulse, ForceMode2D.Impulse);
        m_jumped = true;
        //transform.Translate(Vector2.up * Time.deltaTime * m_inputPlayer);
    }

    /// <summary>
    /// Player character ploping (after the enemy defeating)
    /// </summary>
    /// <param name="p_impulse"></param>
    public void Plop(float p_impulse)
    {
        Vector2 l_velocity = GetComponent<Rigidbody2D>().velocity;
        l_velocity.y -= l_velocity.y;
        GetComponent<Rigidbody2D>().velocity = l_velocity;
        Jump(p_impulse);
    }

    /// <summary>
    /// Player character walking and running
    /// </summary>
    /// <param name="p_speed"></param>
    /// <param name="p_horizontalInput"></param>
    public void Walk(float p_speed, float p_horizontalInput)
    {
        float l_step = p_speed * Time.deltaTime * p_horizontalInput;//<(!) the move is more precise with horizontal input var
        //<(!) Moves the transform in the direction and distance of translation.
        // Source: https://docs.unity3d.com/ScriptReference/Transform.Translate.html
        transform.Translate(l_step, .0f, .0f);
    }

    /// <summary>
    /// Player character ducking
    /// </summary>
    public void Duck()
    {
        if (!m_ducked && transform.localScale.y == 1 && m_currentVelocity.y == 0)
        {
            m_ducked = true;
            Vector3 l_currentMaxBound = GetComponent<SpriteRenderer>().bounds.max;

            //Change scale transform
            Vector3 l_newScale = Vector3.one;
            l_newScale.y *= m_duckUnderScale;
            ChangeSpriteScale(l_newScale);
            //Change position transform
            Vector3 l_newMaxBound = GetComponent<SpriteRenderer>().bounds.max;
            Vector3 l_newOffset = l_currentMaxBound - l_newMaxBound;
            Vector3 l_currentPos = transform.position;
            l_currentPos.y -= l_newOffset.y;
            transform.position = l_currentPos;

            //<(i) Debug Only
            Color l_currentColor = GetComponent<SpriteRenderer>().color;
            l_currentColor.b = 1.0f;
            GetComponent<SpriteRenderer>().color = l_currentColor;
            Debug.Log("Ducked! Reduce: " + l_newScale);
        }
    }

    /// <summary>
    /// Player character unducking
    /// </summary>
    public void UnDuck()
    {
        if (m_ducked && transform.localScale.y < 1.0f)
        {
            m_ducked = false;
            Vector3 l_currentMaxBound = GetComponent<SpriteRenderer>().bounds.max;
            //Changes scale transform
            Vector3 l_newScale = Vector3.one;
            l_newScale.y = 2.0f;
            ChangeSpriteScale(l_newScale);
            //Change position transform
            Vector3 l_newMaxBound = GetComponent<SpriteRenderer>().bounds.max;
            Vector3 l_newOffset = l_currentMaxBound - l_newMaxBound;
            Vector3 l_currentPos = transform.position;
            l_currentPos.y -= l_newOffset.y;
            transform.position = l_currentPos;
            //<(i) Debug Only
            Color l_currentColor = GetComponent<SpriteRenderer>().color;
            l_currentColor.b = .0f;
            GetComponent<SpriteRenderer>().color = l_currentColor;
            Debug.Log("UnDucked! Grow: " + l_newScale);
        }
    }

    /// <summary>
    /// Resize a sprite scale
    /// </summary>
    /// <param name="p_newRelativeScale">A new relative size</param>
    public void ChangeSpriteScale(Vector3 p_newRelativeScale)
    {
        Vector3 l_currentScale = transform.localScale;
        Debug.Log("Change scale -> Before: " + l_currentScale);
        transform.localScale = Vector3.Scale(l_currentScale, p_newRelativeScale);
        Debug.Log("Change scale -> After: " + transform.localScale);
    }
}
