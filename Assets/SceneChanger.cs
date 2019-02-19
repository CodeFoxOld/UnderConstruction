using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.GameTimeManager;
using de.TrustfallGames.UnderConstruction.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    [SerializeField] private SceneEnum sceneEnum;
    [SerializeField] public bool LoadAsync;

    public void ChangeScene() {
        if (LoadAsync) {
            SceneManager.LoadSceneAsync((int) sceneEnum);
        } else {
            SceneManager.LoadScene((int) sceneEnum);
        }
    }

    public void QuitApplication() { Application.Quit(); }

    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}