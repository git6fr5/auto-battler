/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gun : Weapon {

    /* --- Variables --- */
    #region Variables

    // Momemtum.
    [SerializeField] private float fireSpeed;
    [SerializeField] private Transform firePoint;

    // Projectile.
    [SerializeField] public Projectile projectile;

    #endregion

    #region Attack

    public override void Fire(Vector2 direction) {
        projectile.Fire(firePoint.position, direction, fireSpeed);
    }

    #endregion


}
