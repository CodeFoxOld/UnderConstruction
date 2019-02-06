﻿using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.UI;
using TMPro;
using UnityEngine;

public class GameUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _popupScore;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _destructCount;
    private GameManager _gameManager;
    private int baseScore;

    void Start()
    {
        _gameManager = GameManager.GetManager();
        baseScore = _gameManager.Settings.BasePoint;

        ChangeScore(0);
        ChangeDestructorCount(0);
    }

    public void ChangeScore(int newScore)
    {
        _scoreText.SetText(newScore.ToString());
    }

    public void ChangeDestructorCount(int newCount)
    {
        _destructCount.SetText(newCount.ToString());
    }

    public void PopScoreWithMultiplier(int multiplier)
    {
        GameObject popup = GameObject.Instantiate(_popupScore);

        popup.transform.SetParent(gameObject.transform, false);
        popup.transform.position = new Vector3(0, -200, 0);

        TextMeshProUGUI scoreOutput = popup.GetComponent<TextMeshProUGUI>();
        scoreOutput.SetText(multiplier.ToString() + " X " + baseScore);
    }

    public void OnDesctructibleButtonPressed(String direction)
    {
        _gameManager.MapManager.SpawnDesctructible(
            direction.Equals("vertical", StringComparison.CurrentCultureIgnoreCase)
                ? DestructibleDirection.vertical
                : DestructibleDirection.horizontal);
    }
}