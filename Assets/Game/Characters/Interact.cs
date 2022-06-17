/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class Interact : MonoBehaviour {

    public static bool Click0 => UnityEngine.Input.GetMouseButtonDown(0);
    public static bool LetGo0 => UnityEngine.Input.GetMouseButtonUp(0);

    public bool stopped => !GameRules.Arena.paused;

    [SerializeField, ReadOnly] private bool hover;
    [SerializeField, ReadOnly] private bool selected;
    [SerializeField, ReadOnly] private bool dragging;

    public static void AddInteractibility(GameObject gameObject, Vector2 size) {
        Interact interact = new GameObject("Interact", typeof(BoxCollider2D), typeof(Interact)).GetComponent<Interact>();
        interact.GetComponent<BoxCollider2D>().size = size;

        interact.gameObject.layer = LayerMask.NameToLayer("UI");

        interact.transform.SetParent(gameObject.transform);
        interact.transform.localPosition = Vector2.zero;
        interact.transform.SetParent(interact.transform.parent.parent);
        gameObject.transform.SetParent(interact.transform);

    }

    private void OnMouseOver() {
        if (!stopped) {
            hover = true;
        }
    }

    private void OnMouseExit() {
        hover = false;
    }

    private void Update() {
        if (stopped) { return; }

        bool wasDraggin = dragging;

        selected = Interact.Click0 ? hover : selected;
        dragging = Interact.Click0 ? hover : Interact.LetGo0 ? false : dragging;

        if (dragging) {
            transform.position = (Vector3)GameRules.MousePosition;
        }
    }

}