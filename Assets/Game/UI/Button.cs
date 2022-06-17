using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Button : MonoBehaviour {

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public BoxCollider2D hitbox;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void OnMouseDown() {
        Activate();
    }

    protected abstract void Activate();

    public void Enable() {
        hitbox.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void Disable() {
        hitbox.enabled = false;
        spriteRenderer.enabled = false;
    }

}
