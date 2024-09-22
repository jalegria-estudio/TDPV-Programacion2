using Movements;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class Player : MonoBehaviour
{
    //** Observer pattern => Managing Game Events with the Event Bus
    public event System.Action EVT_PLAYER_DAMAGED;
    public event System.Action EVT_ENEMY_STOMPED;

    [Header("CONFIG")]
    [SerializeField] protected int m_lifes = 5;
    [SerializeField] protected Vector2 m_spawnPosition = Vector2.zero;
    ///[SerializeField] protected EnemyGenericDamage m_enemy; //<(i) Observer Pattern <= Obsolete

    public int Lifes { get => m_lifes; }
    public void RemoveLifes(int p_qty = 1)
    {
        if (m_lifes > 0 && m_lifes >= p_qty)
            m_lifes -= p_qty;
    }

    public void AddLifes(int p_qty = 1)
    {
        m_lifes += p_qty;
    }

    protected InputController m_inputController = null;

    // Start is called before the first frame update
    protected void Start()
    {
        m_inputController = new InputController();
    }

    // Update is called once per frame [variable intervale]
    protected void Update()
    {

    }

    protected void FixedUpdate()
    {

    }

    //Sent when another object enters a trigger collider attached to this object (2D physics only).
    protected void OnTriggerEnter2D(Collider2D p_collider)
    {
        Debug.Log($"<DEBUG> {this.name} > Entered on trigger game-object: Name->{p_collider.name}. Tag->{p_collider.tag}");

        if (p_collider.isTrigger && p_collider.CompareTag("tAbyss"))
        {
            OnDamaged();
        }
    }

    //Event handler: Sent when an incoming collider makes contact with this object's collider (2D physics only).
    protected void OnCollisionEnter2D(Collision2D p_collision)
    {
        Debug.Log($"<DEBUG> {this.name} > Touched a game-object: Name->{p_collision.collider.name}. Tag->{p_collision.collider.tag}");

        if (p_collision.collider.CompareTag("tEnemy"))
        {
            ContactPoint2D l_contactPoint = p_collision.GetContact(0);

            if (l_contactPoint.normal == Vector2.up)
                HandleStompCollision(l_contactPoint);
            //EVT_ENEMY_STOMPED?.Invoke(); //<(e) The ?. operator makes it ensures that "subject" doesn't attempt to raise the event when there are no subscribers to that event.
            else
                HandleEnemyDamageCollision(l_contactPoint);
            //EVT_PLAYER_DAMAGED?.Invoke(); // Debug.Log($"<DEBUG> {this.name} > event invoked: EVT_PLAYER_DAMAGED");
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
        RemoveLifes();
        transform.position = m_spawnPosition;
    }
}

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
 * Source: 
 * Game Development Patterns with Unity 2021 => chapter 6 Managing Game Events with the Event Bus
 * https://unity.com/resources/design-patterns-solid-ebook
 * https://learn.microsoft.com/en-us/dotnet/csharp/events-overview
 */
