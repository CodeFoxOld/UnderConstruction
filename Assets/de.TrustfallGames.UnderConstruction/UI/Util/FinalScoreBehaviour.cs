using System;
using System.Linq;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.Util;
using de.TrustfallGames.UnderConstruction.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FinalScoreBehaviour : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _textMeshScore;
    [SerializeField] private TextMeshProUGUI _textMeshStages;
    [SerializeField] private TextMeshProUGUI _textMeshHighscore;
    [SerializeField] private TextMeshProUGUI _textMeshHighscoreIndicator;
    [SerializeField] private char seperator;

    private void OnEnable() {
        if (_gameManager.Character != null) {
            setFinalScore(_gameManager.Character.Highscore, _gameManager.Character.Height);
        }
    }

    private void Awake() { _gameManager = GameManager.GetManager(); }

    private void setFinalScore(int finalScore, int stages) {
        _textMeshScore.text = Separate(finalScore.ToString(), seperator);
        _textMeshStages.text = Separate(stages.ToString(),seperator);

        var a = PlayerPrefHandler.GetHighScore();

        _textMeshHighscore.text = Separate(a.ToString(), seperator);
        
        if (a == finalScore) {
            _textMeshHighscoreIndicator.text = "New Highscore!";
            SoundHandler.GetInstance().PlaySound(SoundName.NewHighscore);
        } else {
            SoundHandler.GetInstance().PlaySound(SoundName.GameOver);
            _textMeshHighscoreIndicator.text = "";
        }
    }

    private string Separate(string s, char separator) {
        int count = 0;
        string a = "";
        for (int i = s.Length - 1; i >= 0; i--) {
            a = string.Concat(a, s[i]);
            count++;
            if (count % 3 == 0) {
                a = string.Concat(a, separator);
            }
        }

        var b = s.ToCharArray();
        Array.Reverse(b);
        return new string(b);
    }
}