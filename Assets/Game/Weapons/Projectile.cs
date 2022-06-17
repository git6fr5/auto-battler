/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    /* --- Variables --- */
    #region Variables.

    // Components.
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public CircleCollider2D hitbox;

    // Settings.
    [SerializeField] private float lifeTime = 3f;

    // Damage
    [SerializeField] private int damage;
    public int Damage => damage;

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start() {
        body = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate() {
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Controller controller = collision.collider.GetComponent<Controller>();
        if (controller is not null) {
            controller.Hit(this);
            Destroy(gameObject);
        }
    }

    #endregion

    /* --- Initalization --- */
    #region Initialization

    public void Fire(Vector2 origin, Vector2 direction, float speed) {
        Projectile projectile = Instantiate(gameObject, origin, Quaternion.identity, null).GetComponent<Projectile>();
        projectile.Init(direction, speed);
        Destroy(projectile.gameObject, lifeTime);
    }
   
    void Init(Vector2 direction, float speed) {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        gameObject.SetActive(true);
        body.velocity = direction * speed;
    }

    #endregion

}
