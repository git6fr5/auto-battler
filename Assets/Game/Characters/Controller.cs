/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Input))]
public class Controller : MonoBehaviour {

    /* --- Variables --- */
    #region Variables

    // Components.
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public BoxCollider2D hurtbox;
    [HideInInspector] public Input input;

    public bool running;
    public bool stopped => !running;

    // Health.
    [Space(2), Header("Health")]
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;

    // Movement.
    [Space(2), Header("Movement")]
    [SerializeField, Range(1f, 20f)] public float speed = 15f;
    [SerializeField, Range(1f, 100f)] protected float acceleration = 15f;
    [SerializeField, Range(0.9f, 0.995f)] protected float resistance = 0.985f;

    // Action.
    [Space(2), Header("Action")]
    [SerializeField] protected Weapon weapon;
    
    // Hurt.
    [Space(2), Header("Hurt")]
    [SerializeField, ReadOnly] public float hurtTicks;
    public bool hurt => hurtTicks != 0f;
    public static float HurtBuffer = 0.5f;

    // Knockback.
    [Space(2), Header("Knocked")]
    [SerializeField, ReadOnly] private float knockDuration = 0f;
    public bool knocked => knockDuration != 0f;

    // Debug.
    [Space(2), Header("Debug")]
    [SerializeField, ReadOnly] protected float debugSpeed;
    [HideInInspector] protected Vector3 previousPosition;

    #endregion

    /* --- Unity --- */
    #region Unity

    // Runs once before the first frame.
    void Start() {
        Init();
    }

    // Runs once every frame.
    void Update() {
        if (stopped) { return; }

        input.Get();
        Attack();
    }

    void FixedUpdate() {
        if (stopped) { return; }

        float deltaTime = Time.fixedDeltaTime;
        Hurt(deltaTime);
        Knocked(deltaTime);
        Movement(deltaTime);
        Debug(deltaTime);
    }

    #endregion

    /* --- Initialization --- */
    #region Initialization

    // Initializes this script.
    public virtual void Init() {
        // Caching components.
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hurtbox = GetComponent<BoxCollider2D>();
        input = GetComponent<Input>();
        health = maxHealth;
        ResetBody(body);
    }

    private static void ResetBody(Rigidbody2D rigidbody) {
        rigidbody.gravityScale = 0f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Run() {
        running = true;
    }

    public void Reset(Vector3 position) {
        running = false;

        transform.position = position;
        body.velocity = Vector2.zero;
        // health = maxHealth;
        input.Reset();
    }

    #endregion

    /* --- Processes --- */
    #region Processing

    private void Attack() {
        if (knocked) { return; }
        
        weapon.Point(transform.position, 0.5f, input.target);
        if (input.attack && weapon != null) {
            weapon.Activate(input.target);
        }
    }

    private void Movement(float deltaTime) {
        if (knocked) { return; }

        // Process the physics.
        Vector2 targetVelocity = (Vector3)input.movement.normalized * speed;
        Vector2 deltaVelocity = (targetVelocity - body.velocity) * acceleration * deltaTime;
        body.velocity += deltaVelocity;
        
        // Resistance
        if (targetVelocity == Vector2.zero) {
            body.velocity *= resistance;
        }
        // Check for released inputs.
        if (targetVelocity.y == 0f && Mathf.Abs(body.velocity.y) < GameRules.MovementPrecision) {
            body.velocity = new Vector2(body.velocity.x, 0f);
        }
        if (targetVelocity.x == 0f && Mathf.Abs(body.velocity.x) < GameRules.MovementPrecision) {
            body.velocity = new Vector2(0f, body.velocity.y);
        }

    }

    private void Knocked(float deltaTime) {
        knockDuration -= deltaTime;
        if (knockDuration <= 0) {
            knockDuration = 0f;
        }
    }

    private void Hurt(float deltaTime) {
        hurtTicks -= deltaTime;
        if (hurtTicks <= 0f) {
            hurtTicks = 0f;
        }
    }

    #endregion

    /* --- Collision --- */
    #region Collision

    public void Hit(Projectile projectile) {
        if (hurt) { return; }
        health -= projectile.Damage;
        Knock(projectile.body.velocity.normalized, 15f, HurtBuffer / 6f);
        CheckHealth();

        hurtTicks = HurtBuffer;
    }

    protected virtual void CheckHealth() {
        if (health <= 0) {
            Destroy(gameObject);
            return;
        }
    }

    public void Knock(Vector2 direction, float force, float duration) {
        knockDuration = duration;
        body.velocity = direction.normalized * force;
    }

    public void Heal(int value) {
        health += value;
    }

    #endregion

    /* --- Debug --- */
    #region Debug

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(transform.position, transform.position + (Vector3)input.target);
    }

    private void Debug(float deltaTime) {
        debugSpeed = (transform.position - previousPosition).magnitude / deltaTime;
        previousPosition = transform.position;
    }

    #endregion

}
