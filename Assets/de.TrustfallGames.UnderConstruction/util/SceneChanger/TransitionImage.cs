using System;
using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.UI.title_screen;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.UI;

public class TransitionImage : MonoBehaviour {
    private Image image;

    [Range(0.01f, 1)]
    [SerializeField]
    private float fadeDuration = 0.05f;

    [Range(0.01f, 10)]
    [SerializeField]
    private float waitDuration = 1f;

    private bool sleep;

    // Start is called before the first frame update
    void Start() {
        image = GetComponent<Image>();
        if (!TransitionBitch.GetInstance().SpriteValid()) {
            sleep = true;
            Debug.Log("No Valid Image was found");
            return;
        }

        image.sprite = TransitionBitch.GetInstance().GetSprite();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (sleep) return;

        waitDuration -= Time.fixedDeltaTime;
        if (waitDuration > 0 && TransitionBitch.GetInstance().StartGamePaused()) return;

        var a = image.color;
        image.color = new Color(a.r, a.g, a.b, Mathf.Clamp(a.a - fadeDuration, 0, 1));
        if (Math.Abs(image.color.a) < 0.001) {
            image.enabled = false;
            sleep = true;
            if (TransitionBitch.GetInstance().StartGamePaused()) {
                GameManager.GetManager().GamePaused = false;
                TransitionBitch.GetInstance().ResetGamePaused();
            }
        }
    }

    public float WaitDuration => waitDuration;
}