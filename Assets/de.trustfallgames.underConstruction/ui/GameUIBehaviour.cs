using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using TMPro;
using UnityEngine;

public class GameUIBehaviour : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private GameManager _gameManager;
    private int baseScore;
    
    void Start()
    {
        _gameManager = GameManager.GetManager();
        _scoreText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        baseScore = _gameManager.Settings.BasePoint;
        
        ChangeScore(0);
    }
    
    public void ChangeScore(int newScore)
    {
        _scoreText.SetText(newScore.ToString());
    }
}
