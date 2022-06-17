/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Weapon : MonoBehaviour {

    /* --- Variables --- */
    #region Variables

    // Components.
    [HideInInspector] protected SpriteRenderer spriteRenderer;

    // User.
    [SerializeField, ReadOnly] private Controller user;
    [SerializeField] private float fireCooldown;
    [SerializeField, ReadOnly] private float cooldown;

    #endregion

    /* --- Uniry --- */
    #region Unity

    void Start() {
        Init();
    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Cooldown(deltaTime);
    }

    #endregion

    /* --- Initialization --- */
    #region Initialization

    private void Init() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        user = transform.parent.GetComponent<Controller>();
    }

    #endregion

    #region Attack

    public void Point(Vector2 pivot, float radius, Vector2 direction) {
        transform.position = (Vector3)(pivot + radius * direction);

        Vector2 quad = new Vector2(Mathf.Abs(direction.x), direction.y);
        transform.eulerAngles = Vector2.SignedAngle(Vector2.right, quad) * Vector3.forward;
        if (direction.x < 0) {
            transform.eulerAngles += 180f * Vector3.up;
        }
    }


    public virtual void Activate(Vector2 direction) {
        if (cooldown > 0f) {
            return;
        }
        Fire(direction);
        cooldown = fireCooldown;
    }

    public abstract void Fire(Vector2 direction);

    private void Cooldown(float deltaTime) {
        cooldown -= deltaTime;
        if (cooldown < 0f) {
            cooldown = 0f;
        }
    }

    #endregion

    /* --- Debug --- */
    #region Debug

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
    }

    #endregion


}
