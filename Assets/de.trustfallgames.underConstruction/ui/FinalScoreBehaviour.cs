using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using TMPro;
using UnityEngine;

public class FinalScoreBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField]private TextMeshProUGUI _textMesh;

    private void OnEnable()
    {
        if (_gameManager.Character != null)
        {
            setFinalScore(_gameManager.Character.Highscore);
        }
    }

    private void Awake()
    {
        _gameManager = GameManager.GetManager();
        _textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void setFinalScore(int finalScore)
    {
        _textMesh.text = finalScore.ToString();
    }
}
