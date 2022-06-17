/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class RollButton : Button {

    protected override void Activate() {
        GameRules.Shop.Roll();
    }

}