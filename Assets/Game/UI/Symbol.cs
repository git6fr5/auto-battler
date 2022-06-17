/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class Symbol : MonoBehaviour {

    public enum Justify {
        Left = 0, Center = -1
    }

    public static Symbol Instance;

    public Sprite coinSymbol;
    public static Sprite Coin => Instance.coinSymbol;
    
    public Sprite heartSymbol;
    public static Sprite Heart => Instance.heartSymbol;
    
    public Sprite speedSymbol;
    public static Sprite Speed => Instance.speedSymbol;

    void Start() {
        Instance = this;
    }

    public static void Create(Sprite sprite, int count, Transform parent, Vector3 center, float spacing, Justify justify) {
        
        float offset = (int)justify * (float)count / 2f;
        for (int i = 0; i < count; i++) {

            SpriteRenderer temp = new GameObject("Symbol" + i.ToString(), typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            temp.sprite = sprite;
            temp.transform.position = center + spacing * (i + offset) * Vector3.right;
            temp.transform.parent = parent;
        }
    }

}