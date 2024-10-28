using Managers;
using Movements;
using Settings;
using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(InputController), typeof(AnimeManager), typeof(AudioManager))]
public class Player : MonoBehaviour
{
    ///// SCRIPTABLE OBJECT /////
    [SerializeField] protected PlayerData m_data;

    ///// EVENT ACTIONS /////
    //** Observer pattern => Managing Game Events with the Event Bus
    public event System.Action EVT_STOMP;
    public event System.Action EVT_GOAL;
    public event System.Action EVT_COLLECT_TUNA;
    public event System.Action EVT_COLLECT_SOUL;

    ///// CONFIG INSPECTOR /////
    protected bool m_damaged = false;
    protected int m_lifes = 0;
    protected Vector2 m_spawnPos = Vector2.zero;
    public Vector2 SpawnPosition { get => m_spawnPos; set => m_spawnPos = value; }

    ///[SerializeField] protected EnemyGenericDamage m_enemy; //<(i) Observer Pattern <= Obsolete
    protected InputController m_inputController = null;
    protected AnimeManager m_animeManager = null;
    protected AudioManager m_audioManager = null;
    public PlayerData Data { get => m_data; }

    /// <summary>
    /// Indicate if player is defeated. eq.-lifes equals 0.
    /// </summary>
    public bool IsDefeated()
    {
        return m_data.Lifes == 0;
    }

    /// <summary>
    /// Reset player position to spawn position
    /// </summary>
    public void TranslateToSpawnPosition()
    {
        this.gameObject.transform.position = new Vector3(SpawnPosition.x, SpawnPosition.y, transform.position.z);
        Camera.main.GetComponent<FollowerCamera>().SetCameraPosition(SpawnPosition);
    }

    // Start is called before the first frame update: Start is only ever called once for a given script.
    //Source: https://docs.unity3d.com/Manual/ExecutionOrder.html
    protected void Start()
    {
        m_inputController = this.GetComponent<InputController>();
        m_animeManager = this.GetComponent<AnimeManager>();
        m_audioManager = this.GetComponent<AudioManager>();
    }

    // Update is called once per frame [variable intervale]
    protected void Update()
    {
        m_damaged = m_animeManager.IsPlaying("Damage");
        EnableActions();
    }

