/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public abstract class Input : MonoBehaviour {

    [SerializeField, ReadOnly] public Vector2 movement = new Vector2(0f, 0f);
    [SerializeField, ReadOnly] public bool attack = false;
    [SerializeField, ReadOnly] public Vector2 target = new Vector2(1f, 0f);

    public abstract void Get();

    public void Reset() {
        movement = new Vector2(0f, 0f);
        attack = false;
        target = new Vector2(1f, 0f);
    }

}