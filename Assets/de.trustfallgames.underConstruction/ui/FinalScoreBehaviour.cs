using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FinalScoreBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private TextMeshProUGUI _textMeshStages;

    private void OnEnable()
    {
        if (_gameManager.Character != null)
        {
            setFinalScore(_gameManager.Character.Highscore, _gameManager.Character.Height);
        }
    }

    private void Awake()
    {
        _gameManager = GameManager.GetManager();
    }

    private void setFinalScore(int finalScore, int stages)
    {
        _textMesh.text = finalScore.ToString();
        _textMeshStages.text = stages.ToString();
    }
    
}
