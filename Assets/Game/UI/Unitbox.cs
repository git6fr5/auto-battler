/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class Unitbox : MonoBehaviour {

    [SerializeField] private Unit unit;
    private int cost => unit.cost;
    private int health => unit.controller.health;
    private float speed => unit.controller.speed;

    [SerializeField] public Transform costSymbolTransform;
    [SerializeField] public Transform healthSymbolTransform;
    [SerializeField] public Transform speedSymbolTransform  ;

    void Start() {
        CostSymbol();
    }

    public void CostSymbol() {
        Symbol.Create(Symbol.Coin, unit.cost, costSymbolTransform, costSymbolTransform.position, 0.275f, Symbol.Justify.Center);
        Symbol.Create(Symbol.Heart, unit.cost, healthSymbolTransform, healthSymbolTransform.position, 0.275f, Symbol.Justify.Left);
        Symbol.Create(Symbol.Speed, unit.cost, speedSymbolTransform, speedSymbolTransform.position, 0.275f, Symbol.Justify.Left);
        Interact.AddInteractibility(unit.gameObject, new Vector2(1f, 1f));
    }

}