/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///
///<summary>
public class Spritesheet : MonoBehaviour {

    /* --- Variables --- */
    #region Variables
    
    // Components.
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public  Controller controller;

    // Animation Data
    [Space(2), Header("Data")]
    [SerializeField] private Material[] materials;
    [SerializeField] private Sprite[] sprites;
    [HideInInspector] private Sprite[] currentAnimation;
    [HideInInspector] private Sprite[] previousAnimation;
    [SerializeField, ReadOnly] private int currentFrame;
    [HideInInspector] private int previousFrame;
    [HideInInspector] private float ticks;

    // Frames
    [Space(2), Header("Slicing")]
    [SerializeField] private int idleFrames;
    [SerializeField] private int movementFrames;

    // Effects.
    [Space(2), Header("Effects")]
    [SerializeField] private Effect stepEFX;

    // Animations
    [HideInInspector] private Sprite[] idleAnimation;
    [HideInInspector] private Sprite[] movementAnimation;

    // Conditions.
    private bool moving => controller.input.movement.sqrMagnitude > 0f;
    private float direction => controller.input.movement.x;
    private bool step => currentAnimation == movementAnimation && currentFrame == 0 && previousFrame != 0;
    
    #endregion
    
    /* --- Unity --- */
    #region Unity

    void Start() {
        Init();
    }

    void Update() {
        float deltaTime = Time.deltaTime;
        if (sprites != null && sprites.Length > 0) {
            Animate(deltaTime);
        }
        Rotate();
        Hurt();
    }

    #endregion

    /* --- Initialization --- */
    #region Initialization

    public virtual void Init() {
        // Caching components.
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<Controller>();
        if (sprites != null && sprites.Length > 0) {
            Organize();
        }
        spriteRenderer.materials = materials;
    }

    // Organizes the sprite sheet into its animations.
    public virtual int Organize() {
        int startIndex = 0;
        startIndex = SliceSheet(startIndex, idleFrames, ref idleAnimation);
        startIndex = SliceSheet(startIndex, movementFrames, ref movementAnimation);        
        return startIndex;
    }

    private int SliceSheet(int startIndex, int length, ref Sprite[] array) {
        List<Sprite> splicedSprites = new List<Sprite>();
        for (int i = startIndex; i < startIndex + length; i++) {
            splicedSprites.Add(sprites[i]);
        }
        array = splicedSprites.ToArray();
        return startIndex + length;
    }

    #endregion

    /* --- Rendering --- */
    #region Rendering

    private void Animate(float deltaTime) {
        currentAnimation = GetAnimation();
        ticks = previousAnimation == currentAnimation ? ticks + deltaTime : 0f;
        currentFrame = (int)Mathf.Floor(ticks * GameRules.FrameRate) % currentAnimation.Length;
        spriteRenderer.sprite = currentAnimation[currentFrame];
        
        GetEffect();

        previousAnimation = currentAnimation;
        previousFrame = currentFrame;
    }

    // Gets the current animation info.
    public virtual Sprite[] GetAnimation() {
        if (moving) {
            return movementAnimation;
        }
        return idleAnimation;
    }

    private void GetEffect() {
        if (step && stepEFX != null) {
            stepEFX.Play();
        }

    }

    protected virtual void Rotate() {
        if (direction < 0f) {
            RotateBody(controller.body, 180f);
        } 
        else if (direction > 0f) {
            RotateBody(controller.body, 0f);
        }
    }

    private static void RotateBody(Rigidbody2D body, float angle) {
        if (body.transform.eulerAngles == angle * Vector3.up) { return; }
        body.constraints = RigidbodyConstraints2D.None;
        body.transform.eulerAngles = angle * Vector3.up;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Hurt() {
        if (controller.knocked) {
            spriteRenderer.color = Color.blue;
        }
        else if (controller.hurt) {
            spriteRenderer.color = Color.red;
        }
        else {
            spriteRenderer.color = Color.white;
            float ratio = (float)controller.health / (float)controller.maxHealth;
            if (ratio <= 0.5f) {
                spriteRenderer.color = new Color(1f, 0.25f + ratio, 0.25f + ratio, 0.45f + ratio);
            }
        }

    }

    public static void AfterImage(SpriteRenderer spriteRenderer, Transform transform, float delay, float transparency) {
        SpriteRenderer afterImage = new GameObject("AfterImage", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        afterImage.transform.position = transform.position;
        afterImage.transform.localRotation = transform.localRotation;
        afterImage.transform.localScale = transform.localScale;
        afterImage.sprite = spriteRenderer.sprite;
        afterImage.color = Color.white * transparency;
        afterImage.sortingLayerName = spriteRenderer.sortingLayerName;
        Destroy(afterImage.gameObject, delay);
    }

    #endregion

}