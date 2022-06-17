/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///<summary>
public class Display : MonoBehaviour {

    [SerializeField] private Image image;
    [SerializeField] private Text text;

    [SerializeField] private float fadeSpeed;
    [SerializeField, ReadOnly] private float fade;
    [SerializeField, ReadOnly] private float fadeDirection;

    public void Update() {

        if (fadeDirection != 0f) {

            fade += fadeSpeed * fadeDirection * Time.deltaTime;
            SetFade();

            if ((fade >= 1f && fadeDirection > 0f) || (fade <= 0f && fadeDirection < 0f)) {
                fade = Mathf.Round(fade);
                fadeDirection = 0f;
                return;
            }

        }

    }

    public void Enable() {
        fade = 1f;
        SetFade();
    }

    public void Disable() {
        fade = 0f;
        SetFade();
    }

    private void SetFade() {
        Color imagecol = image.color;
        imagecol.a = fade;
        image.color = imagecol;

        Color textcol = text.color;
        textcol.a = fade;
        text.color = textcol;
    }

    public void SetText(string newtext) {
        text.text = newtext;
    }

    public void FadeIn() {
        fadeDirection = 1f;
    }

    public void FadeOut() {
        fadeDirection = -1f;
    }

    public void FadeIn(float delay) {
        StartCoroutine(IEFadeDelay(delay, 1f));
    }

    public void FadeOut(float delay) {
        StartCoroutine(IEFadeDelay(delay, -1f));
    }

    private IEnumerator IEFadeDelay(float delay, float direction) {
        yield return new WaitForSeconds(delay);
        fadeDirection = direction;
        yield return null;
    }

}