using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.GameTimeManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {
    [SerializeField] private SceneEnum sceneEnum;
    [SerializeField] private bool LoadAsync;
    [SerializeField] private bool FadeOver;
    [SerializeField] private Image FadeImage;

    private void Awake() {
        if (FadeImage != null) {
            FadeImage.enabled = false;
        }
    }

    private void FixedUpdate() { }

    
    
    public void ChangeScene() {
        if (FadeOver) {
            StartFade();
            return;
        }
        
        if (LoadAsync) {
            SceneManager.LoadSceneAsync((int) sceneEnum);
        } else {
            SceneManager.LoadScene((int) sceneEnum);
        }
    }

    private void StartFade() { throw new System.NotImplementedException(); }

    public void QuitApplication() { Application.Quit(); }

    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}