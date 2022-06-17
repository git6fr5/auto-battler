/* --- Unity --- */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class GameRules : MonoBehaviour {

    /* --- Variables --- */
    #region Variables

    // Tags.
    public static string PlayerTag = "Player";

    // Instance.
    public static GameRules Instance;

    // Arena.
    public Arena arena;
    public static Arena Arena => Instance.arena;

    // Shop.
    public Shop shop;
    public static Shop Shop => Instance.shop;


    // Camera.
    public static UnityEngine.Camera MainCamera => Camera.main;
    public static Vector2 MousePosition => (Vector2)MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    // Movement.
    public float movementPrecision = 0.05f;
    public static float MovementPrecision => Instance.movementPrecision;
    
    // Ticks.
    public static float Ticks;

    // Frame Rate.
    public float frameRate;
    public static float FrameRate => Instance.frameRate;

    // Collision
    public LayerMask characterCollisionLayer;
    public static LayerMask CharacterCollisionLayer => Instance.characterCollisionLayer;

    #endregion

    /* --- Unity --- */
    #region Unity

    void Start(){
        Init();
    }

    private void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        Ticks += deltaTime;
    }

    #endregion

    /* --- Initialization --- */
    #region Initialization

    private void Init() {
        Instance = this;
    }

    #endregion

}