    //Sent when another object enters a trigger collider attached to this object (2D physics only).
    protected void OnTriggerEnter2D(Collider2D p_collider)
    {
#if GAME_DEBUG
        Debug.Log($"<DEBUG> {this.name} > Entered on trigger game-object: Name->{p_collider.name}. Tag->{p_collider.tag}");
#endif
        if (p_collider.isTrigger)
        {
            switch (p_collider.tag)
            {
                case "tAbyss":
                    OnDamaged();
                    break;
                case "tGoal":
                    OnGoal();
                    break;
                case "tItemTuna":
                    if (p_collider.isActiveAndEnabled)
                        EVT_COLLECT_TUNA?.Invoke();
                    break;
                case "tItemSoul":
                    EVT_COLLECT_SOUL?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    //Event handler: Sent when an incoming collider makes contact with this object's collider (2D physics only).
    protected void OnCollisionEnter2D(Collision2D p_collision)
    {
#if GAME_DEBUG
        Debug.Log($"<DEBUG> {this.name} > Touched a game-object: Name->{p_collision.collider.name}. Tag->{p_collision.collider.tag}");
#endif
        if (p_collision.collider.CompareTag("tEnemy"))
        {
            ContactPoint2D l_contactPoint = p_collision.GetContact(0);

            if (l_contactPoint.normal == Vector2.up)
                HandleStompCollision(l_contactPoint);
            else
                HandleEnemyDamageCollision(l_contactPoint);
        }
    }

    /// <summary>
    /// Action when player stomp a enemy - player's attack
    /// </summary>
    public bool HandleStompCollision(ContactPoint2D p_contactPoint)
    {
        if (p_contactPoint.normal != Vector2.up)
            return false;

        Moves.Stomp(this.GetComponent<Rigidbody2D>(), p_contactPoint.collider);
        EVT_STOMP?.Invoke();
        return true;
    }

    /// <summary>
    /// Action when player takes damage and respawn
    /// </summary>
    public bool HandleEnemyDamageCollision(ContactPoint2D p_contactPoint)
    {
        if (p_contactPoint.normal == Vector2.up)
            return false;

        OnDamaged();

        return true;
    }

    /// <summary>
    /// Action when player takes damage and respawn - Event Handler from enemy event
    /// </summary>
    protected void OnDamaged()
    {
        m_data.RemoveLifes();

        if (IsDefeated())
        {
            m_audioManager.OnDefeatSfx();
            m_animeManager.HandleDefeat();
            m_damaged = true;
        }
        else
        {
            m_audioManager.OnDamageSfx();
            m_animeManager.HandleDamage();
            m_damaged = true;
            //transform.position = m_data.SpawnPosition;
            TranslateToSpawnPosition();
        }
    }

    /// <summary>
    /// Enable Player for actions
    /// </summary>
    protected void EnableActions()
    {
        if (!m_damaged && !this.IsDefeated())
        {
            m_inputController.enabled = true;
            m_animeManager.HandleInput();
        }
    }

    /// <summary>
    /// Trigger the Goal Event when player arrives to goal
    /// </summary>
    protected void OnGoal()
    {
        LevelData l_levelData = GameObject.FindFirstObjectByType<Level>().Data;

        //if (m_data.Souls >= l_levelData.m_requiredSoulsQty)
        if (gameObject.GetComponent<ExperienceManager>().IsReadyGoal())
        {
            EVT_GOAL?.Invoke();
        }
    }

}

////////////////////////////////////////////////////////////
//
// Extensive Personal Documentation Notes
//
////////////////////////////////////////////////////////////

/**
 * CONCEPTO FUNDAMENTAL(!)
 * Ley accion-reaccion => 3ra de Newton => Serway Leyes de movimiento
 * Las fuerzas son interacciones entre dos objetos: cuando su dedo empuja sobre el libro, el libro empuja de vuelta sobre su dedo.
 *  La fuerza Fmc que ejerce el martillo sobre el clavo es igual en magnitud y opuesta a la fuerza Fcm que ejerce el clavo sobre el martillo.
 *  Esta última fuerza detiene el movimiento hacia adelante del martillo cuando golpea el clavo.
 *  Ley: Si dos objetos interactúan, la fuerza F(1->2) que ejerce el objeto 1 sobre el objeto 2 es igual en magnitud
 * y opuesta en dirección a la fuerza F(2->1) que ejerce el objeto 2 sobre el objeto 1.
 *  La fuerza de acción es igual en magnitud a la fuerza de reacción y opuesta en dirección. En todos los casos, las fuerzas de acción y reacción actúan en diferentes objetos SIEMPRE.
 * 
 * Cuando se coloca un monitor sobre una mesa, las fuerzas que actúan en el aparato son la fuerza normal n ejercida por la mesa
 * y la fuerza de gravedad Fg. La reacción a n es la fuerza ejercida por el monitor sobre la mesa, n'.
 * La reacción a Fg es la fuerza ejercida por el monitor sobre la Tierra, Fg'.
 * Un error común en la figura es considerar la fuerza normal sobre el objeto como la fuerza de reacción a la fuerza de la gravedad,
 * porque en este caso, estas dos fuerzas son iguales en magnitud y opuestas en dirección.
 * Sin embargo, esto es imposible, debido a que actúan en el mismo objeto.
 * 
 * El monitor no se acelera hacia abajo ya que la sostiene la mesa. Por lo tanto, la mesa ejerce una fuerza hacia arriba n,
 * conocida como la fuerza normal, sobre el monitor. (Normal, un término técnico de matemáticas, que en este contexto significa “perpendicular”.)
 * La fuerza normal es una fuerza elástica (¿que varia?) que surge a causa de la cohesión de la materia y es de origen electromagnético.
 * Equilibra la fuerza de gravitación que actúa sobre el monitor, evitando que este caiga a través de la mesa y
 * puede tener cualquier valor necesario, hasta el punto de romper la mesa.
 * 
 * Perpendicular => Relationship between two lines that meet at a right angle (90 degrees).
 */

/**
 * <(!!) Platform collision replaced by "Platform Effector 2D"
 * Effectors
 * The Platform Effector 2D applies various platform behavior such as one-way collisions, removal of side-friction/bounce and more.
 * Colliders used with the Effector are typically not set as triggers so that other colliders can collide with it.
 * Source: https://docs.unity3d.com/Manual/class-PlatformEffector2D.
 */

/**
 * <(!) OBSERVER
 * The observer pattern is a common solution to this sort of problem. 
 * It allows your objects to communicate but stay loosely coupled using a “one-to-many” dependency.
 * When one object changes states, all dependent objects get notified automatically.
 * This is analogous to a radio tower that broadcasts to many different listeners.
 * The object that is broadcasting is called the subject. The other objects that are listening are called the observers.
 * This pattern loosely decouples the subject, which doesn’t really know the observers or care what they do once they receive the signal.
 * While the observers have a dependency on the subject, the observers themselves don’t know about each other.
 * 
 *             //EVT_ENEMY_STOMPED?.Invoke(); //<(e) The ?. operator makes it ensures that "subject" doesn't attempt to raise the event when there are no subscribers to that event. 
 *             
 * Source: 
 * Game Development Patterns with Unity 2021 => chapter 6 Managing Game Events with the Event Bus
 * https://unity.com/resources/design-patterns-solid-ebook
 * https://learn.microsoft.com/en-us/dotnet/csharp/events-overview
 */
