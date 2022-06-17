/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class Shop : MonoBehaviour {

    /* --- Variables --- */
    #region Variables

    // Content.
    [SerializeField] private int gold;
    public int Gold => gold;
    
    // Buttons.
    [SerializeField] private RollButton rollButton;
    [SerializeField] private PlayButton playButton;

    // Displays.
    [SerializeField] private Display winnerDisplay;
    [SerializeField] private Display roundDisplay;
    [SerializeField] private Display goldDisplay;
    
    #endregion

    public void Enable() {
        winnerDisplay.SetText("Winner: Team " + GameRules.Arena.winner.ToString());
        winnerDisplay.Enable();
        // winnerDisplay.FadeOut(1.5f);

        roundDisplay.SetText("Round Number: " + GameRules.Arena.roundNumber.ToString());
        roundDisplay.Enable();

        goldDisplay.SetText("Gold: " + gold.ToString());

        playButton.Enable();
        rollButton.Enable();
    }

    public void Disable() {
        winnerDisplay.Disable();
        roundDisplay.Disable();

        playButton.Disable();
        rollButton.Disable();
    }

    public void AddGold(int value) {
        gold += value;
    }

    public void Roll() {
        if (gold > 0) {
            // do a thing.
            print("Rolling");
            gold -= 1;
            goldDisplay.SetText("Gold: " + gold.ToString());
        }
    }

}