/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

///<summary>
///
///<summary>
public class Effect : MonoBehaviour {

    VisualEffect vfx;
    SoundEffect sfx;

    public void Play() {
        if (vfx != null) {
            Play();
        }
        if (sfx != null) {
            Play();
        }
    }

}